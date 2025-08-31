using DotEnv;
using Riksbyggen.Data;
using Microsoft.EntityFrameworkCore;
using Riksbyggen.Repositories;
using Riksbyggen.Repositories.Interfaces;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration["Webhook:Secret"] = Environment.GetEnvironmentVariable("WEBHOOK_SECRET");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User Id=sa;Password={dbPassword};Encrypt=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVite", policy =>
        {
            policy.WithOrigins("http://138.68.79.101:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });


builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

    app.UseCors("AllowVite");

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
