using Diskusjonsforum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using diskusjonsforum.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Sqlite.Diagnostics.Internal;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("diskusjonsforumIdentityDbContextConnection") ?? throw new 
    InvalidOperationException("Connection string 'diskusjonsforumIdentityDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ThreadDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ThreadDbContextConnection"]);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ThreadDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddRazorPages(); //order of adding services does not matter

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600); //1 hour
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Save",
        pattern: "Comment/Save",
        defaults: new { controller = "Comment", action = "Save" }
    );

});

app.MapDefaultControllerRoute();

app.UseSession();
app.UseAuthentication();

app.MapRazorPages();

app.Run();