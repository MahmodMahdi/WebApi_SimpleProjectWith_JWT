using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi_Demo.Authentication;
using WebApi_Demo.Models;
using WebApi_Demo.Repository;

namespace WebApi_Project;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        #region Swagger Configuration
        builder.Services.AddSwaggerGen(swagger =>
        {

            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Asp.Net 7 Web API",
                Description = "WebApi_Project"
            });
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insert JWT Token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                            }
                        },
                    new string[]{}
                    }
            });

        });
        #endregion
        #region Context
        var DB = builder.Configuration.GetConnectionString("DB");
        builder.Services.AddDbContext<ApplicationEntity>(options =>
        {
            options.UseSqlServer(DB);
        });
        #endregion
        #region Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
        options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
        })
        .AddEntityFrameworkStores<ApplicationEntity>();

        //[Authorize] used JWT Token in check Authatication
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey =
                   new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
            };
        });
        #endregion
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        builder.Services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("MyPolicy", CorsPolicyBuilder =>
            {
                CorsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // To map static file (html,images,videos,extensions)
        app.UseStaticFiles();

        //// how to make a filter to make the only https
        //app.Use(async (context,next) =>
        //{
        //    // Request Logic
        //    if (!context.Request.IsHttps)
        //    {
        //        await context.Response.WriteAsJsonAsync(new GeneralResponse("Use HTTPS"));
        //        return;
        //    }
        //    await next(context);
        //});
        // setting core polices
        app.UseCors("MyPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication(); //check JWT Token

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

