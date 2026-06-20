using DataBase.Contexts;
using FluentValidation;
using Identity.Authentication.Base;
using Identity.Authentication.Repositories;
using library.AppLoger;
using Library.AppLoger;
using Library.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataBase.Service
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInjectionOptionsDB(this IServiceCollection services, IConfiguration conf)
        {
            string MyConnectionStr = "MyConnectionStr";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conf.GetConnectionString(MyConnectionStr),
                sqlOption =>
                {
                    sqlOption.MigrationsAssembly(typeof(AppDbContext).Assembly);
                    sqlOption.EnableRetryOnFailure();
                }),
                ServiceLifetime.Scoped
            );

            services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

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

            services.AddScoped(typeof(IAppLoger<>), typeof(SerilogerAppAdapter<>));
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagement>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(cfg =>
            {
                //cfg.AddProfile<ProductProfile>();
            });
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllProducts).Assembly));

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