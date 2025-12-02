
using System.Text;
using ECommerce.Abstraction.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Contracts.Seeds;
using ECommerce.Domain.Contracts.UOW;
using ECommerce.Domain.Identity.Models;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repos;
using ECommerce.Persistence.Seed;
using ECommerce.Persistence.UOW;
using ECommerce.Service.MappingProfiles;
using ECommerce.Service.Services;
using ECommerce.Shared.ErrorModels;
using ECommerce.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace ECommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            #region DataBases Connections
            builder.Services.AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            }); 
            #endregion

            builder.Services.AddScoped<IDataSeeding,DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IServiceManager,ServiceManager>();
            builder.Services.AddScoped<ICacheRepository,CacheRepository>();
            builder.Services.AddScoped<ICacheServices,CacheServices>();
            builder.Services.AddAutoMapper(M=>M.AddProfile(new ProjectProfile(builder.Configuration)));

            builder.Services.Configure<ApiBehaviorOptions>((options) =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var Errors = context.ModelState.Where(M => M.Value.Errors.Any())
                                    .Select(M => new ValidationError()
                                    {
                                        Field = M.Key,
                                        Errors = M.Value.Errors.Select(e => e.ErrorMessage)
                                    });
                    var Response = new ValidationErrorToReturn()
                    {
                        ValidationErrors = Errors
                    };
                    return new BadRequestObjectResult(Response);
                };
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options=>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetSection("JWTOptions")["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetSection("JWTOptions")["Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTOptions")["SecurityKey"]))
                };
            });

            var app = builder.Build();

            var Scope = app.Services.CreateScope();
            var ObjectSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectSeeding.DataSeedAsync();
            await ObjectSeeding.IdentitySeedAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
