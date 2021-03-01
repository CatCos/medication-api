namespace MedicationApi
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using MedicationApi.Configuration.Interfaces;
    using MedicationApi.Configuration;
    using MedicationApi.Application.Bootstrap;
    using MedicationApi.Repositories.Bootstrap;

    public class Startup
    {
        private readonly IApplicationSettings settings;

        public Startup(IConfiguration configuration)
        {
            this.settings = configuration.Get<ApplicationSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddServices();

            services.AddValidation();

            services.ConfigureDatabase(this.settings.MongoSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medication API", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medication API v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
