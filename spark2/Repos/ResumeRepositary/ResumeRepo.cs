using Microsoft.EntityFrameworkCore;
using spark2.Data;

using spark2.Models.Entities;

namespace spark2.Repos.ResumeRepositary
{
    public class ResumeRepo : IResumeRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public ResumeRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        #region Basic Resume
        public async Task<Resume> CreateResumeBasicAsync(Resume resume)
        {
            var newResume = new Resume
            {
                FirstName = resume.FirstName,
                LastName = resume.LastName,
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsDeleted = false,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Summary = resume.Summary,
                Address = resume.Address,
                Title = resume.Title,
                Link1 = resume.Link1,
                Link2 = resume.Link2,
                Link3 = resume.Link3,
                IsPublished = false,
                PersonId = resume.PersonId
            };

            _dbContext.Resumes.Add(newResume);
            await _dbContext.SaveChangesAsync();
            return newResume;
        }


        public async Task<Resume?> GetResumeByIdAsync(int id)
        {
            return await _dbContext.Resumes
              .Include(r => r.Educations)
              .Include(r => r.Experiences)
              .Include(r => r.Languages)
              .Include(r => r.Skills)
              .Include(r => r.Certificates)
              .FirstOrDefaultAsync(r => r.ResumeId == id && !r.IsDeleted);
        }


        public async Task<List<Resume>> GetAllResumesAsync()
        {
            return await _dbContext.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .Include(r => r.Skills)
                .Include(r => r.Languages)
                .Include(r => r.Certificates)
                .Where(r => !r.IsDeleted)

                .ToListAsync();
        }


        public async Task<Resume> UpdateResumeBasicAsync(Resume resume)
        {
            var existingResume = await _dbContext.Resumes.FindAsync(resume.ResumeId);
            if (existingResume == null) return null;

            existingResume.FirstName = resume.FirstName;
            existingResume.LastName = resume.LastName;
            existingResume.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            existingResume.Email = resume.Email;
            existingResume.PhoneNumber = resume.PhoneNumber;
            existingResume.Summary = resume.Summary;
            existingResume.Address = resume.Address;
            existingResume.Title = resume.Title;
            existingResume.Link1 = resume.Link1;
            existingResume.Link2 = resume.Link2;
            existingResume.Link3 = resume.Link3;
            existingResume.IsPublished = resume.IsPublished;

            await _dbContext.SaveChangesAsync();
            return existingResume;
        }


