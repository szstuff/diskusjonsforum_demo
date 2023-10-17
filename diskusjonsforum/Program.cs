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

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ThreadDbContext>();

    /*builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ThreadDbContext>();*/

builder.Services.AddRazorPages(); //order of adding services does not matter
builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseRouting();

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
app.UseAuthorization();

app.MapRazorPages();

app.Run();