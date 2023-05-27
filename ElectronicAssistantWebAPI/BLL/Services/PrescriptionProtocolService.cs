using AutoMapper;
using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.BLL.ViewModels;
using ElectronicAssistantWebAPI.DAL.Models;
using ElectronicAssistantWebAPI.DAL.Repository;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public class PrescriptionProtocolService : IPrescriptionProtocolService
    {
        private readonly IRepository<PrescriptionProtocol> _prescriptionProtocolRepository;
        private readonly IRepository<RecommendedPrescription> _recommendedPrescriptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PrescriptionProtocolService> _logger;

        public PrescriptionProtocolService(IRepository<PrescriptionProtocol> prescriptionProtocolRepository,
                                           IRepository<RecommendedPrescription> recommendedPrescriptionRepository,
                                           IMapper mapper,
                                           ILogger<PrescriptionProtocolService> logger)
        {
            _prescriptionProtocolRepository = prescriptionProtocolRepository;
            _recommendedPrescriptionRepository = recommendedPrescriptionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<PrescriptionProtocol> Get()
        {
            return ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).GetPrescriptionProtocols();
        }

        public async Task<PrescriptionProtocol> GetByIdAsync(string id)
        {
            return await ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).GetPrescriptionProtocolByIdAsync(id);
        }

        public async Task<PrescriptionProtocol> AddAsync(AddPrescriptionProtocol model)
        {
            return await ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).AddPrescriptionProtocolAsync(model);
        }

        public async Task<PrescriptionProtocol> UpdateAsync(UpdatePrescriptionProtocol model)
        {
            return await ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).UpdatePrescriptionProtocolAsync(model);
        }

        public async Task DeleteAsync(DelPrescriptionProtocol model)
        {
            await ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).DeletePrescriptionProtocolAsync(model.Id);
        }

        public async Task<FileUploadResultModel> PostFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                    new FileUploadResultModel { NotError = false, Message = "File Not Selected" };

                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    new FileUploadResultModel { NotError = false, Message = "Invalid file format" };

                var path = Path.Combine(Directory.GetCurrentDirectory(), "FileDownloaded", file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);

                    ISheet sheet;
                    fileStream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(fileStream); //This will read the Excel 97-2000 formats   
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(fileStream); //This will read 2007 Excel format   
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook    
                    }

                    var idFileUpload = Guid.NewGuid().ToString();
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells
                        {
                            string[] strings = sheet.GetRow(row).GetCell(7).StringCellValue.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            var prescriptions = string.Join("/", strings);

                            var addPrescriptionProtocol = new AddPrescriptionProtocol();

                            addPrescriptionProtocol.IdFileUpload = idFileUpload;
                            addPrescriptionProtocol.LineNumberExcel = (row + 1);
                            addPrescriptionProtocol.PatientGender = sheet.GetRow(row).GetCell(0).StringCellValue;
                            addPrescriptionProtocol.PatientsDateOfBirth = sheet.GetRow(row).GetCell(1).StringCellValue;
                            addPrescriptionProtocol.PatientID = sheet.GetRow(row).GetCell(2).NumericCellValue.ToString();
                            addPrescriptionProtocol.MKB10 = sheet.GetRow(row).GetCell(3).StringCellValue;
                            addPrescriptionProtocol.Diagnosis = sheet.GetRow(row).GetCell(4).StringCellValue;
                            addPrescriptionProtocol.DateOfService = sheet.GetRow(row).GetCell(5).StringCellValue.ToString();
                            addPrescriptionProtocol.Position = sheet.GetRow(row).GetCell(6).StringCellValue;
                            addPrescriptionProtocol.Prescription = prescriptions;

                            await AddAsync(addPrescriptionProtocol);
                        }
                    }

                    return new FileUploadResultModel { NotError = true, Message = idFileUpload };
                }
            }
            catch (Exception)
            {
                return new FileUploadResultModel { NotError = false, Message = "File read error" };
            }
        }

        public IEnumerable<string> GetPositions()
        {
            var prescriptionProtocols = ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).GetPrescriptionProtocols();
            return prescriptionProtocols.Select(o => o.Position).Distinct();
        }

        public ProtocolAnalysisResultViewModel GetProtocolAnalysis(string IdFileUpload, string position = "")
        {
            var protocolAnalysisResultViewModel = new ProtocolAnalysisResultViewModel();

            var prescriptionProtocols = ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).Get().Where(o => o.IdFileUpload == IdFileUpload);
            if(!string.IsNullOrEmpty(position))
            {
                prescriptionProtocols = prescriptionProtocols.Where(o => o.Position == position);
            }

            foreach (var pp in prescriptionProtocols)
            {
                var typeResult = ProtocolAnalysis(pp.Diagnosis, pp.Prescription);
                if (typeResult != 0)
                {
                    var protocolAnalysisResult = new ProtocolAnalysisResult()
                    {
                        LineNumberExcel = pp.LineNumberExcel,
                        PatientGender = pp.PatientGender,
                        PatientsDateOfBirth = pp.PatientsDateOfBirth,
                        PatientID = pp.PatientID,
                        MKB10 = pp.MKB10,
                        Diagnosis = pp.Diagnosis,
                        DateOfService = pp.DateOfService,
                        Position = pp.Position,
                        Prescription = pp.Prescription.Replace("/", "; "),

                        TypeResult = typeResult
                    };
                    protocolAnalysisResultViewModel.ProtocolAnalysisResults.Add(protocolAnalysisResult);

                    if (typeResult == 1)
                        ++protocolAnalysisResultViewModel.Type1;
                    else if (typeResult == 2)
                        ++protocolAnalysisResultViewModel.Type2;
                    else
                        ++protocolAnalysisResultViewModel.Type3;
                }
            }

            return protocolAnalysisResultViewModel;
        }

        private int ProtocolAnalysis(string diagnosis, string prescription)
        {
            string[] strings = prescription.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var recommendedPrescriptions = ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRecommendedPrescriptions()
                                                                                                                  .Where(o => o.Diagnosis.Trim().ToLower() == diagnosis.Trim().ToLower())
                                                                                                                  .ToList();
            if(!recommendedPrescriptions.Any())
            {
                return 0;
            }
            
            var fullCompliance = true;
            var additionalAppointments = false;
            foreach (var rp in recommendedPrescriptions)
            {
                if(strings.FirstOrDefault(o => o.Trim().ToLower() == rp.Prescription.Trim().ToLower()) == null)
                {
                    fullCompliance = false;
                    break;
                }
            }

            if (fullCompliance) 
            {
                if(strings.Count() != recommendedPrescriptions.Count())
                {
                    fullCompliance = false;
                    if(strings.Count() > recommendedPrescriptions.Count())
                    {
                        additionalAppointments = true;
                    }
                }
            }

            if (fullCompliance)
                return 1;
            else if (additionalAppointments)
                return 2;
            else
                return 3;
        }

        public IEnumerable<string> GetIdFilesUpload()
        {
            var model = ((PrescriptionProtocolRepository)_prescriptionProtocolRepository).GetPrescriptionProtocols()
                                                                                         .Select(o => o.IdFileUpload)
                                                                                         .Distinct()
                                                                                         .ToList();

            return model;
        }
    }
}
