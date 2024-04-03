using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Identity;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("roles")]
    [Authorize(Policy = "RequireAdminRole")]
    public class RolesController : Controller
    {
        private readonly UserManager<User> userManager;

        public RolesController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, IList<string> roles)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);

                //await userManager.UpdateSecurityStampAsync(user);
            }

            return RedirectToAction("GetAll", "Account");
        }
    }
}
