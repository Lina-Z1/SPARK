namespace spark2.Models.DTOs.Portofolio
{
    public class PortofolioExtractionDto
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public string Bio { get; set; } = "";
        public string? Address { get; set; }
        public string? PersonalImage { get; set; }   

        public List<ServiceExtractionDto> Services { get; set; } = new();
        public List<ProjectExtractionDto> Projects { get; set; } = new();
    }

    public class ServiceExtractionDto
    {
        public string ServiceName { get; set; } = "";
        public string ServiceDescription { get; set; } = "";
        public string? ServiceImage { get; set; } 
    }

    public class ProjectExtractionDto
    {
        public string ProjectName { get; set; } = "";
        public string ProjectDescription { get; set; } = "";
        public string? ProjectLink { get; set; }
        public string? ProjectAttachment { get; set; }
        public string? ProjectImage { get; set; }  
    }
}

