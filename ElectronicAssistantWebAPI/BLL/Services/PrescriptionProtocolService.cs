using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;
using ElectronicAssistantWebAPI.DAL.Repository;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public class PrescriptionProtocolService : IPrescriptionProtocolService
    {
        private readonly IRepository<PrescriptionProtocol> _prescriptionProtocolRepository;
        private readonly ILogger<PrescriptionProtocolService> _logger;

        public PrescriptionProtocolService(IRepository<PrescriptionProtocol> prescriptionProtocolRepository,
                                              ILogger<PrescriptionProtocolService> logger)
        {
            _prescriptionProtocolRepository = prescriptionProtocolRepository;
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
                            var addPrescriptionProtocol = new AddPrescriptionProtocol();

                            addPrescriptionProtocol.IdFileUpload = idFileUpload;
                            addPrescriptionProtocol.LineNumberExcel = row;
                            addPrescriptionProtocol.PatientGender = sheet.GetRow(row).GetCell(0).StringCellValue;
                            addPrescriptionProtocol.PatientsDateOfBirth = sheet.GetRow(row).GetCell(1).StringCellValue;
                            addPrescriptionProtocol.PatientID = sheet.GetRow(row).GetCell(2).StringCellValue;
                            addPrescriptionProtocol.MKB10 = sheet.GetRow(row).GetCell(3).StringCellValue;
                            addPrescriptionProtocol.Diagnosis = sheet.GetRow(row).GetCell(4).StringCellValue;
                            addPrescriptionProtocol.DateOfService = sheet.GetRow(row).GetCell(5).StringCellValue;
                            addPrescriptionProtocol.Position = sheet.GetRow(row).GetCell(6).StringCellValue;
                            addPrescriptionProtocol.Prescription = sheet.GetRow(row).GetCell(7).StringCellValue;

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
    }
}
