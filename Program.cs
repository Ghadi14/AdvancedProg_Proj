using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HouseMates.Areas.Identity.Data;
using HouseMates.Business;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Register your business and database services
builder.Services.AddScoped<IHouseBusiness, HouseBusiness>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
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

app.MapRazorPages();

// Seed initial admin user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Check if the admin user already exists
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        // Create and set up the initial admin user if not present
        adminUser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            IsAdmin = true,
            firstname = "rami", // Replace with actual values
            lastname = "rachini"
        };
        await userManager.CreateAsync(adminUser, "AdminPassword123!");
    }
    else
    {
        // Update existing admin user details if needed
        adminUser.IsAdmin = true;
        adminUser.firstname = "rami";
        adminUser.lastname = "rachini";
        await userManager.UpdateAsync(adminUser);
    }
}

app.Run();
