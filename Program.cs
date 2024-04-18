using BooksApp_Spring2024;
using BooksApp_Spring2024.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//fetch the connection string
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

//defines that we will use our DBContext class with SQLserver on the connection string that we have fetched in the previous step
builder.Services.AddDbContext<BooksDbContext>(options => options.UseSqlServer(connString));

//stripe settings
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BooksDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IEmailSender, EmailSender>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BooksDbContext>().AddDefaultTokenProviders();


//build that adds our services
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

//logging in with appropriate credentials
app.UseAuthentication();

//checking whether user has permission to access functionality
app.UseAuthorization();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
