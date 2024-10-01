using Authentication.Lib.Data;
using Authentication.Lib.Entities;
using Authentication.Lib.Extensions;
using Authentication.Lib.Extensions.Email;
using Authentication.Lib.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Uygulama ayarlar�n� y�kleyin
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Servisleri ekleyin
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.LoadDataLayerExtension(builder.Configuration); // DbContext'i buradan ekliyoruz
builder.Services.AddSwaggerGen();
// JWT ayarlar�n� al
var jwtSettings = builder.Configuration.GetSection("Jwt");

// JWT kimlik do�rulama ayarlar�n� yap�land�r
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<IEmailSender>(provider =>
    new EmailSender(
        builder.Configuration["EmailSettings:SmtpHost"],
        int.Parse(builder.Configuration["EmailSettings:SmtpPort"]),
        builder.Configuration["EmailSettings:SmtpUsername"],
        builder.Configuration["EmailSettings:SmtpPassword"]
    ));
// Uygulama yap�land�rmas�
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
