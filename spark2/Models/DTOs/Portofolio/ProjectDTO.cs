using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
       
        public string? ProjectName { get; set; }  
        public string? ProjectDescription { get; set; }  
        public string? ProjectLink { get; set; }
         public string? ProjectAttachment { get; set; }


        public byte[]? ProjectImage { get; set; }
       

        public int PortofolioId { get; set; }        

    }
}
