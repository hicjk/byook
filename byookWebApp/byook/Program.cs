global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using byook.DataAccess;
global using byook.Models;
global using byook.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using byook.Utility;
using byook.Utility.Extensions;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ByookDbContext>(option => option.UseInMemoryDatabase("byook"));
builder.Services.AddDbContext<ByookDbContext>(option => option.UseSqlite("DataSource=byook.db"));

//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    })
//    .AddCookie(options =>
//    {
//        options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
//    });

var tokenOptions = new TokenOptions(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], builder.Configuration["Jwt:SecretKey"]);
builder.Services.AddJwtAuthenticationWithProtectedCookie(tokenOptions, "Home");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(Seller), policy => policy.RequireClaim(ClaimTypes.Role));
    options.AddPolicy(nameof(Consumer), policy => policy.RequireClaim(ClaimTypes.Role));
});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.ExpireTimeSpan = TimeSpan.FromHours(1);
//    options.LoginPath = "/Account/LogIn";
//    options.LogoutPath = "/Account/LogOut";
//    options.AccessDeniedPath = "/Home/Index";
//});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.RequireAuthenticatedSignIn = false;
//})
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//    };
//});
builder.Services.AddMvc();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
