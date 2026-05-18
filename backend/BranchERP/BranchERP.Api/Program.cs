using BranchERP.Api.Extensions;
using BranchERP.Api.Filters;
using BranchERP.Api.Middleware;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Mapping;
using BranchERP.Infrastructure.Configuration;
using BranchERP.Infrastructure.Data;
using BranchERP.Infrastructure.Identity;
using BranchERP.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BranchERP.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ============================
            // CORS
            // ============================
            //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins" ;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ============================
            // Database
            // ============================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ============================
            // Identity
            // ============================
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;

                // إعدادات الباسورد (زي ما كانت)
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();   // 🔥 ضروري لتشغيل Reset Password

            // 🔥 الحل النهائي لمشكلة تسجيل الدخول بعد تغيير الباسورد
            builder.Services.Configure<PasswordHasherOptions>(options =>
            {
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            });

            // ============================
            // JWT
            // ============================
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            // ============================
            // Permission Services
            // ============================
            builder.Services.AddScoped<PermissionService>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // ============================
            // AutoMapper
            // ============================
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // ============================
            // Project Services
            // ============================
            builder.Services.AddProjectServices();

            // ============================
            // Auth Service
            // ============================
            builder.Services.AddScoped<IAuthService, AuthService>();

            // ============================
            // FluentValidation
            // ============================
            builder.Services.AddValidatorsFromAssemblyContaining<BranchERP.Application.DTOs.Country.CountryCreateUpdateDto>();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            // ============================
            // Controllers
            // ============================
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ============================
            // CORS
            // ============================
        

            // ============================
            // Database Seeding
            // ============================
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;

            //    var context = services.GetRequiredService<AppDbContext>();
            //    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            //    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            //    await AppDbContextSeed.SeedAsync(context);
            //    await AppDbContextSeed.SeedRolesAsync(roleManager);
            //    //await AppDbContextSeed.SeedRolePermissionsAsync(context);
            //    await AppDbContextSeed.SeedAdminAsync(userManager, roleManager);
            //}
          
            // ============================
            // Middlewares
            // ============================
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "docs";
            });

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
          
            app.UseStaticFiles();
            app.MapControllers();

            // ============================
            // Run
            // ============================
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            await app.RunAsync();
        }
    }
}
