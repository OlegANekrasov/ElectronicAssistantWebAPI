using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Services;
using ElectronicAssistantWebAPI.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace ElectronicAssistantWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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
            var prescriptionProtocols = _prescriptionProtocolService.Get();
            return new OkObjectResult(prescriptionProtocols);
        }

        [HttpPost]
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
        
        [HttpGet(Name = "GetPositions")]
        public IActionResult GetPositions()
        {
            var positions = _prescriptionProtocolService.GetPositions();
            return new OkObjectResult(positions);
        }

        [HttpGet(Name = "GetIdFilesUpload")]
        public IActionResult GetIdFilesUpload()
        {
            var idFilesUpload = _prescriptionProtocolService.GetIdFilesUpload();
            return new OkObjectResult(idFilesUpload);
        }

        [HttpGet(Name = "GetProtocolAnalysis")]
        public IActionResult GetProtocolAnalysis(string idFileUpload, string? position = "")
        {
            var result = _prescriptionProtocolService.GetProtocolAnalysis(idFileUpload, position);
            return new OkObjectResult(result);
        }


    }
}
