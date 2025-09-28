
using DomainLayer.Contracts;
using E_Store.Web.CustomExceptionMiddelWares;
using E_Store.Web.Extensions;
using E_Store.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data.Configurations;
using Persistence.Data.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerService();
            builder.Services.AddInfrastructureService(builder.Configuration);
            builder.Services.AddTransient<PictureUrlResolver>();
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationService();
            builder.Services.AddJwtService(builder.Configuration);
            // builder.Services.AddAutoMapper(typeof(ServiceAssemblyReferences).Assembly);
            //Swagger Documentation Configuration :
            

            #endregion

            var app = builder.Build();

            #region DataSeeding
            using var Scope = app.Services.CreateScope();
            var ObjectofDataSeeding = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjectofDataSeeding.DataSeedAsync();
            await ObjectofDataSeeding.IdentityDataSeedAsync();
            #endregion

            #region Configure the HTTP request pipeline.
            app.UseMiddleware<CustomExceptionHandlerMiddelWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(Options =>
                {
                    Options.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration = true,
                    };
                    Options.DocumentTitle = "My E-Store API Documentation";
                    Options.JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    Options.DocExpansion(DocExpansion.None);
                    Options.EnableFilter();
                    Options.EnablePersistAuthorization();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
