using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.BLL.ViewModels
{
    public class ProtocolAnalysisResultViewModel
    {
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public int Type3 { get; set; }
        public List<ProtocolAnalysisResult> ProtocolAnalysisResults { get; set; } = new List<ProtocolAnalysisResult>();
    }
}