        public async Task<bool> DeleteResumeAsync(int id)
        {
            var resume = await _dbContext.Resumes.FindAsync(id);
            if (resume == null) return false;

            resume.IsDeleted = true;
            resume.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task<bool> SetPublishStatusForResumeAsync(int resumeId, bool publish)
        {
            var resume = await _dbContext.Resumes.FindAsync(resumeId);
            if (resume == null)
                return false;

            resume.IsPublished = publish;
           resume.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        #endregion


        #region Education CRUD

        public async Task<Education> AddEducationAsync(int resumeId, Education education)
        {
            education.ResumeId = resumeId;

            _dbContext.Educations.Add(education);
            await _dbContext.SaveChangesAsync();
            return education;
        }

        public async Task<Education?> GetEducationByIdAsync(int educationId)
        {
            return await _dbContext.Educations.FindAsync(educationId);
        }



        public async Task<Education> UpdateEducationAsync(Education education)
        {
            var existingEducation = await _dbContext.Educations.FindAsync(education.EducationId);
            if (existingEducation == null) return null;

            existingEducation.UniversityName = education.UniversityName;
            existingEducation.DegreeType = education.DegreeType;
            existingEducation.StartDate = education.StartDate;
            existingEducation.EndDate = education.EndDate;
            existingEducation.Major = education.Major;
            existingEducation.GPA = education.GPA;
            existingEducation.ResumeId = education.ResumeId;

            await _dbContext.SaveChangesAsync();
            return existingEducation;
        }

        public async Task<bool> DeleteEducationAsync(int educationId)
        {
            var education = await _dbContext.Educations.FindAsync(educationId);
            if (education == null) return false;

            _dbContext.Educations.Remove(education);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        #endregion


        #region Language CRUD

        public async Task<Language> AddLanguageAsync(int resumeId, Language language)
        {
            language.ResumeId = resumeId;

            _dbContext.Languages.Add(language);
            await _dbContext.SaveChangesAsync();
            return language;
        }

        public async Task<Language?> GetLanguageByIdAsync(int languageId)
        {
            return await _dbContext.Languages.FindAsync(languageId);
        }


        public async Task<Language> UpdateLanguageAsync(Language language)
        {
            var existingLanguage = await _dbContext.Languages.FindAsync(language.LanguageId);
            if (existingLanguage == null) return null;

            existingLanguage.LanguageName = language.LanguageName;
            existingLanguage.LanguageLevel = language.LanguageLevel;
            existingLanguage.ResumeId = language.ResumeId;

            await _dbContext.SaveChangesAsync();
            return existingLanguage;
        }

        public async Task<bool> DeleteLanguageAsync(int languageId)
        {
            var language = await _dbContext.Languages.FindAsync(languageId);
            if (language == null) return false;

            _dbContext.Languages.Remove(language);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        #endregion


        #region Experience CRUD
        public async Task<Experience> AddExperienceAsync(int resumeId, Experience experience)
        {
            experience.ResumeId = resumeId;
            _dbContext.Experiences.Add(experience);
            await _dbContext.SaveChangesAsync();
            return experience;
        }

        public async Task<Experience?> GetExperienceByIdAsync(int experienceId)
        {
            return await _dbContext.Experiences.FindAsync(experienceId);
        }

        public async Task<Experience?> UpdateExperienceAsync(Experience experience)
        {
            var existingExperience = await _dbContext.Experiences.FindAsync(experience.ExperienceId);
            if (existingExperience == null) return null;

            existingExperience.CompanyName = experience.CompanyName;
            existingExperience.Position = experience.Position;
            existingExperience.StartDate = experience.StartDate;
            existingExperience.EndDate = experience.EndDate;
            existingExperience.IsCurrent = experience.IsCurrent;
            existingExperience.Duties = experience.Duties;
            existingExperience.ResumeId = experience.ResumeId;

            await _dbContext.SaveChangesAsync();
            return existingExperience;
        }

        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
            var experience = await _dbContext.Experiences.FindAsync(experienceId);
            if (experience == null) return false;

            _dbContext.Experiences.Remove(experience);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion


        #region Skill CRUD
        public async Task<Skill> AddSkillAsync(int resumeId, Skill skill)
        {
            skill.ResumeId = resumeId;
            _dbContext.Skills.Add(skill);
            await _dbContext.SaveChangesAsync();
            return skill;
        }

        public async Task<Skill?> GetSkillByIdAsync(int skillId)
        {
            return await _dbContext.Skills.FindAsync(skillId);
        }

        public async Task<Skill?> UpdateSkillAsync(Skill skill)
        {
            var existingSkill = await _dbContext.Skills.FindAsync(skill.SkillId);
            if (existingSkill == null) return null;

            existingSkill.SkillName = skill.SkillName;
            existingSkill.SkillType = skill.SkillType;
            existingSkill.ResumeId = skill.ResumeId;

            await _dbContext.SaveChangesAsync();
            return existingSkill;
        }

        public async Task<bool> DeleteSkillAsync(int skillId)
        {
            var skill = await _dbContext.Skills.FindAsync(skillId);
            if (skill == null) return false;

            _dbContext.Skills.Remove(skill);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion


        #region Certificate CRUD
        public async Task<Certificate> AddCertificateAsync(int resumeId, Certificate certificate)
        {
            certificate.ResumeId = resumeId;
            _dbContext.Certificates.Add(certificate);
            await _dbContext.SaveChangesAsync();
            return certificate;
        }

        public async Task<Certificate?> GetCertificateByIdAsync(int certificateId)
        {
            return await _dbContext.Certificates.FindAsync(certificateId);
        }

        public async Task<Certificate?> UpdateCertificateAsync(Certificate certificate)
        {
            var existingCertificate = await _dbContext.Certificates.FindAsync(certificate.CertificateId);
            if (existingCertificate == null) return null;

            existingCertificate.TopicName = certificate.TopicName;
            existingCertificate.ProviderName = certificate.ProviderName;
            existingCertificate.IssuedDate = certificate.IssuedDate;
            existingCertificate.ExpirationDate = certificate.ExpirationDate;
            existingCertificate.CertificateUrl = certificate.CertificateUrl;
            existingCertificate.ResumeId = certificate.ResumeId;

            await _dbContext.SaveChangesAsync();
            return existingCertificate;
        }

        public async Task<bool> DeleteCertificateAsync(int certificateId)
        {
            var certificate = await _dbContext.Certificates.FindAsync(certificateId);
            if (certificate == null) return false;

            _dbContext.Certificates.Remove(certificate);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
