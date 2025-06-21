using System.ComponentModel.DataAnnotations;

namespace spark2.Models.DTOs
{
    public class CertificateDTO
    {
        public int CertificateId { get; set; }               
        public string? TopicName { get; set; }               
        public string? ProviderName { get; set; }
        public string? IssuedDate { get; set; }
        public string? ExpirationDate { get; set; }
        public string? CertificateUrl { get; set; }


        public int ResumeId { get; set; }
    }
}
