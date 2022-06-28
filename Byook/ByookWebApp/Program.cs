global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Byook.DataAccess;
global using Byook.Models;
global using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using Byook.Utility;
using Byook.Utility.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ByookDbContext>(option => option.UseInMemoryDatabase("byook"));
builder.Services.AddDbContext<ByookDbContext>(option => option.UseSqlite("DataSource=byook.db"));

var tokenOptions = new TokenOptions(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], builder.Configuration["Jwt:SecretKey"]);
builder.Services.AddJwtAuthenticationWithProtectedCookie(tokenOptions, "Home");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(Seller), policy => policy.RequireClaim(ClaimTypes.Role));
    options.AddPolicy(nameof(Consumer), policy => policy.RequireClaim(ClaimTypes.Role));
});

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
