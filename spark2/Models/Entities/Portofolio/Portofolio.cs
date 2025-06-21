using Microsoft.CodeAnalysis;
using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Portofolio
    {
        public int PortofolioId { get; set; }

        public string FullName { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }


        public string Email { get; set; }
        public byte[]? PersonalImage { get; set; }
       
        public string? Address { get; set; }
        public DateOnly PortofolioCreatedDate { get; set; }
        public DateOnly PortofolioModifiedDate { get; set; }
        public bool IsDeleted { get; set; }        
        public ICollection<Service> Services { get; set; } =new List<Service>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public bool IsPublished { get; set; } = false;


        [ForeignKey("Person")]
        public string PersonId { get; set; }
        public Person Person { get; set; }


    }
}
