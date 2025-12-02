using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts.Seeds;
using ECommerce.Domain.Identity.Models;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Seed
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly StoreIdentityDbContext storeIdentityDbContext;

        public DataSeeding(StoreDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,StoreIdentityDbContext storeIdentityDbContext)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.storeIdentityDbContext = storeIdentityDbContext;
        }
        public async Task DataSeedAsync()
        {
            var PendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
                context.Database.Migrate();

            if (!context.ProductBrands.Any())
            {
                var ProductBrandData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\brands.json");
                var ProductBrands = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrandData);

                if (ProductBrands is not null && ProductBrands.Any())
                {
                    context.ProductBrands.AddRange(ProductBrands);
                }
            }
            if (!context.ProductTypes.Any())
            {
                var ProductTypeData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\types.json");
                var ProductTypes = JsonSerializer.Deserialize<List<ProductType>>(ProductTypeData);

                if (ProductTypes is not null && ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(ProductTypes);
                }
            }
            if (!context.Products.Any())
            {
                var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerce.Persistence\Data\products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products is not null && Products.Any())
                {
                    context.Products.AddRange(Products);
                }
            }
            context.SaveChanges();
        }

        public async Task IdentitySeedAsync()
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!userManager.Users.Any())
                {
                    var User1 = new ApplicationUser()
                    {
                        Email = "adham@gmail.com",
                        DisplayName = "Adham Samaan",
                        PhoneNumber = "01018063625",
                        UserName = "AdhamSamaan",
                    };
                    var User2 = new ApplicationUser()
                    {
                        Email = "ahmed@gmail.com",
                        DisplayName = "Ahmed Samaan",
                        PhoneNumber = "01002003000",
                        UserName = "AhmedSamaan",
                    };
                    await userManager.CreateAsync(User1, "P@ssw0rd");
                    await userManager.CreateAsync(User2, "P@ssw0rd");
                    await userManager.AddToRoleAsync(User1, "Admin");
                    await userManager.AddToRoleAsync(User2, "SuperAdmin");

                }
                await storeIdentityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
