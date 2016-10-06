namespace Jericho
{
    using System.IO;
    using System.Text;

    using AutoMapper;
    using Jericho.Extensions;
    using Jericho.Options;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using System;

    public class Startup
    {
        #region Properties

        public IConfigurationRoot Configuration { get; }

        private MapperConfiguration MapperConfiguration { get; set; }

        #endregion

        #region Constructor

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

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbOptions>(this.Configuration.GetSection("MongoDb"));
            services.Configure<SendGridOptions>(this.Configuration.GetSection("SendGrid"));
            services.Configure<Options.AuthenticationOptions>(this.Configuration.GetSection("Authentication"));

            services.AddApplicationInsightsTelemetry(this.Configuration);
            services.AddMvc();
            services.AddSwaggerGen();

            services.AddMongoDbInstance();
            services.AddAutoMapper(this.MapperConfiguration);

            services.AddIdentityService(this.Configuration);
            services.AddMongoIdentityService();
            services.AddHttpContextAccessorService();
            services.AddEmailService();
            services.AddUserService();
            services.AddPostService();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseIdentity();

            app.UseSwagger();

            app.UseSwaggerUi();

            this.ConfigureJwtAuthentication(app);

            app.UseMvc();
        }

        private void ConfigureJwtAuthentication(IApplicationBuilder app)
        {
            var secretKey = this.Configuration.GetValue<string>("Authentication:SecretKey");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
                AuthenticationType = JwtBearerDefaults.AuthenticationScheme
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}
