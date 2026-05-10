using Application.Services.CurrentUserServices;
using Appliction.Repositories;
using Appliction.Services.AuthServices;
using Appliction.Services.CatagoryServices;
using Appliction.Services.RequestServices;
using Appliction.Services.RoleService;
using Appliction.Services.TCServices;
using Appliction.Services.UserService;
using Infrastructre.Context;
using Infrastructre.Data;
using Infrastructre.Repositories;
using Infrastructre.Services.CurrentUserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MRS API",
        Version = "v1"
    });


    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put **_ONLY_** your JWT Bearer token hera",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityReq = new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod();
        });
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IRoleService), typeof(RoleService));
builder.Services.AddScoped(typeof(ITCService), typeof(TCService));
builder.Services.AddScoped(typeof(ICatagoryService), typeof(CatagoryService));
builder.Services.AddScoped(typeof(IRequestService), typeof(RequestService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));


var app = builder.Build();


UserSeedData.UserSeed(app.Services);

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();

var uploadPath = builder.Configuration["FileStorage:UploadPath"];
if (!string.IsNullOrEmpty(uploadPath))
{
    if (!Directory.Exists(uploadPath))
    {
        Directory.CreateDirectory(uploadPath);
    }
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadPath),
        RequestPath = "/External"
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
