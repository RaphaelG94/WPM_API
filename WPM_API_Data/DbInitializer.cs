using WPM_API.Common.Utils;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using static WPM_API.Common.Constants;

namespace  WPM_API.Data
{
    public static class DbInitializer
    {
        public static async void InitializeAsync(IApplicationBuilder app)
        {
            // Get an instance of the DbContext from the DI container
            using (var context = app.ApplicationServices.GetRequiredService<DBData>())
            {
                // Migrate DB
                context.Database.Migrate();
                //if (!context.Database.EnsureCreated())
                //{
                //    throw new Exception("Database Error: No Database Created");
                //}
                // Add Roles
                Role adminRole = null;
                try
                {
                    adminRole = await context.Roles.FirstAsync(x => x.Name == Roles.Admin);
                }
                catch (Exception) { }
                Role customerRole = null;
                try
                {
                    customerRole = await context.Roles.FirstAsync(x => x.Name == Roles.Customer);
                }
                catch (Exception) { }
                Role systemhouseRole = null;
                try
                {
                    systemhouseRole = await context.Roles.FirstAsync(x => x.Name == Roles.Systemhouse);
                }
                catch (Exception) { }
                if (adminRole == null)
                {
                    adminRole = new Role()
                    {
                        Name = Roles.Admin
                    };
                    context.Roles.Add(adminRole);
                }
                if (customerRole == null)
                {
                    customerRole = new Role()
                    {
                        Name = Roles.Customer
                    };
                    context.Roles.Add(customerRole);
                }
                if (systemhouseRole == null)
                {
                    systemhouseRole = new Role()
                    {
                        Name = Roles.Systemhouse
                    };
                    context.Roles.Add(systemhouseRole);
                }
                context.SaveChanges();

                // Look for Admin-Account.
                User adminAccount = null;
                try
                {
                    adminAccount = await context.Users.SingleAsync(x => x.Email == "ams@bitstream.de" && x.DeletedDate == null);
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
                        Password = PasswordHash.HashPassword("BitStream2000!")
                    };
                    context.Users.Add(adminAccount);
                }
                context.SaveChanges();
                // Admin Account has Roles
                adminAccount = await context.Users.SingleAsync(x => x.Email == "ams@bitstream.de" && x.DeletedDate == null);
                try
                {
                    await context.UserRoles.FirstAsync(x => x.RoleId == adminRole.Id && x.UserId == adminAccount.Id);
                }
                catch (Exception)
                {
                    UserRole adminUserRole = new UserRole() { Role = adminRole, User = adminAccount };
                    adminAccount.UserRoles.Add(adminUserRole);
                }
                try
                {
                    await context.UserRoles.FirstAsync(x => x.RoleId == systemhouseRole.Id && x.UserId == adminAccount.Id);
                }
                catch (Exception)
                {
                    UserRole systemhouseUserRole = new UserRole() { Role = systemhouseRole, User = adminAccount };
                    adminAccount.UserRoles.Add(systemhouseUserRole);
                }

                context.SaveChanges();
            }
        }
    }
}
