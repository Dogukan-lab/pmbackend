using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using pmbackend.Database;
using System.Text;
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

            //Swagger for debugging
            m_services.AddEndpointsApiExplorer();
            m_services.AddSwaggerGen();

            //Adding automapper for mapping DTO's to actual models.
            m_services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Adding authentication for user login
            m_services.AddIdentity<PmUser, IdentityRole<int>>(
                options =>
                {
                    //Identity requirement options
                    options.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<PaleMessengerContext>()
                .AddDefaultTokenProviders();
            m_services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                        ValidIssuer = m_configBuilder.Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience = m_configBuilder.Configuration.GetSection("Jwt:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_configBuilder.Configuration.GetSection("Jwt:Key").Value))
                    };
                });
        }

        public void ConfigureApp(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
