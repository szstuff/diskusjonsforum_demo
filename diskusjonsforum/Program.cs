using Diskusjonsforum.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ThreadDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ThreadDbContextConnection"]);
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Save",
        pattern: "Comment/Save",
        defaults: new { controller = "Comment", action = "Save" }
    );

});

app.MapDefaultControllerRoute();

app.Run();