using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.DTOs
{

    public class PortofolioDTO
    {
        public int PortofolioId { get; set; }

        [Required(ErrorMessage = "Please Enter you Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bio is required.")]
        public string Bio { get; set; } = string.Empty;

        [Required(ErrorMessage = "JobTitle is required")]
        public string JobTitle { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Please Enter your email")]
        public string Email { get; set; } = string.Empty;

       
        public byte[]? PersonalImage { get; set; }

           
        public string? Address { get; set; }  
        public DateOnly PortofolioCreatedDate { get; set; }
        public DateOnly PortofolioModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool IsPublished { get; set; } = false;

        //  public string PersonId { get; set; }

        [NotMapped]
        public IFormFile? PersonalImageFile { get; set; }

    }

}

