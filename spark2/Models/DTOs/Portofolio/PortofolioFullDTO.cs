using spark2.Models.DTOs;

namespace spark2.Models.DTOs.Portofolio
{
    public class PortofolioFullDTO
    {
        public PortofolioDTO Portofolio { get; set; } 
        public List<ProjectDTO> Projects { get; set; } 
        public List<ServiceDTO> Services { get; set; } 
    }
}
