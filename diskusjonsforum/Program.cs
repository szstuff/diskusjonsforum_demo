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

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ThreadDbContext>().AddDefaultTokenProviders().AddDefaultUI();


builder.Services.AddRazorPages(); //order of adding services does not matter
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapDefaultControllerRoute();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Save",
        pattern: "Comment/Save",
        defaults: new { controller = "Comment", action = "Save" }
    );

});

app.MapRazorPages();

app.Run();