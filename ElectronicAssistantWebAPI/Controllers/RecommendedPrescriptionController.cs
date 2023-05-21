using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Services;
using ElectronicAssistantWebAPI.BLL.ViewModels;
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
            var recommendedPrescriptions = _recommendedPrescriptionService.Get();
            return new OkObjectResult(recommendedPrescriptions);
        }

        [HttpPost(Name = "PostSingleFile")]
        public async Task<ActionResult> PostSingleFile([FromForm] FileUploadViewModel file)
        {
            if (file.FileUpload == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await _recommendedPrescriptionService.PostFileAsync(file.FileUpload);
                if(result.NotError)
                    return Ok(result.Message);
                else
                    return BadRequest(result.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
