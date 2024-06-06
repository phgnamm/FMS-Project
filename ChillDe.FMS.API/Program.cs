using ChillDe.FMS.API;
using ChillDe.FMS.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ChillDe.FMS.Repositories;
using ChillDe.FMS.Repositories.Common;
using System.Text;
using ChillDe.FMS.API.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
    options.JsonSerializerOptions.DictionaryKeyPolicy = new KebabCaseNamingPolicy();

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "FMS NextBean Edition", Version = "v1" });
});

// Local Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});

// ===================== FOR DEPLOY AZURE =======================

//var connection = String.Empty;
//if (builder.Environment.IsDevelopment())
//{
//	builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//	connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//	connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}

//builder.Services.AddDbContext<AppDbContext>(options =>
//  options.UseSqlServer(connection));

// ==================== NO EDIT OR REMOVE COMMENT =======================


// Add API Configuration
builder.Services.AddAPIConfiguration();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Cookie.Name = "refreshToken";
    options.Cookie.HttpOnly = true; // Ensure HTTP-only cookie
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Make sure to send only over HTTPS
    options.SlidingExpiration = true; // Renew the cookie expiration time on each request
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["OAuth2:Google:ClientId"];
    options.ClientSecret = builder.Configuration["OAuth2:Google:ClientSecret"];
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        builder =>
        {
            builder
                //.AllowAnyOrigin()
                .WithOrigins("http://localhost:5173", "https://fms-nextbean-edition.vercel.app",
                    "http://localhost:63661")
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Allow CORS
app.UseCors("cors");

//Initial Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await InitialSeeding.Initialize(services);
}

// Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<KebabQueryStringMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();