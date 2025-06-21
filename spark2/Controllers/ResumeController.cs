using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using spark2.Models.DTOs;
using spark2.Models.Entities;
using spark2.Repos.ResumeRepositary;
using spark2.Services;
using System.Configuration.Provider;
using System.Security.Claims;

namespace spark2.Controllers
{
    
    public class ResumeController : Controller
    {
        private readonly IResumeRepo _resumeRepo;
        private readonly IResumeParserService _parser;

        public ResumeController(IResumeRepo resumeRepo, IResumeParserService parser)
        {
            _resumeRepo = resumeRepo;
            _parser = parser;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allResumes = await _resumeRepo.GetAllResumesAsync();
            var userResumes = allResumes.Where(r => r.PersonId == userId).ToList();
            return View(userResumes);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var resume = await _resumeRepo.GetResumeByIdAsync(id);
            if (resume == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!resume.IsPublished && resume.PersonId != userId)
               return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            return View(resume);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBasicResume(ResumeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var newResume = new Resume
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Summary = dto.Summary,
                Address = dto.Address,
                Title = dto.Title,
                Link1 = dto.Link1,
                Link2 = dto.Link2,
                Link3 = dto.Link3,
                PersonId = userId,
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                
            };

            var result = await _resumeRepo.CreateResumeBasicAsync(newResume);
            return Ok(result.ResumeId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEducation(  EducationDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UniversityName) || dto.ResumeId == 0)
            {
                return Ok();
            }

            var newEducation = new Education
            {
                UniversityName = dto.UniversityName,
                DegreeType = dto.DegreeType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Major = dto.Major,
                GPA = dto.GPA,
                ResumeId = dto.ResumeId
            };

              await _resumeRepo.AddEducationAsync(dto.ResumeId, newEducation);
            return Ok(newEducation.EducationId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddExperience(  ExperienceDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CompanyName) || dto.ResumeId == 0)
            {
                return Ok();
            }

            var newExperience = new Experience
            {
                CompanyName = dto.CompanyName,
                Position = dto.Position,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsCurrent = dto.IsCurrent,
                Duties = dto.Duties,
                ResumeId = dto.ResumeId
            };

            await _resumeRepo.AddExperienceAsync(dto.ResumeId, newExperience);
            return Ok(newExperience.ExperienceId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLanguage(  LanguageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LanguageName) || dto.ResumeId == 0)
            {
                return Ok();
            }

            var newLanguage = new Language
            {
                LanguageName = dto.LanguageName,
                LanguageLevel = dto.LanguageLevel,
                ResumeId = dto.ResumeId
            };

            await _resumeRepo.AddLanguageAsync(dto.ResumeId, newLanguage);
            return Ok(newLanguage.LanguageId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSkill(  SkillDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SkillName) || dto.ResumeId == 0)
            {
                return Ok();
            }

            var newSkill = new Skill
            {
                SkillName = dto.SkillName,
                SkillType = dto.SkillType,
                ResumeId = dto.ResumeId
            };

              await _resumeRepo.AddSkillAsync(dto.ResumeId, newSkill);
            return Ok(newSkill.SkillId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCertificate(CertificateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TopicName) || dto.ResumeId == 0)
            {
                return Ok();
            }

            var newCertificate = new Certificate
            {
                TopicName = dto.TopicName,
                ProviderName = dto.ProviderName,
                IssuedDate = dto.IssuedDate,
                ExpirationDate = dto.ExpirationDate,
                CertificateUrl = dto.CertificateUrl,
                ResumeId = dto.ResumeId
            };
             await _resumeRepo.AddCertificateAsync(dto.ResumeId, newCertificate);
            return Ok(newCertificate.CertificateId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateBasicResume(ResumeDTO dto)
        {
            var resume = await _resumeRepo.GetResumeByIdAsync(dto.ResumeId);
            if (resume == null)
                return NotFound();

            resume.ResumeId = dto.ResumeId;
            resume.FirstName = dto.FirstName;
            resume.LastName = dto.LastName;
            resume.Email = dto.Email;
            resume.PhoneNumber = dto.PhoneNumber;
            resume.Summary = dto.Summary;
            resume.Address = dto.Address;
            resume.Title = dto.Title;
            resume.Link1 = dto.Link1;
            resume.Link2 = dto.Link2;
            resume.Link3 = dto.Link3;
            resume.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);            

            await _resumeRepo.UpdateResumeBasicAsync(resume);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _resumeRepo.DeleteResumeAsync(id);
            if (!success) return NotFound();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePublishStatus(int id, bool publish)
        {
            var success = await _resumeRepo.SetPublishStatusForResumeAsync(id, publish);
            if (!success) return RedirectToPage("/Identity/Account/AccessDenied");

            if (publish)
            {
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Index");
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var resume = await _resumeRepo.GetResumeByIdAsync(id);

            if (resume == null)
                return NotFound();

            var dto = new ResumeDTO
            {
                ResumeId = resume.ResumeId,
                FirstName = resume.FirstName,
                LastName = resume.LastName,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Summary = resume.Summary,
                Address = resume.Address,
                Title = resume.Title,
                Link1 = resume.Link1,
                Link2 = resume.Link2,
                Link3 = resume.Link3,
                
            };

            ViewBag.Educations = resume.Educations.Select(e => new EducationDTO
            {
                EducationId = e.EducationId,
                ResumeId = e.ResumeId,
                UniversityName = e.UniversityName,
                DegreeType = e.DegreeType,
                Major = e.Major,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                GPA = e.GPA
            }).ToList();

            ViewBag.Experiences = resume.Experiences.Select(ex => new ExperienceDTO
            {
                ExperienceId = ex.ExperienceId,
                ResumeId = ex.ResumeId,
                CompanyName = ex.CompanyName,
                Position = ex.Position,
                StartDate = ex.StartDate,
                EndDate = ex.EndDate,
                Duties = ex.Duties,
                IsCurrent = ex.IsCurrent
            }).ToList();

            ViewBag.Skills = resume.Skills.Select(s => new SkillDTO
            {
                SkillId = s.SkillId,
                ResumeId = s.ResumeId,
                SkillName = s.SkillName,
                SkillType = s.SkillType
            }).ToList();

            ViewBag.Languages = resume.Languages.Select(l => new LanguageDTO
            {
                LanguageId = l.LanguageId,
                ResumeId = l.ResumeId,
                LanguageName = l.LanguageName,
                LanguageLevel = l.LanguageLevel
            }).ToList();

            ViewBag.Certificates = resume.Certificates.Select(c => new CertificateDTO
            {
                CertificateId = c.CertificateId,
                ResumeId = c.ResumeId,
                TopicName = c.TopicName,
                ProviderName = c.ProviderName,
                ExpirationDate = c.ExpirationDate,
                 IssuedDate = c.IssuedDate,                 
                CertificateUrl = c.CertificateUrl
            }).ToList();

            return View(dto);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await _resumeRepo.GetEducationByIdAsync(id);
            if (education == null)
                return NotFound();

            await _resumeRepo.DeleteEducationAsync(id);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experince = await _resumeRepo.GetExperienceByIdAsync(id);
            if (experince == null)
                return NotFound();

            await _resumeRepo.DeleteExperienceAsync(id);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var language = await _resumeRepo.GetLanguageByIdAsync(id);
            if (language == null)
                return NotFound();

            await _resumeRepo.DeleteLanguageAsync(id);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _resumeRepo.GetSkillByIdAsync(id);
            if (skill == null)
                return NotFound();

            await _resumeRepo.DeleteSkillAsync(id);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var certificate = await _resumeRepo.GetCertificateByIdAsync(id);
            if (certificate == null)
                return NotFound();

            await _resumeRepo.DeleteCertificateAsync(id);
            return Ok();

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateEducation([FromBody] List<EducationDTO> educations)
        {
            if (educations == null ||educations.Count == 0)
                return Ok();            
            foreach (var dto in educations)
            {
                if (dto.EducationId == 0)
                {
                   
                    var newEducation = new Education
                    {
                        ResumeId = dto.ResumeId,
                        EducationId = dto.EducationId,
                        UniversityName = dto.UniversityName,
                        DegreeType = dto.DegreeType,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        Major = dto.Major,
                        GPA = dto.GPA,                       
                    };
                    await _resumeRepo.AddEducationAsync(dto.ResumeId, newEducation);
                }
                else
                {
                    
                    var existingEducation = await _resumeRepo.GetEducationByIdAsync(dto.EducationId);
                    if (existingEducation == null) continue;

                    existingEducation.UniversityName = dto.UniversityName;
                    existingEducation.DegreeType = dto.DegreeType;
                    existingEducation.StartDate = dto.StartDate;
                    existingEducation.EndDate = dto.EndDate;
                    existingEducation.Major = dto.Major;
                    existingEducation.GPA = dto.GPA;  


                    await _resumeRepo.UpdateEducationAsync(existingEducation);
                }
            }

            return Ok();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateExperience([FromBody] List<ExperienceDTO> experiences)
        {          
            if (experiences == null || experiences.Count == 0)
                return Ok();
            foreach (var dto in experiences)
            {
                if (dto.ExperienceId == 0)
                {

                    var newExperince = new Experience
                    {
                        ResumeId = dto.ResumeId,
                        ExperienceId = dto.ExperienceId,
                        CompanyName = dto.CompanyName,
                        Position = dto.Position,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        IsCurrent = dto.IsCurrent,
                        Duties = dto.Duties,
                       
                    };
                    await _resumeRepo.AddExperienceAsync(dto.ResumeId, newExperince);
                }
                else
                {
                    
                    var existingExperince = await _resumeRepo.GetExperienceByIdAsync(dto.ExperienceId);
                    if (existingExperince == null) continue;

                    existingExperince.ExperienceId = dto.ExperienceId;
                    existingExperince.CompanyName = dto.CompanyName;
                    existingExperince.Position = dto.Position;
                    existingExperince.StartDate = dto.StartDate;
                    existingExperince.EndDate = dto.EndDate;
                    existingExperince.IsCurrent = dto.IsCurrent;
                    existingExperince.Duties = dto.Duties;


                    await _resumeRepo.UpdateExperienceAsync(existingExperince);
                }
            }

            return Ok();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateLanguage([FromBody] List<LanguageDTO> languages)
        {
            if (languages == null || languages.Count == 0)
                return Ok();
            foreach (var dto in languages)
            {
                if (dto.LanguageId == 0)
                {

                    var newLanguage = new Language
                    {
                        ResumeId = dto.ResumeId,
                        LanguageId = dto.LanguageId,
                        LanguageName = dto.LanguageName,
                        LanguageLevel = dto.LanguageLevel,

                    };
                    await _resumeRepo.AddLanguageAsync(dto.ResumeId, newLanguage);
                }
                else
                {                    
                    var existingLanguage = await _resumeRepo.GetLanguageByIdAsync(dto.LanguageId);
                    if (existingLanguage == null) continue;

                    existingLanguage.LanguageId = dto.LanguageId;
                    existingLanguage.LanguageName = dto.LanguageName;
                    existingLanguage.LanguageLevel = dto.LanguageLevel;


                    await _resumeRepo.UpdateLanguageAsync(existingLanguage);
                }
            }

            return Ok();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateSkill([FromBody] List<SkillDTO> skills)
        {
            if (skills == null || skills.Count == 0)
                return Ok();
            foreach (var dto in skills)
            {
                if (dto.SkillId == 0)
                {

                    var newSkill = new Skill
                    {
                        ResumeId = dto.ResumeId,
                        SkillId = dto.SkillId,
                        SkillName = dto.SkillName,
                        SkillType = dto.SkillType,

                    };
                    await _resumeRepo.AddSkillAsync(dto.ResumeId, newSkill);
                }
                else
                {                    
                    var existingSkill = await _resumeRepo.GetSkillByIdAsync(dto.SkillId);
                    if (existingSkill == null) continue;

                    existingSkill.SkillId = dto.SkillId;
                    existingSkill.SkillName = dto.SkillName;
                    existingSkill.SkillType = dto.SkillType;

                    await _resumeRepo.UpdateSkillAsync(existingSkill);
                }
            }

            return Ok();

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCertificate([FromBody] List<CertificateDTO> certificates)
        {          
            if (certificates == null || certificates.Count == 0)
                return Ok();
            foreach (var dto in certificates)
            {
                if (dto.CertificateId == 0)
                {

                    var newCertificate = new Certificate
                    {
                        ResumeId = dto.ResumeId,
                        CertificateId = dto.CertificateId,
                        TopicName = dto.TopicName,
                        ProviderName = dto.ProviderName,
                        IssuedDate = dto.IssuedDate,
                        ExpirationDate = dto.ExpirationDate,
                        CertificateUrl = dto.CertificateUrl,

                    };
                    await _resumeRepo.AddCertificateAsync(dto.ResumeId, newCertificate);
                }
                else
                {                    
                    var existingCertificate = await _resumeRepo.GetCertificateByIdAsync(dto.CertificateId);
                    if (existingCertificate == null) continue;

                    existingCertificate.CertificateId = dto.CertificateId;
                    existingCertificate.TopicName = dto.TopicName;
                    existingCertificate.ProviderName = dto.ProviderName;
                    existingCertificate.IssuedDate = dto.IssuedDate;
                    existingCertificate.ExpirationDate = dto.ExpirationDate;
                    existingCertificate.CertificateUrl = dto.CertificateUrl;

                    await _resumeRepo.UpdateCertificateAsync(existingCertificate);
                }
            }

            return Ok();

        }



        [Authorize]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Parse([FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var resume = await _parser.ParseAndSaveAsync(rawText,userId);

            // return View("ParsedResume", resume);
            return RedirectToAction("Index");
        }


    }
}