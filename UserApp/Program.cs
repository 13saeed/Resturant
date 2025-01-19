using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resturant.Data;
using Resturant.Models;
using Microsoft.Extensions.DependencyInjection;
using Resturant.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<Users , IdentityRole>() 
   .AddEntityFrameworkStores<AppDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<FoodsServices>();
builder.Services.AddScoped<Foods>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true; // کوکی فقط از سمت سرور خوانده شود
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // مدت اعتبار کوکی (به دلخواه)
        options.SlidingExpiration = true; // تمدید خودکار کوکی غیرفعال
        options.Cookie.IsEssential = true; // برای GDPR
        options.Cookie.SameSite = SameSiteMode.Lax; // تنظیمات SameSite
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // فقط HTTPS
        options.Cookie.Expiration = TimeSpan.Zero;
    });

    var app = builder.Build();

using (var scope = app.Services.CreateScope())

{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<Users>>();
    var roleManegar = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    var adminFullName = "Admin";
    var adminEmail = "Admin@gmail.com";
    var adminPassword = "Admin1234";


    if (!await roleManegar.RoleExistsAsync("User"))
    {
        await roleManegar.CreateAsync(new IdentityRole("User"));
    }

    if (await roleManegar.RoleExistsAsync("Admin"))
    {
        await roleManegar.CreateAsync(new IdentityRole("Admin"));
    }
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null) 
    {
        adminUser = new Users
        {
            FullName = adminFullName,
            UserName = adminEmail,
            Email = adminEmail
        };
        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
