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
            return ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRooms();
        }

        public async Task<RecommendedPrescription> GetByIdAsync(string id)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).GetRecommendedPrescriptionByIdAsync(id);
        }

        public async Task<RecommendedPrescription> AddAsync(AddRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).AddRoomAsync(model);
        }

        public async Task<RecommendedPrescription> UpdateAsync(UpdateRecommendedPrescription model)
        {
            return await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).UpdateRoomAsync(model);
        }

        public async Task DeleteAsync(DelRecommendedPrescription model)
        {
            await ((RecommendedPrescriptionRepository)_recommendedPrescriptionRepository).DeleteRoomAsync(model.Id);
        }

        public async Task<string> PostFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                    return "File Not Selected";

                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    return "Invalid file format";

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

                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells
                        {
                            var addRecommendedPrescription = new AddRecommendedPrescription();
                            addRecommendedPrescription.Diagnosis = sheet.GetRow(row).GetCell(0).StringCellValue;
                            addRecommendedPrescription.Prescription = sheet.GetRow(row).GetCell(1).StringCellValue;

                            await AddAsync(addRecommendedPrescription);
                        }
                    }

                    return "File read successfully";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
