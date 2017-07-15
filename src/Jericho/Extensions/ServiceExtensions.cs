namespace Jericho.Extensions
{
    using System;

    using Aggregators;
    using Aggregators.Interfaces;

    using AutoMapper;

    using Jericho.Configuration;
    using Jericho.Identity;
    using Jericho.Options;
    using Jericho.Providers;
    using Jericho.Providers.Interfaces;
    using Jericho.Services;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using MongoDB.Driver;

    public static class ServiceExtensions
    {
        public static void AddUserService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IUserService, UserService>();
        }

        public static void AddPostService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IPostService, PostService>();
        }

        public static void AddFavoriteService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IFavoritesService, FavoritesService>();
        }

        public static void AddCommentService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<ICommentService, CommentService>();
        }

        public static void AddUploadService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IUploadService, UploadService>();
        }

        public static void AddHttpContextAccessorService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddEmailService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IEmailService, EmailService>();
        }

        public static void AddIdentityService(this IServiceCollection service, IConfigurationRoot configuration)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<IUserStore<ApplicationUser>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbOptions>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new MongoUserStore<ApplicationUser>(database, loggerFactory);
            });
        }

        public static void AddMongoIdentityService(this IServiceCollection service)
        {
            service.AddScoped<IdentityMarkerService>();
            service.AddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            service.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            service.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            service.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            service.AddScoped<IdentityErrorDescriber>();
            service.AddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            service.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser>>();
            service.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            service.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            service.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void AddAutoMapper(this IServiceCollection service, MapperConfiguration mapperConfiguration)
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            service.AddScoped(sp => mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddMongoDbInstance(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IDataProvider, DataProvider>();
        }

        public static void AddCommentAggregator(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddScoped<ICommentAggregator, CommentAggregator>();
        }
    }
}