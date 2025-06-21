using spark2.Models;
using Microsoft.SemanticKernel;
using spark2.Models.Entities;
using System.Text.Json;
using spark2.Data;



namespace spark2.Services
{
    public class ResumeParserService : IResumeParserService
    {
        private readonly Kernel _kernel;
        private readonly ApplicationDbContext _dbContext;

        public ResumeParserService(Kernel kernel, ApplicationDbContext context)
        {
            _kernel = kernel;
            this._dbContext = context;
        }

        public async Task<Resume> ParseAndSaveAsync(string rawText, string personId)
        {
            // Parse the raw resume text to get the Resume object
            var resume = await ParseResumeAsync(rawText);

            resume.PersonId = personId;
            resume.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            resume.ModifiedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            resume.IsDeleted = false;
            resume.IsPublished = false;
            // Save to database
            _dbContext.Resumes.Add(resume);
            await _dbContext.SaveChangesAsync();

            return resume;
        }

        public async Task<Resume> ParseResumeAsync(string rawText)
        {
            var prompt = @"
You are a professional data extraction assistant.
Your task is to read the following unstructured resume text and convert it into clean, structured JSON using the schema below.
If any field is missing or unclear, use your reasoning skills to infer a suitable value or default to 'Not provided'. 
You should auto-generate a brief professional summary if none is explicitly provided. Also, infer skill categories, job duties, and education details where appropriate. Be concise, consistent, and accurate.


Schema:
{  
  FirstName: string,
  LastName: string,  
  Email: string,
  PhoneNumber: string,
  Summary: string,  // Generate if missing
  Address: string,
  Title: string,
  Link1: string,
  Link2: string,
  Link3: string,
  PersonId: string,
  
  Educations: [
    {       
       UniversityName: string,
       DegreeType: string,
       StartDate: string,
       EndDate:string,
       Major: string,
       GPA: string
 ,       
    }
  ],
  
  Experiences: [
    {      
      CompanyName: string,
      Position: string,
      StartDate: string,
      EndDate: string,
      IsCurrent: boolean,
      Duties: string     // Generate from position if not provided
    }
  ],
  
  Skills: [
    {     
      SkillName: string,
      SkillType: string   // Choose from: Technical, Soft, Language, Management, Analytical, Creative   
    }
  ],
  
  Languages: [
    {      
       LanguageName: string,
       LanguageLevel: string    //choose from: Beginner, Intermediate, Fluent, Native  
    }
  ],
  
  Certificates: [
    {      
      TopicName: string,
      ProviderName: string,
      IssuedDate:string,
      ExpirationDate: string,
      CertificateUrl: string     
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
            //BASE 64
            var json = result.ToString();

            var resume = JsonSerializer.Deserialize<Resume>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return resume!;
        }


    }
}


