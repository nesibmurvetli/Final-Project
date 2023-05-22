using END_Project.DAL;
using END_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
builder.Services.AddIdentity<AppUser, IdentityRole>(IdentityOptions =>
{
    IdentityOptions.Password.RequiredLength = 8;   /*simvol sayının min uzunluğu*/
    IdentityOptions.Password.RequireNonAlphanumeric = false;  /*simvollar olmasada olar*/
    IdentityOptions.Password.RequireUppercase = true;          /* vacibdir*/
    IdentityOptions.Password.RequireLowercase = true;
    IdentityOptions.Lockout.MaxFailedAccessAttempts = 4;  /*nece defe kodu sehv yazmaq*/
    IdentityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);  /*nece dəqiqəlik bloklanmaq*/
    IdentityOptions.Lockout.AllowedForNewUsers = true;      /*qeydiyyatdan kecmeni müvəqqəti dayandırmaq*/
    IdentityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";  /*bunlardan yalnız istifadə et*/
    IdentityOptions.User.RequireUniqueEmail = true;
    //IdentityOptions.SignIn.RequireConfirmedAccount = true;
    //IdentityOptions.SignIn.RequireConfirmedEmail = false;
    //IdentityOptions.SignIn.RequireConfirmedPhoneNumber = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
