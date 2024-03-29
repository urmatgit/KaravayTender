// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Infrastructure.Constants.ClaimTypes;
using CleanArchitecture.Razor.Application.Constants.Permission;
using CleanArchitecture.Razor.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var administratorRole = new ApplicationRole("Admin") { Description = "Admin Group" };
            var ManagerRole = new ApplicationRole("Manager") { Description = "Manager" };
            var userRole = new ApplicationRole("Basic") { Description = "Basic Group" };
            var super = new ApplicationRole("Super") { Description = "Super Group" };
            var Supplier = new ApplicationRole("Supplier") { Description = "Поставщик сырья" }; //Supplier of raw materials
            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
                await roleManager.CreateAsync(ManagerRole);
                await roleManager.CreateAsync(userRole);
                await roleManager.CreateAsync(super);
                await roleManager.CreateAsync(Supplier);
                var Permissions = GetAllPermissions();
                foreach (var permission in Permissions)
                {
                    await roleManager.AddClaimAsync(administratorRole, new System.Security.Claims.Claim(ApplicationClaimTypes.Permission, permission));
                    await roleManager.AddClaimAsync(super, new System.Security.Claims.Claim(ApplicationClaimTypes.Permission, permission));
                }
            }

            var superUser = new ApplicationUser { UserName = "Super", IsActive = true, DisplayName = "Super user", Email = "neozhu@126.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"neozhu@126.com".ToMD5()}?s=120&d=retro" };
            var administrator = new ApplicationUser { UserName = "administrator", IsActive = true, DisplayName = "Administrator", Email = "new163@163.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"new163@163.com".ToMD5()}?s=120&d=retro" };
            var demo = new ApplicationUser { UserName = "Demo", IsActive = true, DisplayName = "Demo", Email = "neozhu@126.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"neozhu@126.com".ToMD5()}?s=120&d=retro" };
            var supplier = new ApplicationUser { UserName = "Supplier", IsActive = true, Email = "supplier@126.com", EmailConfirmed = true, ProfilePictureDataUrl = "" };
            var manager = new ApplicationUser { UserName = "Manager1", IsActive = true, Email = "Manager1@126.com", EmailConfirmed = true, ProfilePictureDataUrl = "",PhoneNumber="+7(999) 111-11-11" };
            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Password123!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                await userManager.CreateAsync(demo, "Password123!");
                await userManager.AddToRolesAsync(demo, new[] { userRole.Name });


                await userManager.CreateAsync(superUser, "Password123!");
                await userManager.AddToRolesAsync(superUser, new[] { super.Name });
                await userManager.CreateAsync(supplier, "Password123!");
                await userManager.AddToRolesAsync(supplier, new[] { Supplier.Name });
                await userManager.CreateAsync(manager, "Password123!");
                await userManager.AddToRolesAsync(manager, new[] { ManagerRole.Name });
            }

        }
        private static IEnumerable<string> GetAllPermissions()
        {
            var allPermissions = new List<string>();
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var moduleName = string.Empty;
                var moduleDescription = string.Empty;

                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (var fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(propertyValue.ToString());
                }
            }

            return allPermissions;
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            //Seed, if necessary
            //if (!context.DocumentTypes.Any())
            //    {
            //    context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Document", Description = "Document" });
            //    context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "PDF", Description = "PDF" });
            //    context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Image", Description = "Image" });
            //    context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Other", Description = "Other" });
            //    await context.SaveChangesAsync();
            //    }
            if (!context.KeyValues.Any())
            {
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "initialization", Text = "initialization", Description = "Status of workflow" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "processing", Text = "processing", Description = "Status of workflow" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "pending", Text = "pending", Description = "Status of workflow" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "finished", Text = "finished", Description = "Status of workflow" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNC", Text = "CNC", Description = "Region of Customer" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNN", Text = "CNN", Description = "Region of Customer" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNS", Text = "CNS", Description = "Region of Customer" });
                context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "Oversea", Text = "Oversea", Description = "Region of Customer" });
                await context.SaveChangesAsync();
                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Initial add key values", Level = "Information", UserName = "System", TimeStamp = System.DateTime.Now });
                await context.SaveChangesAsync();
            }
            if (!context.Customers.Any())
            {
                context.Customers.Add(new Domain.Entities.Customer() { Name = "SmartAdmin", AddressOfEnglish = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", GroupName = "SmartAdmin", Address = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", Sales = "GotBootstrap", RegionSalesDirector = "GotBootstrap", Region = "CNC", NameOfEnglish = "SmartAdmin", PartnerType = Domain.Enums.PartnerType.TP, Contact = "GotBootstrap", Email = "drlantern@gotbootstrap.com" });
                await context.SaveChangesAsync();

                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Initial add customer", Level = "Information", UserName = "System", TimeStamp = System.DateTime.Now });

                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Debug", Level = "Debug", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-1) });
                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Error", Level = "Error", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-1) });
                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Warning", Level = "Warning", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-2) });
                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Trace", Level = "Trace", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-4) });
                context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Fatal", Level = "Fatal", UserName = "System", TimeStamp = System.DateTime.Now.AddHours(-4) });
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeekDirectionAndCategory(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                Direction directionMuka = new Direction
                {
                    Name = "Мука"
                    ,
                    Created = DateTime.UtcNow
                };
                Direction directionMeterial = new Direction
                {
                    Name = "Сырье"
                    ,
                    Created = DateTime.UtcNow
                };
                Direction directionTMC = new Direction
                {
                    Name = "ТМЦ"
                    ,
                    Created = DateTime.UtcNow
                };
                Direction directionPackage = new Direction
                {
                    Name = "Упаковка"
                    ,
                    Created = DateTime.UtcNow
                };
                context.Directions.Add(directionMuka);
                context.Directions.Add(directionMeterial);
                context.Directions.Add(directionTMC);
                context.Directions.Add(directionPackage);
                await context.SaveChangesAsync();
                if (!context.Categories.Any())
                {
                    Category categoryIngredient = new Category
                    {
                        Created = DateTime.UtcNow,
                        Name = "Добавки",
                        DirectionId = directionMeterial.Id
                    };
                    Category categorySugar = new Category
                    {
                        Created = DateTime.UtcNow,
                        Name = "Сахар",
                        DirectionId = directionMeterial.Id
                    };
                    Category categoryYeast = new Category
                    {
                        Created = DateTime.UtcNow,
                        Name = "Дрожжи",
                        DirectionId = directionMeterial.Id
                    };
                    Category categoryMix = new Category
                    {
                        Created = DateTime.UtcNow,
                        Name = "Смеси",
                        DirectionId = directionMeterial.Id
                    };
                    context.Categories.Add(categoryIngredient);
                    context.Categories.Add(categorySugar);
                    context.Categories.Add(categoryYeast);
                    context.Categories.Add(categoryMix);
                    await context.SaveChangesAsync();

                }


            }
            if (!context.Areas.Any())
            {
                Area area = new Area()
                {
                    Name = "Каравай",
                    Address = "Херсонская 22"
                };
                context.Areas.Add(area);
                area = new Area()
                {
                    Name = "Заря",
                    Address = "Стачек 39"
                };
                context.Areas.Add(area);
                await context.SaveChangesAsync();

            }
            if (!context.UnitOfs.Any())
            {
                UnitOf unitOf = new UnitOf
                {
                    Name = "Кг"
                };
                context.UnitOfs.Add(unitOf);
                 unitOf = new UnitOf
                {
                    Name = "шт"
                };
                context.UnitOfs.Add(unitOf);
                await context.SaveChangesAsync();

            }
            if (!context.Vats.Any())
            {
                Vat vat = new Vat
                {
                    Name = "Налог1",
                    Stavka = 10
                };
                context.Vats.Add(vat);
                 vat = new Vat
                {
                    Name = "Налог2",
                    Stavka = 13
                };
                context.Vats.Add(vat);
                await context.SaveChangesAsync();
            }
            if (!context.QualityDocs.Any())
            {
                QualityDoc qualityDoc = new QualityDoc
                {
                    Name = "Документ1",

                };
                context.QualityDocs.Add(qualityDoc);
                qualityDoc = new QualityDoc
                {
                    Name = "Документ2",

                };
                context.QualityDocs.Add(qualityDoc);
                await context.SaveChangesAsync();

            }
        }
        public static async Task SeedSampleProductDataAsync(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                var randomNumber = new Random();
                List<Product> products = new List<Product>();
                for (int i = 0; i < 1000; i++)
                {
                    var product = new Product()
                    {
                        Name = $"Product  ({i + 1})",
                        Price = randomNumber.Next(1, 100),
                        Description = $"Auto generate {DateTime.UtcNow}"
                    };
                    products.Add(product);
                }
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
                List<Customer> customers = new List<Customer>();
                for (int i = 1; i < 1001; i++)
                {
                    int customerCount = randomNumber.Next(1, 500);

                    for (int j = 0; j < customerCount; j++)
                    {
                        var customer = new Customer
                        {
                            Name = $"Customer ({i} {j})",
                            NameOfEnglish = $"Customer ({i} {j})",
                            GroupName = $"Group {customerCount}",
                            PartnerType = Domain.Enums.PartnerType.TP,
                            Created = DateTime.UtcNow,
                            ProductId = i
                        };

                        customers.Add(customer);
                    }


                    Debug.WriteLine($"Product {i} customer count {customerCount}");

                }
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }

        }
        
    }
}
