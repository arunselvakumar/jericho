using Jericho.Helpers;
using Jericho.Helpers.Interfaces;
using Jericho.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jericho.Extensions
{
    using System;
    using AutoMapper;

    using Jericho.Config;
    using Jericho.Options;
    using Jericho.Services;
    using Jericho.Services.Interfaces;
    using Jericho.Validations;
    using Jericho.Validations.Interfaces;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static void AddUserService(this IServiceCollection service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserService, UserService>();
        }

        public static void AddPostService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IPostService, PostService>();
        }

        public static void AddCreateUserValidationService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<ICreateUserValidationService, CreateUserValidationService>();
        }

        public static void AddIdentityService(this IServiceCollection service, IConfigurationRoot configuration)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserStore<MongoIdentityUser>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbOptions>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new MongoUserStore<MongoIdentityUser>(database, loggerFactory);
            });
        }

        public static void AddMongoIdentityService(this IServiceCollection service)
        {
            service.AddSingleton<IdentityMarkerService>();
            service.AddSingleton<IUserValidator<MongoIdentityUser>, UserValidator<MongoIdentityUser>>();
            service.AddSingleton<IPasswordValidator<MongoIdentityUser>, PasswordValidator<MongoIdentityUser>>();
            service.AddSingleton<IPasswordHasher<MongoIdentityUser>, PasswordHasher<MongoIdentityUser>>();
            service.AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            service.AddSingleton<IdentityErrorDescriber>();
            service.AddSingleton<ISecurityStampValidator, SecurityStampValidator<MongoIdentityUser>>();
            service.AddSingleton<IUserClaimsPrincipalFactory<MongoIdentityUser>, UserClaimsPrincipalFactory<MongoIdentityUser>>();
            service.AddSingleton<UserManager<MongoIdentityUser>, UserManager<MongoIdentityUser>>();
            service.AddScoped<SignInManager<MongoIdentityUser>, SignInManager<MongoIdentityUser>>();
        }

        public static void AddAutoMapper(this IServiceCollection service, MapperConfiguration mapperConfiguration)
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfig());
            });

            service.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddMongoDbInstance(this IServiceCollection service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IMongoHelper, MongoHelper>();
        }
    }
}