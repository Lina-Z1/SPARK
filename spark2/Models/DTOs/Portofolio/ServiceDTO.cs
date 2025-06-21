using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class ServiceDTO
    {

        public int ServiceId { get; set; }
        
        public string? ServiceName { get; set; }  

      //  public string? ServiceImage { get; set; }
        public string? ServiceDescription { get; set; }

        public byte[]? ServiceImage { get; set; }
         


        public int PortofolioId { get; set; }

    }
}
