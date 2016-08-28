using System.IO;
using AutoMapper;

namespace MeMeSquad
{
    using System.Text;
    using MeMeSquad.Config;
    using MeMeSquad.Services;
    using MeMeSquad.Services.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        #region Fields
        private MapperConfiguration mapperConfiguration { get; set; }

        public IConfigurationRoot Configuration { get; }
        #endregion

        #region Public Methods

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DocumentDbConfig>(Configuration.GetSection("DocumentDb"));

            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();

            this.RegisterAutoMapper(services);
            this.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.ConfigureJwtAuthentication(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
        #endregion

        #region Private Methods

        private void RegisterAutoMapper(IServiceCollection services)
        {
            this.mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfig());
            });

            services.AddSingleton<IMapper>(sp => this.mapperConfiguration.CreateMapper());
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IPostService, PostService>();
        }

        private void ConfigureJwtAuthentication(IApplicationBuilder app)
        {
            var secretKey = Configuration.GetValue<string>("Authentication:SecretKey");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey))
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }
        #endregion
    }
}
