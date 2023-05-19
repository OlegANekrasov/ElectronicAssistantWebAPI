using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicAssistantWebAPI.DAL.Models
{
    [Table("RecommendedPrescriptions")]
    public class RecommendedPrescription
    {
        public string Id { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
    }
}
