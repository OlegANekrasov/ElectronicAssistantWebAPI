using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.BLL.ViewModels;
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
        Task<FileUploadResultModel> PostFileAsync(IFormFile fileData); 
        IEnumerable<string> GetPositions();
        ProtocolAnalysisResultViewModel GetProtocolAnalysis(string IdFileUpload, string position);
        IEnumerable<string> GetIdFilesUpload();
    }
}
