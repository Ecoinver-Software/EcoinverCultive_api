using System.Text;
using EcoinverGMAO_api.Data;
using EcoinverGMAO_api.Models.Identity;
using EcoinverGMAO_api.Profiles;
using EcoinverGMAO_api.Seeders;
using EcoinverGMAO_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1) Registro de servicios y configuración de dependencias

// Repositorios genéricos y servicios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICommercialNeedsService, CommercialNeedsService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICultiveService, CultiveService>();
builder.Services.AddScoped<ICultivePlanningService, CultivePlanningService>();
builder.Services.AddScoped<ICultivePlanningDetailsService, CultivePlanningDetailsService>();
builder.Services.AddScoped<ICultiveProductionService, CultiveProductionService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<ICommercialNeedsPlanningService, CommercialNeedsPlanningService>();
builder.Services.AddScoped<ICommercialNeedsPlanningDetailsService, CommercialNeedsPlanningDetailsService>();

// Servicio para leer la base del ERP por ADO.NET
builder.Services.AddTransient<ErpDataService>();

// DbContext principal de tu aplicación con MySQL (Pomelo)
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 40))
    );
    o.EnableSensitiveDataLogging();
});

// Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Opciones de Identity (contraseñas, etc.)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
});

// JWT para el token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"])
        )
    };
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(CompanyProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(CommercialNeedsProfile));
builder.Services.AddAutoMapper(typeof(RoleProfile));
builder.Services.AddAutoMapper(typeof(ClientProfile));
builder.Services.AddAutoMapper(typeof(CultiveProfile));
builder.Services.AddAutoMapper(typeof(CultivePlanningProfile));
builder.Services.AddAutoMapper(typeof(CultivePlanningDetailsProfile));
builder.Services.AddAutoMapper(typeof(CultiveProductionProfile));
builder.Services.AddAutoMapper(typeof(GenderProfile));
builder.Services.AddAutoMapper(typeof(CommercialNeedsPlanningProfile));
builder.Services.AddAutoMapper(typeof(CommercialNeedsPlanningDetailsProfile));

// Autorización
builder.Services.AddAuthorization();

// Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("https://localhost:4200",
            "https://172.16.10.65:4200"
            )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 2) Seeding de datos iniciales (si corresponde)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error durante el seed: {ex.Message}");
    }
}

// 3) Middlewares y configuración del pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAngularDev");

// **Importante**: primero autenticación, luego autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 4) Run
app.Run();
