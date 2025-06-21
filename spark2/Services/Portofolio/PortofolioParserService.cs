using spark2.Models.DTOs.Portofolio;
using spark2.Data;
using spark2.Models.Entities;
using Microsoft.SemanticKernel;
using System.Text.Json;

namespace spark2.Services
{
    public class PortofolioParserService : IPortofolioParserService
    {
        private readonly Kernel _kernel;
        private readonly ApplicationDbContext _dbContext;

        public PortofolioParserService(Kernel kernel, ApplicationDbContext context)
        {
            _kernel = kernel;
            _dbContext = context;
        }

        public async Task<Portofolio> ParseAndSaveAsync(string rawText, string personId)
        {
            var portofolio = await ParsePortofolioAsync(rawText);

             
            portofolio.PersonId = personId;
            portofolio.PortofolioCreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            portofolio.PortofolioModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            portofolio.IsDeleted = false;
            portofolio.IsPublished = false;

             
            _dbContext.Portofolios.Add(portofolio);
            await _dbContext.SaveChangesAsync();

            return portofolio;
        }

        public async Task<Portofolio> ParsePortofolioAsync(string rawText)
        {
            var prompt = @"
You are a professional data extraction assistant.
Your task is to read the following freeform user portfolio text and convert it into clean, structured JSON using the schema provided below.
If any field is missing or unclear, fill it in with a reasonable guess based on available context or default to 'Not provided'. If an image is not provided, use null (do NOT use 'Not provided').
Also, if bio, job title, service descriptions, or project descriptions are weak or missing, use your reasoning skills to auto-generate short, relevant summaries based on the provided content.

Schema:
{
  FullName: string,
  Email: string,         
  Bio: string,                      
  JobTitle: string,
  Address: string?,   
  PersonalImage: string?, // base64 encoded string or null
  Services: [
    {
      ServiceName: string,     
      ServiceDescription: string,
      ServiceImage: string? // base64 encoded string or null
    }
  ],
  Projects: [
    {
      ProjectName: string,          
      ProjectDescription: string,   
      ProjectLink: string?,         
      ProjectAttachment: string?,   
      ProjectImage: string? // base64 encoded string or null
    }
  ]
}

CV TEXT:
{{$input}}

JSON:
";

            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);

            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });

            var json = result.ToString();

            var dto = JsonSerializer.Deserialize<PortofolioExtractionDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dto == null)
                throw new Exception("Failed to parse portfolio JSON.");

             
            var portofolio = new Portofolio
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Bio = dto.Bio,
                JobTitle = dto.JobTitle,
                Address = dto.Address,
                PersonalImage = TryConvertBase64(dto.PersonalImage),
                Services = dto.Services.Select(s => new Service
                {
                    ServiceName = s.ServiceName,
                    ServiceDescription = s.ServiceDescription,
                    ServiceImage = TryConvertBase64(s.ServiceImage)
                }).ToList(),
                Projects = dto.Projects.Select(p => new Project
                {
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    ProjectLink = p.ProjectLink,
                    ProjectAttachment = p.ProjectAttachment,
                    ProjectImage = TryConvertBase64(p.ProjectImage)
                }).ToList()
            };

            return portofolio;
        }

        private static byte[]? TryConvertBase64(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return null;

            try
            {
                return Convert.FromBase64String(base64);
            }
            catch
            {
                return null;
            }
        }
    }
}
