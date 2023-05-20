using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public interface IPrescriptionProtocolService
    {
        IEnumerable<PrescriptionProtocol> Get();
        Task<PrescriptionProtocol> GetByIdAsync(string id);
        Task<PrescriptionProtocol> AddAsync(AddPrescriptionProtocol model);
        Task<PrescriptionProtocol> UpdateAsync(UpdatePrescriptionProtocol model);
        Task DeleteAsync(DelPrescriptionProtocol model);
        public Task<FileUploadResultModel> PostFileAsync(IFormFile fileData);
    }
}
