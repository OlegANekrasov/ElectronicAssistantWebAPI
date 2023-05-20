using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.Models;
using ElectronicAssistantWebAPI.DAL.Repository;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace ElectronicAssistantWebAPI.BLL.Services
{
    public class RecommendedPrescriptionService : IRecommendedPrescriptionService
    {
        private readonly IRepository<RecommendedPrescription> _recommendedPrescriptionRepository;
        private readonly ILogger<RecommendedPrescriptionService> _logger;

        public RecommendedPrescriptionService(IRepository<RecommendedPrescription> recommendedPrescriptionRepository, 
                                              ILogger<RecommendedPrescriptionService> logger)
        {
            _recommendedPrescriptionRepository = recommendedPrescriptionRepository;
            _logger = logger;
        }

        public IEnumerable<RecommendedPrescription> Get()
        {
            return ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRecommendedPrescriptions();
        }

        public async Task<RecommendedPrescription> GetByIdAsync(string id)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRecommendedPrescriptionByIdAsync(id);
        }

        public async Task<RecommendedPrescription> AddAsync(AddRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).AddRecommendedPrescriptionAsync(model);
        }

        public async Task<RecommendedPrescription> UpdateAsync(UpdateRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).UpdateRecommendedPrescriptionAsync(model);
        }

        public async Task DeleteAsync(DelRecommendedPrescription model)
        {
            await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).DeleteRecommendedPrescriptionAsync(model.Id);
        }

        public async Task<FileUploadResultModel> PostFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                    return new FileUploadResultModel { NotError = false, Message = "File Not Selected" };

                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    return new FileUploadResultModel { NotError = false, Message = "Invalid file format" };

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

                    var previousDiagnosis = "";
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells
                        {
                            var addRecommendedPrescription = new AddRecommendedPrescription();

                            var diagnosis = sheet.GetRow(row).GetCell(0).StringCellValue;
                            if(string.IsNullOrEmpty(diagnosis))
                            {
                                addRecommendedPrescription.Diagnosis = previousDiagnosis;
                            }
                            else
                            {
                                addRecommendedPrescription.Diagnosis = diagnosis;
                                previousDiagnosis = diagnosis;
                            }
                            
                            addRecommendedPrescription.Prescription = sheet.GetRow(row).GetCell(1).StringCellValue;

                            await AddAsync(addRecommendedPrescription);
                        }
                    }

                    return new FileUploadResultModel { NotError = true, Message = "File read successfully" };
                }
            }
            catch (Exception)
            {
                return new FileUploadResultModel { NotError = false, Message = "File read error" };
            }
        }
    }
}
