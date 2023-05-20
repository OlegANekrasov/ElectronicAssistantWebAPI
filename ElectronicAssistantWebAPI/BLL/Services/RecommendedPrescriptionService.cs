using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;
using ElectronicAssistantWebAPI.DAL.Repository;
using Microsoft.VisualBasic.FileIO;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public class RecommendedPrescriptionService : IRecommendedPrescriptionService
    {
        private readonly IRepository<RecommendedPrescription> _recommendedPrescriptionRepository;
        private readonly ILogger<RecommendedPrescriptionService> _logger;

        public RecommendedPrescriptionService(IRepository<RecommendedPrescription> recommendedPrescriptionRepository, 
                                              ILogger<RecommendedPrescriptionService> logger)
        {
            _recommendedPrescriptionRepository = recommendedPrescriptionRepository;
            _logger = logger;
        }

        public IEnumerable<RecommendedPrescription> Get()
        {
            return ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRooms();
        }

        public async Task<RecommendedPrescription> GetByIdAsync(string id)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRecommendedPrescriptionByIdAsync(id);
        }

        public async Task<RecommendedPrescription> AddAsync(AddRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).AddRoomAsync(model);
        }

        public async Task<RecommendedPrescription> UpdateAsync(UpdateRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).UpdateRoomAsync(model);
        }

        public async Task DeleteAsync(DelRecommendedPrescription model)
        {
            await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).DeleteRoomAsync(model.Id);
        }

        public async Task PostFileAsync(IFormFile fileData)
        {
            try
            {
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
