using diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
//using diskusjonsforum.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ItemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ItemDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddControllers().AddNewtonsofJson( ... ); Fant dette i koden hans i uke 8/authentication. Hvis vi ikke har lagt det til har vi nok hoppet over noe (før authentication)

builder.Services.AddDbContext<ThreadDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ThreadDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ThreadDbContext>();

// builder.Services.AddScoped<>() Fant dette i koden hans i uke 8/authentication. Hvis vi ikke har lagt det til har vi nok hoppet over noe (før authentication)

builder.Services.AddRazorPages();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();