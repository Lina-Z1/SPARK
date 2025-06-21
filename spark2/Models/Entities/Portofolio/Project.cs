using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }  
        public string? ProjectDescription { get; set; }  
        public string? ProjectLink { get; set; }  
        public string? ProjectAttachment { get; set; }

       public byte[]? ProjectImage { get; set; }


        [ForeignKey("Portofolio")]
        public int PortofolioId { get; set; }
        public Portofolio Portofolio { get; set; }

       


    }
}
