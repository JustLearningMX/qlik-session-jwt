using BusinessLogic.Models;
using BusinessLogic.Service;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal5173", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
              .WithMethods("POST", "GET", "OPTIONS")
              .WithHeaders("Content-Type", "Accept")
              .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient(typeof(ApplicationJWTService));
builder.Services.AddHttpClient<QlikAdminService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// bind JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<QlikCloudSettings>(builder.Configuration.GetSection("QlikCloudSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var qlikCloudSettings = builder.Configuration.GetSection("QlikCloudSettings");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowLocal5173");

app.MapControllers();

app.Run();
