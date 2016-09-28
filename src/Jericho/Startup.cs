namespace Jericho
{
    using System.IO;
    using System.Text;

    using AutoMapper;

    using Jericho.Config;
    using Jericho.Extensions;
    using Jericho.Options;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private MapperConfiguration MapperConfiguration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            if (env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbOptions>(this.Configuration.GetSection("MongoDb"));

            services.AddApplicationInsightsTelemetry(this.Configuration);
            services.AddMvc();
            services.AddSwaggerGen();

            services.AddMongoDbInstance();
            services.AddAutoMapper(this.MapperConfiguration);
            services.AddIdentityService(this.Configuration);
            services.AddMongoIdentityService();
            services.AddCreateUserValidationService();
            services.AddUserService();
            services.AddPostService();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.ConfigureJwtAuthentication(app);

            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentity();

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUi();
        }

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
    }
}
