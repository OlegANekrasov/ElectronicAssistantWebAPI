using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public interface IRecommendedPrescriptionService
    {
        IEnumerable<RecommendedPrescription> Get();
        Task<RecommendedPrescription> GetByIdAsync(string id);
        Task<RecommendedPrescription> AddAsync(AddRecommendedPrescription model);
        Task<RecommendedPrescription> UpdateAsync(UpdateRecommendedPrescription model);
        Task DeleteAsync(DelRecommendedPrescription model);
    }
}
