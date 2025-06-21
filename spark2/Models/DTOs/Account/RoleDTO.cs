using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs.Account
{
    public class RoleDTO
    {
        [Required,StringLength(256)]
        public string RoleName { get; set; }
    }
}
