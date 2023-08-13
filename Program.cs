using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoveMateWebApi.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MoveMateDbContext>();

// Configure authentication
var authentication = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
authentication.AddJwtBearer(ConfigureJwtBearer);

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


void ConfigureJwtBearer(JwtBearerOptions options) {
	string privateKey = builder.Configuration["Jwt:PrivateKey"] ?? string.Empty;
	SymmetricSecurityKey ssk = new(Encoding.UTF8.GetBytes(privateKey));

	options.IncludeErrorDetails = builder.Environment.IsDevelopment();
	options.TokenValidationParameters = new()
	{
		IssuerSigningKey = ssk,
		RequireExpirationTime = true,
		ValidateLifetime = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Issuer"]
	};
}