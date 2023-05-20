using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.EF;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.DAL.Repository
{
    public class RecommendedPrescriptionRepository : Repository<RecommendedPrescription>
    {
        public RecommendedPrescriptionRepository(ApplicationDbContext db) : base(db)
        {

        }

        public IEnumerable<RecommendedPrescription> GetRecommendedPrescriptions()
        {
            return base.Get();
        }

        public async Task<RecommendedPrescription> GetRecommendedPrescriptionByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<RecommendedPrescription> AddRecommendedPrescriptionAsync(AddRecommendedPrescription model)
        {
            var recommendedPrescription = new RecommendedPrescription()
            {
                Id = Guid.NewGuid().ToString(),
                Diagnosis = model.Diagnosis,
                Prescription = model.Prescription
            };

            await CreateAsync(recommendedPrescription);
            return recommendedPrescription;
        }

        public async Task<RecommendedPrescription> UpdateRecommendedPrescriptionAsync(UpdateRecommendedPrescription model)
        {
            var recommendedPrescription = await GetByIdAsync(model.Id);
            if (recommendedPrescription != null)
            {
                recommendedPrescription.Diagnosis = model.Diagnosis;
                recommendedPrescription.Prescription = model.Prescription;

                await UpdateAsync(recommendedPrescription);
                return recommendedPrescription;
            }

            return null;
        }

        public async Task DeleteRecommendedPrescriptionAsync(string id)
        {
            await DeleteAsync(id);
        }
    }
}
