using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class User : IEntity, IDeletable
    {
        public User()
        {
            UserRoles = new List<UserRole>();
            UpdatedUsers = new List<User>();
            DeletedUsers = new List<User>();
            UserForgotPasswords = new HashSet<UserForgotPassword>();
        }

        [Key, Column("PK_User")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required, StringLength(64)]
        public string Login { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(64)]
        public string UserName { get; set; }

        //[Required, StringLength(64)]
        //public string LastName { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public string FullName { get; set;
        //    //get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        //    //private set { }
        //}

        [Required, StringLength(64)]
        public string Email { get; set; }
        public bool Active { get; set; }

        public string? CustomerId { get; set; }
        [Required]
        public string SystemhouseId { get; set; }

        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean Deletable { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Das zugewiesene Systemhouse, wenn der User ein Login für ein Systemhouse ist
        /// </summary>
        [ForeignKey(nameof(SystemhouseId))]
        public virtual Systemhouse Systemhouse { get; set; }

        /// <summary>
        /// Der zugewiesene Kunde, wenn der User ein Login für einen Kunden ist
        /// </summary>
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        //[ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(UpdatedUsers))]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey(nameof(UpdatedByUserId))]
        public virtual List<User> UpdatedUsers { get; set; }

        //[ForeignKey(nameof(DeletedByUserId))]
        [InverseProperty(nameof(DeletedUsers))]
        public virtual User DeletedByUser { get; set; }

        [ForeignKey(nameof(DeletedByUserId))]
        public virtual List<User> DeletedUsers { get; set; }

        public virtual ICollection<UserForgotPassword> UserForgotPasswords { get; set; }

        public bool Admin
        {
            get
            {
                return UserRoles.Exists(x => x.Role.Name == "admin");
            }
        }


    }

    //internal class UserConfiguration : EntityMappingConfiguration<User>
    //{
    //    public override void Map(EntityTypeBuilder<User> b)
    //    {
    //        b.Property(p => p.FullName).HasComputedColumnSql("ltrim(rtrim(FirstName + ' ' + LastName))");
    //    }
    //}
}
