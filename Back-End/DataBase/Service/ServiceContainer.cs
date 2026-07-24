using DataBase.Contexts;
using DataBase.Repository;
using FluentValidation;
using Identity.Authentication.Base;
using Identity.Authentication.Repositories;
using library.AppLoger;
using Library.AppLoger;
using Library.Features.Users.Queries;
using Library.IRepository;
using Library.MappingProfiles;
using Library.Models;
using Library.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace DataBase.Service
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInjectionOptionsDB(this IServiceCollection services, IConfiguration conf)
        {
            string MyConnectionStr = "MyConnectionStr";

            // 1. Database Context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conf.GetConnectionString(MyConnectionStr),
                sqlOption =>
                {
                    sqlOption.MigrationsAssembly(typeof(AppDbContext).Assembly);
                    sqlOption.EnableRetryOnFailure();
                }),
                ServiceLifetime.Scoped
            );

            // 2. Identity Configuration (Fixed Roles to use Guid)
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // 3. Authentication & JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = conf["JWT:Issuer"],
                    ValidAudience = conf["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["JWT:Key"]!))
                };
            });

            services.AddMemoryCache();

            // 4. AutoMapper Registration
            //services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);

            // 5. MediatR Registration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPagedUsersQuery).Assembly));

            // 6. Repositories & Unit of Work
            services.AddScoped(typeof(IAppLoger<>), typeof(SerilogerAppAdapter<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBoatRepository, BoatRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICaptainRepository, CaptainRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 7. Services & Managements
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagement>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddHostedService<TripStatusUpdaterService>();

            // 8. File Service Registration (Missing in your code)
            services.AddScoped<IFileService, FileService>();

            services.AddHttpContextAccessor();


            // 9. Fluent Validation
            services.AddScoped<IValidator<library.DTOs.DtoLoginUser>, Identity.Validation.LoginUserValidator>();
            services.AddScoped<IValidator<library.DTOs.DtoCreateUser>, Identity.Validation.CreateUserValidator>();

            return services;
        }

        public static IApplicationBuilder AddMiddleWareDb(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}