using PortfolioApp1.Controllers;
using PortfolioApp1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add reCAPTCHA anti-spam services
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleCaptchaKeys"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

// Add Email service
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailAuthentication"));

// build the app with the declared services
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
