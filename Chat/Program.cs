using Chat.Config;
using Chat.DataContext;
using Chat.Hubs;
using Chat.Models;
using Chat.Repositories;
using Chat.Repositories.Abstraction;
using Chat.Services;
using Chat.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "chat", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

services.AddSignalR();
services.AddControllers();
services.AddMemoryCache();

services.AddHttpContextAccessor();
services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true)
        .Build();
    });
});

services.AddDbContext<AppDataContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

services.AddIdentity<AppIdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDataContext>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = SettingsManager.AppSettings["JWT:ValidIssuer"],
            ValidAudience = SettingsManager.AppSettings["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsManager.AppSettings["JWT:Secret"]))
        };
    });

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IClanRepository, ClanRepository>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IClanService, ClanService>();
services.AddScoped<IMessageCacheRepository, MessageCacheRepository>();
services.AddTransient<IAuthService, AuthService>();
services.AddTransient<IJwTokenService, JwTokenService>();
services.AddSingleton<IDictionary<string, UserConnection>>(opt => new Dictionary<string, UserConnection>());
services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(SettingsManager.AppSettings["RedisConnection"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{   
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "chat_v1");
});

app.UseCors();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

//app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("/chat");

app.Run();
