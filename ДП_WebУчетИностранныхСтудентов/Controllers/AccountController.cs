using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonitoringTheProgressOfForeignStudents.Infrastructure.Identity;
using MonitoringTheProgressOfForeignStudents.ViewModels.AccountVM;
using MonitoringTheProgressOfForeignStudents.ViewModels.Auth;

namespace MonitoringTheProgressOfForeignStudents.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager,
                                SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("all")]
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var usersWithRoles = userManager.Users.ToList()
                .Select(x =>
                    new UserWithRolesViewModel
                    {
                        User = x,
                        Roles = userManager.GetRolesAsync(x).Result
                    }
                ).OrderBy(x => x.User.Id);

            return View(new GetUserListViewModel
            {
                UserWithRoles = usersWithRoles
            });
        }

        [Route("all")]
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public IActionResult GetAll(GetUserListViewModel viewModel)
        {
            var usersWithRoles = userManager.Users.ToList();

            if (!string.IsNullOrEmpty(viewModel.SearchAccount.SearchString))
            {
                usersWithRoles = usersWithRoles.Where(x => x.ToString().ToLower().Contains(viewModel.SearchAccount.SearchString.ToLower())).ToList();
            }

            viewModel.UserWithRoles = usersWithRoles.Select(x => new UserWithRolesViewModel
            {
                User = x,
                Roles = userManager.GetRolesAsync(x).Result
            });

            return View(viewModel);
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = registerViewModel.Name,
                    Surname = registerViewModel.Surname,
                    Patronymic = registerViewModel.Patronymic,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email
                };

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded) return RedirectToAction("Index", "Home");

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(registerViewModel);
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email,
                    loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded) return RedirectToAction("Index", "Home");
                else ModelState.AddModelError("", "Неправильная почта или пароль");
            }

            return View(loginViewModel);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
