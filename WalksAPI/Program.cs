using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalksAPI.Data;
using WalksAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//add authentication in to swagger add options
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//builder.Services.AddSwaggerGen(options=>
//{
//    var securityScheme = new OpenApiSecurityScheme
//    {
//        Name = "JWT Authentication",
//        Description = "Enter a valid JWT bearer token",
//        In = ParameterLocation.Header,
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };
//    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {securityScheme,new string[] {} }
//    });
//});
//inject fluent validation
builder.Services.
    AddFluentValidation(options=>options.RegisterValidatorsFromAssemblyContaining<Program>());

//inject dbcontext class into service collection
//type of dbcontext is the name of dbcontext class name
//give options to use sql server
builder.Services.AddDbContext<WalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Walks"));
});

//inject interface and implementation in services
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();
//using static repository (can't use AddScoped) so use AddSingleton so that one instance of static repository would be generated
//so no new guids generated
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddSingleton<IUserRepository, StaticUserRepository>();//used in Authcontroller
builder.Services.AddScoped<ITokenHandler, WalksAPI.Repositories.TokenHandler>();//addscoped means it is diff for diff request

//inject profile into soln
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//inject authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { 
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//use authentication in middle ware pipeline
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
//app.MapControllerRoute(
//    name: default,
//    pattern: "{controller=walks}/{action}/{id?}");

app.Run();
