namespace ElectronicAssistantWebAPI.BLL.Models
{
    public class ProtocolAnalysisResult
    {
        public int LineNumberExcel { get; set; }
        public string PatientGender { get; set; }
        public string PatientsDateOfBirth { get; set; }
        public string PatientID { get; set; }
        public string MKB10 { get; set; }
        public string Diagnosis { get; set; }
        public string DateOfService { get; set; }
        public string Position { get; set; }
        public string Prescription { get; set; }

        public int TypeResult { get; set; }
    }
}
