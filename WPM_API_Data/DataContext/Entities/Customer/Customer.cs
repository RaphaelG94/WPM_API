using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class Customer : IEntity, IDeletable
    {
        [Key, Column("PK_Customer")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string SystemhouseId { get; set; }

        /// <summary>
        /// Das Systemhouse, das den Kunden administriert
        /// </summary>
        [ForeignKey("SystemhouseId")]
        public virtual Systemhouse Systemhouse { get; set; }

        /// <summary>
        /// Die Subscription die dem Kunden zugeordnet sind
        /// </summary>
        public List<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Kundenname
        /// </summary>
        public string Name { get; set; }
        public Boolean Deletable { get; set; }

        /// <summary>
        /// Alle User die auf diesen Kunden zugreifen dürfen
        /// </summary>
        public ICollection<UserCustomer> UserCustomer { get; set; }

        [InverseProperty("Customer")]
        public virtual List<Default> Defaults { get; set; }

        [InverseProperty("Customer")]
        public List<Company> Companies { get; set; }

        public string MainCompanyId { get; set; }

        [ForeignKey("MainCompanyId")]
        public Company MainCompany { get; set; }

        [InverseProperty("Customer")]
        public List<Location> Locations { get; set; }

        [InverseProperty("Customer")]
        public List<Person> Persons { get; set; }

        public List<Parameter> Parameters { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("IconRightId")]
        public File IconRight { get; set; }

        public string IconRightId { get; set; }

        [ForeignKey("IconLeftId")]
        public File IconLeft { get; set; }

        public string IconLeftId { get; set; }

        [ForeignKey("BannerId")]
        public File Banner { get; set; }

        public string BannerId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OpeningTimes { get; set; }
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
        public string LtSASWríte { get; set; }
        public List<CloudEntryPoint> CloudEntryPoints { get; set; }
        public List<StorageEntryPoint> StorageEntryPoints { get; set; }
        public DateTime? LtSASExpireDate { get; set; }
        public List<CustomerSoftwareStream> CustomerSoftwareStreams { get; set; }
        public List<CustomerImageStream> CustomerImageStreams { get; set; }
        public string WinPEDownloadLink { get; set; }
        public string BannerLink { get; set; }
        public string AutoRegisterPassword { get; set; }
        public bool AutoRegisterClients { get; set; }
        public string OfficeConfig { get; set; }
        public bool UseCustomConfig { get; set; }

        public Company Where()
        {
            throw new NotImplementedException();
        }


    }
}