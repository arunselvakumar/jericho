namespace MeMeSquad
{
    using System.IO;
    using System.Text;

    using Swashbuckle.Swagger;

    using AutoMapper;

    using MeMeSquad.Config;
    using MeMeSquad.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        #region Fields

        public IConfigurationRoot Configuration { get; }

        private MapperConfiguration MapperConfiguration { get; set; }

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
            this.Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DocumentDbConfig>(this.Configuration.GetSection("DocumentDb"));

            services.AddApplicationInsightsTelemetry(this.Configuration);
            services.AddMvc();
            services.AddSwaggerGen();

            services.AddAutoMapper(this.MapperConfiguration);
            services.AddCreateUserValidationService();
            services.AddUserService();
            services.AddPostService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.ConfigureJwtAuthentication(app);

            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
        #endregion

        #region Private Methods

        private void ConfigureJwtAuthentication(IApplicationBuilder app)
        {
            var secretKey = this.Configuration.GetValue<string>("Authentication:SecretKey");
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
