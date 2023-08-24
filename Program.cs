using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MoveMateWebApi.Database;
using MoveMateWebApi.Middleware;
using MoveMateWebApi.Repositories;
using MoveMateWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.Configure<JsonOptions>(options =>
// {
//     options.SerializerOptions.PropertyNameCaseInsensitive = false;
//     options.SerializerOptions.PropertyNamingPolicy = null;
//     options.SerializerOptions.WriteIndented = true;
// });

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<FireBaseService>();
builder.Services.AddSingleton<ITokenFactory, JwtTokenFactory>();
builder.Services.AddDbContext<MoveMateDbContext>();
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();

// Build the application
var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoveMateDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

