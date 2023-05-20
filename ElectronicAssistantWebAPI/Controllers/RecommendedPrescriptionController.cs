using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicAssistantWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendedPrescriptionController : ControllerBase
    {
        private readonly IRecommendedPrescriptionService _recommendedPrescriptionService;
        private readonly IMapper _mapper;

        public RecommendedPrescriptionController(IRecommendedPrescriptionService recommendedPrescriptionService, IMapper mapper)
        {
            _recommendedPrescriptionService = recommendedPrescriptionService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetRecommendedPrescriptions")]
        public IActionResult Get()
        {
            var rooms = _recommendedPrescriptionService.Get();
            return new OkObjectResult(rooms);
        }
    }
}
