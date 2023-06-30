using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pizza_Delivery_System.DBContext;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.JWT;
using Pizza_Delivery_System.MongoDb;
using Pizza_Delivery_System.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Injecting Jwt Token
//builder.Services.AddScoped<IJwtToken, JwtToken>();

// DI of DbContext and Connection string
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Pizza_Api_System")));

//Injecting UserServices
builder.Services.AddScoped<IUserServices, UserServices>();

//Injecting ManagerServices
builder.Services.AddScoped<IManagerServices, ManagerServices>();

//Injecting JWT Services
builder.Services.AddScoped<IJwtToken, JwtToken>();

//Injecting MongoDb 
builder.Services.AddTransient<IMongoRepo, MongoRepo>();


builder.Services.Configure<MongoSetting>(builder.Configuration.GetSection("MongoDb"));

//Adding Authentication
builder.Services.AddAuthentication(
    // Adding Jwt AuthenticationScheme
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        // Adding Validations
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Create Doc using Swagger Generator
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pizza API", Version = "v1" });
    // JWT Bearer configuration
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
    options.AddSecurityRequirement(
        // Ensure that only refernce id is Considered when 2 dictionary keys are compared
        new OpenApiSecurityRequirement
        {
            //Create new object
            {
                // Adding elements
                new OpenApiSecurityScheme
                {
                    // Refernce object 
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                    // Name of Http Authorization scheme
                    Scheme = "Oauth2",
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header
                },
                // Have default initialise capacity 
                new List<string>()
            }
        }
    );
});





var app = builder.Build();

//For Seeding Data 
var seedData = app.Services.GetRequiredService<IMongoRepo>();
seedData.SeedDataInMenu();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
