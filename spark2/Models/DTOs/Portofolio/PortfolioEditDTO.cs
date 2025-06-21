using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs.Portofolio
{
    public class PortfolioEditDTO
    {
        public PortofolioDTO Portofolio { get; set; } = new PortofolioDTO();
        public List<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
        public List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
    }
}
