﻿using ShoppingCart.Components; // ✅ Ensure this is added
using ShoppingCart.Service;
using Repository.Services.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI;
using Repository.Services.Contract;
using Repository.Services.Library;
using static ShoppingCart.Service.AuthenticationAppService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repository.Services.Extensions;
using Microsoft.AspNetCore.Authentication;
using Blazored.Toast;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth-Token";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.AccessDeniedPath = "/auth/access-denied";
    });

builder.Services.AddAuthentication();

// Add database context
//builder.Services.AddDbContext<AddDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register necessary services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.ConfigureRepoServiceImplementation(builder.Configuration);


builder.Services.AddScoped<ISecurityRepositoryService, SecurityRepositoryService>();
builder.Services.AddScoped<AuthenticationAppService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepositoryService, UserRepositoryService>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IProductRepositoryService, ProductRepositoryService>();

builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ICartRepositoryService, CartRepositoryService>();

builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<IOrdersRepositoryService, OrdersRepositoryService>();

builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<IProfileRepositoryService, ProfileRepositoryService>();

builder.Services.AddScoped<MessageService>();



// builder.Services.AddScoped<AuthenticationAppService, AuthenticationAppService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseAntiforgery();

app.MapControllers();
//app.MapBlazorHub();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapGet("/auth/logout", async (HttpContext httpContext) =>
{
    // Clear all cookies
    foreach (var cookie in httpContext.Request.Cookies.Keys)
    {
        httpContext.Response.Cookies.Delete(cookie);
    }

    // Sign out the user
    await httpContext.SignOutAsync(Constants.AuthScheme);

    // Redirect to the login page
    httpContext.Response.Redirect("/home");
});


app.Run();
