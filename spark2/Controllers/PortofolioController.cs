using spark2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using spark2.Models.DTOs;
using spark2.Repos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using spark2.Services;
using Microsoft.AspNetCore.Authorization;
using Humanizer;
using spark2.Models.DTOs.Portofolio;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace spark2.Controllers
{
    public class PortofolioController : Controller
    {
        private readonly IPortofolioRepo _portofolioRepo;
        private readonly IPortofolioParserService _parser;

        public PortofolioController(IPortofolioRepo portofolioRepo, IPortofolioParserService parser)
        {
            _portofolioRepo = portofolioRepo;
            _parser = parser;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allPortfolios = await _portofolioRepo.GetAllPortofoliosAsync();

            var userPortfolios = allPortfolios.Where(p => p.PersonId == userId).ToList();

            return View(userPortfolios);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var portfolio = await _portofolioRepo.GetPortofolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!portfolio.IsPublished && portfolio.PersonId != userId)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            return View(portfolio);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBasicPortofolio(PortofolioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            ///image byte
            byte[]? imageBytes = null;

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files["PersonalImageFile"];


                if (file != null)
                {
                    if (file.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("PersonalImage", "File size must be less than 2MB.");
                        return BadRequest(ModelState);
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("PersonalImage", "Only .jpg, .jpeg, and .png files are allowed.");
                        return BadRequest(ModelState);
                    }

                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    imageBytes = dataStream.ToArray();
                }
            }

            var newPortofolio = new Portofolio
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PersonalImage = imageBytes,
                Bio = dto.Bio,
                JobTitle = dto.JobTitle,
                Address = dto.Address,
                PortofolioCreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsDeleted = false,
                IsPublished = false,
                PersonId = userId
            };

            var result = await _portofolioRepo.CreatePortofolioBasicAsync(newPortofolio);
            return Ok(result.PortofolioId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddService(ServiceDTO dto)
        {

            if (string.IsNullOrWhiteSpace(dto.ServiceName) || dto.PortofolioId == 0)
            {
                return Ok();
            }

            byte[]? imageBytes = null;

            var file = Request.Form.Files["ServiceImageFile"];
            if (file != null)
            {
                if (file.Length > 2 * 1024 * 1024)
                    return BadRequest("Service image must be less than 2MB.");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Only .jpg, .jpeg, .png allowed.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
            var newService = new Service
            {
                ServiceName = dto.ServiceName,
                ServiceImage = imageBytes,
                ServiceDescription = dto.ServiceDescription,
                PortofolioId = dto.PortofolioId,
            };
            await _portofolioRepo.AddServiceToPortofolioAsync(dto.PortofolioId, newService);
            return Ok(newService.ServiceId);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProjectName) || dto.PortofolioId == 0)
            {
                return Ok();
            }

            byte[]? imageBytes = null;

            var file = Request.Form.Files["ProjectImageFile"];
            if (file != null)
            {
                if (file.Length > 2 * 1024 * 1024)
                    return BadRequest("Project image must be less than 2MB.");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Only .jpg, .jpeg, .png allowed.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var newProject = new Project
            {
                ProjectName = dto.ProjectName,
                ProjectDescription = dto.ProjectDescription,
                ProjectLink = dto.ProjectLink,
                ProjectAttachment = dto.ProjectAttachment,
                ProjectImage = imageBytes,
            };

            await _portofolioRepo.AddProjectToPortofolioAsync(dto.PortofolioId, newProject);

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _portofolioRepo.DeletePortofolioAsync(id);
            if (!success) return NotFound();

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.PortfolioId = id;

            // return View();
            var portfolio = await _portofolioRepo.GetPortofolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var dto = new PortofolioDTO
            {
                PortofolioId = portfolio.PortofolioId,
                FullName = portfolio.FullName,
                Email = portfolio.Email,
                JobTitle = portfolio.JobTitle,
                Bio = portfolio.Bio,
                Address = portfolio.Address,
                PersonalImage = portfolio.PersonalImage
            };

            ViewBag.Services = portfolio.Services.Select(s => new ServiceDTO
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                ServiceDescription = s.ServiceDescription,
                ServiceImage = s.ServiceImage,
                PortofolioId = s.PortofolioId
            }).ToList();

            ViewBag.Projects = portfolio.Projects.Select(p => new ProjectDTO
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                ProjectDescription = p.ProjectDescription,
                ProjectLink = p.ProjectLink,
                ProjectAttachment = p.ProjectAttachment,
                ProjectImage = p.ProjectImage,
                PortofolioId = p.PortofolioId
            }).ToList();

            return View(dto);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBasicPortofolio([FromForm] PortofolioDTO dto)
        {
            var portfolio = await _portofolioRepo.GetPortofolioByIdAsync(dto.PortofolioId);
            if (portfolio == null)
                return NotFound();

            var file = Request.Form.Files["PersonalImageFile"];
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("PersonalImage", "Only .jpg, .jpeg, and .png files are allowed.");
                    return BadRequest(ModelState);
                }

                if (file.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("PersonalImage", "File size must be less than 2MB.");
                    return BadRequest(ModelState);
                }

                using var dataStream = new MemoryStream();
                await file.CopyToAsync(dataStream);
                portfolio.PersonalImage = dataStream.ToArray();
            }

            portfolio.FullName = dto.FullName;
            portfolio.Email = dto.Email;
            portfolio.JobTitle = dto.JobTitle;
            portfolio.Bio = dto.Bio;
            portfolio.Address = dto.Address;

            await _portofolioRepo.UpdatePortofolioBasicAsync(dto.PortofolioId, portfolio);

            return Ok();

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateService(ServiceDTO dto)
        {
            if (dto.PortofolioId == 0)
                return BadRequest("Missing or invalid PortofolioId.");


            byte[]? imageBytes = null;

            var file = Request.Form.Files["ServiceImageFile"];
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Only .jpg, .jpeg, and .png files are allowed.");

                if (file.Length > 2 * 1024 * 1024)
                    return BadRequest("File size must be under 2MB.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            if (dto.ServiceId == 0)
            {
                // New service
                var newService = new Service
                {
                    PortofolioId = dto.PortofolioId,
                    ServiceName = dto.ServiceName,
                    ServiceDescription = dto.ServiceDescription,
                    ServiceImage = imageBytes
                };
                await _portofolioRepo.AddServiceToPortofolioAsync(dto.PortofolioId, newService);
                return Ok(newService.ServiceId);

            }
            // Update existing
            var existingService = await _portofolioRepo.GetServiceByIdAsync(dto.ServiceId);


            existingService.ServiceName = dto.ServiceName;
            existingService.ServiceDescription = dto.ServiceDescription;
            if (imageBytes != null)
                existingService.ServiceImage = imageBytes;
            await _portofolioRepo.UpdateServiceAsync(existingService);
            return Ok(existingService.ServiceId);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProject(ProjectDTO dto)
        {
            if (dto.PortofolioId == 0)
                return BadRequest("Invalid portfolio ID.");


            byte[]? imageBytes = null;

            var file = Request.Form.Files["ProjectImageFile"];
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Only .jpg, .jpeg, and .png files are allowed.");

                if (file.Length > 2 * 1024 * 1024)
                    return BadRequest("File size must be under 2MB.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }


            if (dto.ProjectId == 0)
            {
                var newProject = new Project
                {
                    PortofolioId = dto.PortofolioId,
                    ProjectName = dto.ProjectName,
                    ProjectDescription = dto.ProjectDescription,
                    ProjectLink = dto.ProjectLink,
                    ProjectAttachment = dto.ProjectAttachment,
                    ProjectImage = imageBytes
                };
                await _portofolioRepo.AddProjectToPortofolioAsync(dto.PortofolioId, newProject);
                return Ok(newProject.ProjectId);

            }

            var existingProject = await _portofolioRepo.GetProjectByIdAsync(dto.ProjectId);
            if (existingProject == null)
                return NotFound();

            existingProject.ProjectName = dto.ProjectName;
            existingProject.ProjectDescription = dto.ProjectDescription;
            existingProject.ProjectLink = dto.ProjectLink;
            existingProject.ProjectAttachment = dto.ProjectAttachment;

            if (imageBytes != null)
                existingProject.ProjectImage = imageBytes;

            await _portofolioRepo.UpdateProjectAsync(existingProject);
            return Ok();

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _portofolioRepo.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();

            await _portofolioRepo.DeleteServiceAsync(id);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _portofolioRepo.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            await _portofolioRepo.DeleteProjectAsync(id);
            return Ok();
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePublishStatus(int id, bool publish)
        {
            var success = await _portofolioRepo.SetPublishStatusForPortofolioAsync(id, publish);
            if (!success)
                return NotFound();

            if (publish)
            {
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }


        private byte[]? TryConvertBase64(string? base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return null;

            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch
            {
                return null; // Or optionally log the error
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Parse([FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // Parse and save via the parser service
            var portofolio = await _parser.ParseAndSaveAsync(rawText, userId);

            // Display it
            //return View("ParsedPortofolio", portofolio);
            return RedirectToAction("Index");
        }
    }
}




