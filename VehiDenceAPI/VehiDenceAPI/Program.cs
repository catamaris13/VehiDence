using Microsoft.EntityFrameworkCore;
using User.Management.Service.Services;
using VehiDenceAPI.Data;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configurare DbContext
builder.Services.AddDbContext<AplicatieDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("VehiDenceConnectionString")));

// Adăugare serviciu pentru e-mail
builder.Services.AddScoped<IEmailServices, EmailServices>();

// Adăugare politică CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Adăugare servicii necesare
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurare Hangfire
builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("VehiDenceConnectionString");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

// Construire aplicație
var app = builder.Build();

// Configurare pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicare politică CORS
app.UseCors();

app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();