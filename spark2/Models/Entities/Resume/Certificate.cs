using System.ComponentModel.DataAnnotations.Schema;

namespace spark2.Models.Entities
{
    public class Certificate
    {
        public int CertificateId { get; set; }        
        public string? TopicName { get; set; }
        public string? ProviderName { get; set; }
        public string? IssuedDate { get; set; }
        public string? ExpirationDate { get; set; }
        public string? CertificateUrl { get; set; }


        [ForeignKey("Resume")]
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
