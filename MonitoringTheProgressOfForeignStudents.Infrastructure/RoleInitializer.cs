using Microsoft.AspNetCore.Identity;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Identity;
using System.Text.Json;
using System.Threading.Tasks;
namespace MonitoringTheProgressOfForeignStudents.Infrastructure
{
    public static class RoleInitializer
    {
        public static async Task InitRoles(UserManager<User> usermgr, RoleManager<IdentityRole> rolemgr)
        {
            if(await rolemgr.FindByNameAsync("department") == null)
            {
                await rolemgr.CreateAsync(new IdentityRole("department"));
            }
            if(await rolemgr.FindByNameAsync("admin") == null)
            {
                await rolemgr.CreateAsync(new IdentityRole("admin"));
            }

            if (File.Exists("admin.json"))
            {
                var admin = JsonSerializer.Deserialize<Admin>(File.ReadAllText("admin.json"));

                if (admin != null && await usermgr.FindByNameAsync(admin.Username) == null)
                {
                    if (string.IsNullOrEmpty(admin.Username) || string.IsNullOrEmpty(admin.Password))
                        return;
                    User adminUser = new User { Email = admin.Username, UserName = admin.Username, Name = "", Surname = "", Patronymic = "admin" };
                    IdentityResult result = await usermgr.CreateAsync(adminUser, admin.Password);
                    if (result.Succeeded)
                    {
                        await usermgr.AddToRoleAsync(adminUser, "admin");
                    }
                }
            }
        }

        private record Admin
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
