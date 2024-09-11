using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Songcash.Configuration;
using Songcash.Model.Configuration;
using Songcash.Repository;
using Songcash.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.GetSection("Authentication").Bind(authenticationConfiguration);

builder.Services.Configure<DatabaseConfiguration>(
    builder.Configuration.GetSection("DatabaseConfiguration"));
builder.Services.Configure<AuthenticationConfiguration>(
    builder.Configuration.GetSection("Authentication"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/auth/login";
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.Secret))
            };
        })
    .AddGoogle(options =>
        {
            options.ClientId = authenticationConfiguration.ClientId;
            options.ClientSecret = authenticationConfiguration.ClientSecret;
        });

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<RequestRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
