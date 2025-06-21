using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class ResumeDTO
    {
        public int ResumeId { get; set; }

        [Required(ErrorMessage = "Please Enter you  Name")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Please Enter you Email")]
        public string Email { get; set; }


        public string? PhoneNumber { get; set; }
        public string? Summary { get; set; }
        public string? Address { get; set; }  
        public string? Title { get; set; }
        public string? Link1 { get; set; }  
        public string? Link2 { get; set; }  
        public string? Link3 { get; set; }    

        public bool IsPublished { get; set; } = false;

    }
}
