using Microsoft.AspNetCore.Identity;
using MonitoringTheProgressOfForeignStudents.Application;
using MonitoringTheProgressOfForeignStudents.Infrastructure;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Identity;
using MonitoringTheProgressOfForeignStudents.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.ImplementAppPersistence(builder.Configuration);
builder.Services.ImplementAuthPersistence(builder.Configuration);
builder.Services.AddIdentity<User, IdentityRole>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 8;
    o.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<SecurityStampValidatorOptions>(o =>
{
    o.ValidationInterval = TimeSpan.Zero; // для обновления роли
});

builder.Services.AddConverters();
builder.Services.AddRepositories();
builder.Services.AddAppServices();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireWorkerRoles",
         policy => policy.RequireRole("department", "admin"));

    options.AddPolicy("RequireAdminRole",
         policy => policy.RequireRole("admin"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var usermgr = services.GetRequiredService<UserManager<User>>();
    var rolemgr = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitRoles(usermgr, rolemgr);
}

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
