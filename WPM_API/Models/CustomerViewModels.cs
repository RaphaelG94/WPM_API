namespace WPM_API.Models
{

    public class CustomerViewModel
    {
        public CustomerViewModel() { }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OpeningTimes { get; set; }
        public SystemhouseRefViewModel Systemhouse { get; set; }
        public List<SubscriptionViewModel> Subscriptions { get; set; }
        public List<ParameterViewModel> Parameters { get; set; }
        public FileRefModel IconLeft { get; set; }
        public FileRefModel IconRight { get; set; }
        public FileRefModel Banner { get; set; }
        public string CmdBtn1 { get; set; }
        public string CmdBtn2 { get; set; }
        public string CmdBtn3 { get; set; }
        public string CmdBtn4 { get; set; }
        public string Btn1Label { get; set; }
        public string Btn2Label { get; set; }
        public string Btn3Label { get; set; }
        public string Btn4Label { get; set; }
        public string CsdpRoot { get; set; }
        public string CsdpContainer { get; set; }
        public string LtSASRead { get; set; }
        public string LtSASWrite { get; set; }
        public string WinPEDownloadLink { get; set; }
        public string BannerLink { get; set; }
    }

    public class CustomerEditViewModel
    {
        public CustomerEditViewModel() { }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OpeningTimes { get; set; }
        public string SystemhouseId { get; set; }
        public FileRefModel IconLeft { get; set; }
        public FileRefModel IconRight { get; set; }

        public FileRefModel Banner { get; set; }
        public string CmdBtn1 { get; set; }
        public string CmdBtn2 { get; set; }
        public string CmdBtn3 { get; set; }
        public string CmdBtn4 { get; set; }
        public string Btn1Label { get; set; }
        public string Btn2Label { get; set; }
        public string Btn3Label { get; set; }
        public string Btn4Label { get; set; }
        public string CsdpRoot { get; set; }
        public string CsdpContainer { get; set; }
        public string LtSASRead { get; set; }
        public string LtSASWrite { get; set; }
        public string WinPEDownloadLink { get; set; }
        public string BannerLink { get; set; }
    }

    public class CustomerRefViewModel
    {
        public CustomerRefViewModel() { }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OpeningTimes { get; set; }
        public FileRefModel? IconRight { get; set; }
        public FileRefModel? IconLeft { get; set; }
        public FileRefModel? Banner { get; set; }
        public string? CmdBtn1 { get; set; }
        public string? CmdBtn2 { get; set; }
        public string? CmdBtn3 { get; set; }
        public string? CmdBtn4 { get; set; }
        public string? Btn1Label { get; set; }
        public string? Btn2Label { get; set; }
        public string? Btn3Label { get; set; }
        public string? Btn4Label { get; set; }
        public string? CsdpRoot { get; set; }
        public string? CsdpContainer { get; set; }
        public string? LtSASRead { get; set; }
        public string? LtSASWrite { get; set; }
        public string? WinPEDownloadLink { get; set; }
        public string? BannerLink { get; set; }
    }

    public class CreateCustomerViewModel
    {
        public CreateCustomerViewModel() { }

        public string Name { get; set; }

        public AddCompanyViewModel CompanyHQData { get; set; }

        public PersonViewModel Expert { get; set; }
    }

    public class CustomerOverviewViewModel
    {
        public CustomerRefViewModel Customer { get; set; }

        public AddCompanyViewModel Company { get; set; }

        public PersonViewModel Expert { get; set; }

        public AddLocationViewModel Headquarter { get; set; }
    }

    public class CreateCustomerResultViewModel
    {
        public CustomerViewModel Customer { get; set; }

        public CompanyViewModel Company { get; set; }
    }

    public class CustomerNameViewModel
    {
        public string Name { get; set; }
        public bool Exists { get; set; }
        public string? CsdpRoot { get; set; }
        public string? CsdpContainer { get; set; }
        public string? LtSASRead { get; set; }
        public string? LtSASWrite { get; set; }
        public string? WinPEDownloadLink { get; set; }
    }
}
