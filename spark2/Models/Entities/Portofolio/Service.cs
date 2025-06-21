using spark2.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; } 
       // public string? ServiceImage { get; set; }
        public string? ServiceDescription { get; set; }

        public byte[]? ServiceImage { get; set; }

        [ForeignKey("Portofolio")]
        public int PortofolioId { get; set; }
        public Portofolio Portofolio { get; set; }       

    }
}
