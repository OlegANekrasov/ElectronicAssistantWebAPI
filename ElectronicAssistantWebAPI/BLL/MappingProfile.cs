using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProtocolAnalysisResult, PrescriptionProtocol>().ForMember(x => x.IdFileUpload, opt => opt.Ignore()); ;
        }
    }
}
