using Microsoft.AspNetCore.Identity;
using spark2.Models.Entities;
 

namespace spark2.Models.Entities
{
    public class Person : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }

        public ICollection<Portofolio> Portofolios { get; set; }
        public ICollection<Resume> Resumes { get; set; }
    }
}
