namespace WPM_API.Models
{
    public class PersonViewModel
    {
        public PersonViewModel() { }

        public string Id { get; set; }

        public string Title { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string AcademicDegree { get; set; }

        public string EmployeeType { get; set; }

        public string CostCenter { get; set; }

        public string PhoneNr { get; set; }

        public string FaxNr { get; set; }

        public string MobileNr { get; set; }

        public string EmailPrimary { get; set; }

        public string State { get; set; }

        public string EmailOptional { get; set; }
        public string Domain { get; set; }

        public string DepartementName { get; set; }

        public string DepartementShort { get; set; }

        public string RoomNr { get; set; }

        public string EmployeeNr { get; set; }

        public string CustomerId { get; set; }

        public string CompanyId { get; set; }

        public UpdateMainCompanyViewModel Company { get; set; }
    }

    public class PersonNameView
    {
        public PersonNameView() { }

        public string Id { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }
    }
}
