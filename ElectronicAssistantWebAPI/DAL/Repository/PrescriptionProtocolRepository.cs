using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.DAL.EF;
using ElectronicAssistantWebAPI.DAL.Models;

namespace ElectronicAssistantWebAPI.DAL.Repository
{
    public class PrescriptionProtocolRepository : Repository<PrescriptionProtocol>
    {
        public PrescriptionProtocolRepository(ApplicationDbContext db) : base(db)
        {

        }

        public IEnumerable<PrescriptionProtocol> GetPrescriptionProtocols()
        {
            return base.Get();
        }

        public async Task<PrescriptionProtocol> GetPrescriptionProtocolByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<PrescriptionProtocol> AddPrescriptionProtocolAsync(AddPrescriptionProtocol model)
        {
            var prescriptionProtocol = new PrescriptionProtocol()
            {
                Id = Guid.NewGuid().ToString(),
                IdFileUpload = model.IdFileUpload,
                LineNumberExcel = model.LineNumberExcel,
                PatientGender = model.PatientGender,
                PatientsDateOfBirth = model.PatientsDateOfBirth,
                PatientID = model.PatientID,
                MKB10 = model.MKB10,
                DateOfService = model.DateOfService,
                Position = model.Position,
                Diagnosis = model.Diagnosis,
                Prescription = model.Prescription
            };

            await CreateAsync(prescriptionProtocol);
            return prescriptionProtocol;
        }

        public async Task<PrescriptionProtocol> UpdatePrescriptionProtocolAsync(UpdatePrescriptionProtocol model)
        {
            var prescriptionProtocol = await GetByIdAsync(model.Id);
            if (prescriptionProtocol != null)
            {
                prescriptionProtocol.Diagnosis = model.Diagnosis;
                prescriptionProtocol.Prescription = model.Prescription;
                prescriptionProtocol.PatientGender = model.PatientGender;
                prescriptionProtocol.PatientsDateOfBirth = model.PatientsDateOfBirth;
                prescriptionProtocol.PatientID = model.PatientID;
                prescriptionProtocol.MKB10 = model.MKB10;
                prescriptionProtocol.DateOfService = model.DateOfService;
                prescriptionProtocol.Position = model.Position;

                await UpdateAsync(prescriptionProtocol);
                return prescriptionProtocol;
            }

            return null;
        }

        public async Task DeletePrescriptionProtocolAsync(string id)
        {
            await DeleteAsync(id);
        }
    }
}
