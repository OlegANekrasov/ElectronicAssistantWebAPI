using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;
using Microsoft.VisualBasic.FileIO;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public interface IRecommendedPrescriptionService
    {
        IEnumerable<RecommendedPrescription> Get();
        Task<RecommendedPrescription> GetByIdAsync(string id);
        Task<RecommendedPrescription> AddAsync(AddRecommendedPrescription model);
        Task<RecommendedPrescription> UpdateAsync(UpdateRecommendedPrescription model);
        Task DeleteAsync(DelRecommendedPrescription model);
        public Task<FileUploadResultModel> PostFileAsync(IFormFile fileData);
    }
}
