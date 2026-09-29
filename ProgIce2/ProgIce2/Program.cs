using Microsoft.EntityFrameworkCore;
using ProgIce2.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BursaryClaimsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BursaryClaimsConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// This creates the coursework database on first run. For a production system,
// replace this with versioned Entity Framework migrations.
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<BursaryClaimsContext>();
    database.Database.EnsureCreated();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
