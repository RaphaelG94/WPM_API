using Microsoft.EntityFrameworkCore;
using WPM_API.Common.Utils;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using static WPM_API.Common.Constants;

namespace WPM_API.Data
{
    public static class DbInitializer
    {
        public static async System.Threading.Tasks.Task InitializeAsync(DBData dbContext)
        {
            // Get an instance of the DbContext from the DI container
            //using (var dbContext = app.ApplicationServices.GetRequiredService<DBData>())
            //{
            // Migrate DB
            dbContext.Database.Migrate();
            //if (!dbContext.Database.EnsureCreated())
            //{
            //    throw new Exception("Database Error: No Database Created");
            //}
            // Add Roles
            Role adminRole = null;
            try
            {
                adminRole = await dbContext.Roles.FirstAsync(x => x.Name == Roles.Admin);
            }
            catch (Exception) { }
            Role customerRole = null;
            try
            {
                customerRole = await dbContext.Roles.FirstAsync(x => x.Name == Roles.Customer);
            }
            catch (Exception) { }
            Role systemhouseRole = null;
            try
            {
                systemhouseRole = await dbContext.Roles.FirstAsync(x => x.Name == Roles.Systemhouse);
            }
            catch (Exception) { }
            if (adminRole == null)
            {
                adminRole = new Role()
                {
                    Name = Roles.Admin
                };
                dbContext.Roles.Add(adminRole);
            }
            if (customerRole == null)
            {
                customerRole = new Role()
                {
                    Name = Roles.Customer
                };
                dbContext.Roles.Add(customerRole);
            }
            if (systemhouseRole == null)
            {
                systemhouseRole = new Role()
                {
                    Name = Roles.Systemhouse
                };
                dbContext.Roles.Add(systemhouseRole);
            }
            dbContext.SaveChanges();

            // Look for Admin-Account.
            User adminAccount = null;
            try
            {
                adminAccount = await dbContext.Users.SingleAsync(x => x.Email == "ams@bitstream.de" && x.DeletedDate == null);
            }
            catch (Exception) { };

            if (adminAccount == null)
            {
                Systemhouse systemhouse = new Systemhouse
                {
                    Name = "Bitstream Systems",
                    Deletable = false
                };

                adminAccount = new User
                {
                    UserName = "Administrator",
                    Email = "ams@bitstream.de",
                    Login = "ams@bitstream.de",
                    Deletable = false,
                    Systemhouse = systemhouse,
                    Active = true,
                    Password = PasswordHash.HashPassword("BitStream2000!"),
                    CreatedDate = DateTime.Now
                };
                dbContext.Users.Add(adminAccount);
            }
            dbContext.SaveChanges();
            // Admin Account has Roles
            adminAccount = await dbContext.Users.SingleAsync(x => x.Email == "ams@bitstream.de" && x.DeletedDate == null);
            try
            {
                await dbContext.UserRoles.FirstAsync(x => x.RoleId == adminRole.Id && x.UserId == adminAccount.Id);
            }
            catch (Exception)
            {
                UserRole adminUserRole = new UserRole() { Role = adminRole, User = adminAccount };
                adminAccount.UserRoles.Add(adminUserRole);
            }
            try
            {
                await dbContext.UserRoles.FirstAsync(x => x.RoleId == systemhouseRole.Id && x.UserId == adminAccount.Id);
            }
            catch (Exception)
            {
                UserRole systemhouseUserRole = new UserRole() { Role = systemhouseRole, User = adminAccount };
                adminAccount.UserRoles.Add(systemhouseUserRole);
            }

            dbContext.SaveChanges();
            //}
        }
    }
}
