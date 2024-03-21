using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using pmbackend.Database;
using System.Text;
using Microsoft.OpenApi.Models;
using pmbackend.Hub;
using pmbackend.Models;

namespace pmbackend
{
    /// <summary>
    /// A Configurator to setup the project
    /// This was so that the clutter wasn't visible inside of the program.cs
    /// Will be specialised to do fun generic typing to custom setup the configurator like a decorator.
    /// </summary>
    public class Configurator
    {
        public IServiceCollection m_services;
        public WebApplicationBuilder m_configBuilder;

        public Configurator(IServiceCollection services,
            WebApplicationBuilder configBuilder)
        {
            m_services = services;
            m_configBuilder = configBuilder;
        }

        public void BuildServices()
        {
            //Adding controllers to do HTTP Requests with.
            m_services.AddControllers();

            //Add policy
            m_services.AddCors(options => options.AddPolicy("CorsPolicy", policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));

            //Swagger for debugging
            m_services.AddEndpointsApiExplorer();
            m_services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });

            //Adding automapper for mapping DTO's to actual models.
            m_services.AddAutoMapper(typeof(Mapper.MapProfile));

            m_services.AddScoped<IPmUserRepository, PmUserRepository>();
            m_services.AddScoped<IChatRepository, ChatRepository>();

            //Adding authentication for user login
            m_services.AddIdentity<PmUser, IdentityRole<int>>(
                    options =>
                    {
                        //Identity requirement options
                        options.Password.RequiredLength = 5;
                        options.Password.RequireDigit = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                .AddEntityFrameworkStores<PaleMessengerContext>()
                .AddDefaultTokenProviders();
            m_services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = m_configBuilder.Configuration
                            .GetSection("Jwt:Issuer").Value,
                        ValidAudience = m_configBuilder.Configuration
                            .GetSection("Jwt:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(m_configBuilder.Configuration
                                .GetSection("Jwt:Key").Value))
                    };
                });
            m_services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            });        }

        public void ConfigureApp(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.UseEndpoints(endpoints => { endpoints.MapHub<ChatHub>("/chatHub"); });
        }
    }
}