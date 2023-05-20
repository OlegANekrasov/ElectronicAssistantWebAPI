using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Services;
using ElectronicAssistantWebAPI.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicAssistantWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionProtocolController : ControllerBase
    {
        private readonly IPrescriptionProtocolService _prescriptionProtocolService;
        private readonly IMapper _mapper;

        public PrescriptionProtocolController(IPrescriptionProtocolService prescriptionProtocolService, IMapper mapper)
        {
            _prescriptionProtocolService = prescriptionProtocolService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetPrescriptionProtocols")]
        public IActionResult Get()
        {
            var rooms = _prescriptionProtocolService.Get();
            return new OkObjectResult(rooms);
        }

        [HttpPost("PostSingleFile")]
        public async Task<ActionResult> PostSingleFile([FromForm] FileUploadViewModel file)
        {
            if (file.FileUpload == null)
            {
                return BadRequest();
            }

            try
            {
                var result = await _prescriptionProtocolService.PostFileAsync(file.FileUpload);
                if (result.NotError)
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
