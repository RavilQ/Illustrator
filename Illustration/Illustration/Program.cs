using Illustration.DAL;
using Illustration.Models;
using Illustration.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IllustratorDbContext>(opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")); });

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<IllustratorDbContext>();

builder.Services.AddScoped<LayoutService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/AdminPanel"))
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/AdminPanel/account/login" + redirectPath.Query);
        }
        else
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + redirectPath.Query);
        }

        return Task.CompletedTask;
    };
});


builder.Services.AddAuthentication()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = "1052189299383-qbkpoco0iv1s9r8p2sj7qrqdf6kkh8bt.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.SignInScheme = IdentityConstants.ExternalScheme;
}).AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = "1198809320739698";
    facebookOptions.AppSecret = "4c262dd94602bd167d23c4cfedd13c59";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
