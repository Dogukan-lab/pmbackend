using pmbackend;
using pmbackend.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Configurator configurator = new Configurator(builder.Services, builder);

builder.Services.AddTransient<IAuthService, AuthenticationService>();

configurator.BuildServices();

builder.Services.AddDbContext<PaleMessengerContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("pmuser");
    options
        .UseMySql(connectionString, ServerVersion.AutoDetect
        (connectionString));
});

var app = builder.Build();

/*Needed for later */
// SeedDb.SeedUserIdentities(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Configures all capabilities of application
configurator.ConfigureApp(app);

app.Run();
