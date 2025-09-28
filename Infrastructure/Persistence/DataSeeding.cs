using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Configurations;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext ,
        UserManager<ApplicationUser> _userManager ,
        RoleManager<IdentityRole> _roleManager ,
        StoreIdentityDbContext _identityDbContext) : IDataSeeding

    {
        public async Task DataSeedAsync()
        {
            try
            {
                //1-Check on the Apply Migrations:
                if (( await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    _dbContext.Database.Migrate();
                }
                if (!_dbContext.ProductBrands.Any())
                {
                    //1-ReadData:
                    // var productBrandRead = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var productBrandRead = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    //2-ConvertData from string To C# Objects:
                    var productbrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandRead);
                    //3-Add Range :
                    if (productbrands is not null && productbrands.Any())
                       await _dbContext.ProductBrands.AddRangeAsync(productbrands);
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    //1-ReadData:
                    var productTypeRead = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    //2-ConvertData from string To C# Objects:
                    var producttype =await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypeRead);
                    //3-Add Range :
                    if (producttype is not null && producttype.Any())
                       await _dbContext.ProductTypes.AddRangeAsync(producttype);
                }

                if (!_dbContext.Products.Any())
                {
                    //1-ReadData:
                    var productRead = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    //2-ConvertData from string To C# Objects:
                    var product = await JsonSerializer.DeserializeAsync<List<Product>>(productRead);
                    //3-Add Range :
                    if (product is not null && product.Any())
                      await _dbContext.Products.AddRangeAsync(product);
                }

                if(!_dbContext.Set<DeliveryMethod>().Any())
                {
                    //1-Read Data:
                    var DeliverRead = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
                    //2-convertData from string to C# object :
                    var Delivery = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliverRead);
                    //3-Add Range :
                    if(Delivery is not null && Delivery.Any())
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(Delivery);

                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // Console.WriteLine($"An error occurred during data seeding: {ex.Message}");
            }
        }

        public async Task IdentityDataSeedAsync()
        {
           try
            {
                if(!_roleManager.Roles.Any())
                {
                   await _roleManager.CreateAsync(new IdentityRole("Admin"));
                   await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if(!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser
                    {
                        Email = "mohamed@gmail.com",
                        DisplayName = "Mohamed Said",
                        UserName = "MohamedSaid",
                        PhoneNumber= "01113245761"
                    };
                    var user02 = new ApplicationUser
                    {
                        Email = "salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        UserName = "SalmaMohamed",
                        PhoneNumber= "01298764571"
                    };
                   await _userManager.CreateAsync(user01, "Pa$$w0rd");
                   await _userManager.CreateAsync(user02, "Pa$$w0rd");
                   await _userManager.AddToRoleAsync(user01, "Admin");
                   await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
                await _identityDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // Console.WriteLine($"An error occurred during identity data seeding: {ex.Message}");
            }
        }
    }
}
