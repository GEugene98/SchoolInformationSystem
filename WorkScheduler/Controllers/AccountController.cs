using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Shared;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly Context Context;
        private readonly Logger Logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, Context context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Context = context;
            Logger = Logger.GetInstance();
        }

        [Authorize(Roles = "Директор")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var roles = Context.Roles.Where(r => model.Roles.Contains(r.Name));

            if (roles == null)
            {
                return BadRequest("Указанной роли пользователя не существует");
            }

            User user = new User
            {
                UserName = GenerateUsername(model.FirstName, model.LastName),
                FirstName = model.FirstName,
                LastName = model.LastName,
                SurName = model.SurName
            };

            var password = GeneratePassword();

            var result = await UserManager.CreateAsync(user, password);

            var userToMakeActive = Context.Users.FirstOrDefault(u => u.Id == user.Id);
            userToMakeActive.LockoutEnabled = false;
            Context.SaveChanges();

            if (result.Succeeded)
            {
                foreach (var role in roles)
                {
                    await UserManager.AddToRoleAsync(user, role.Name);
                }

                return Ok(new
                {
                    FullName = $"{user.LastName} {user.FirstName} {user.SurName}",
                    Username = user.UserName,
                    Password = password,
                    Roles = string.Join(", ", roles.Select(r => r.Name))
                }
                );
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            User user = null;

            try
            {
                user = await UserManager.FindByNameAsync(model.Username);

                if (user == null)
                {
                    ViewBag.Message = "Пользователь с таким логином не найден в системе. Повторите ввод.";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    ViewBag.Message = "Не все поля были заполнены"; 
                    return View();
                }

                if (user.LockoutEnabled)
                {
                    ViewBag.Message = "Пользователь заблокирован или удалён";
                    return View();
                }

                var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.Remember, false);

                if (result.Succeeded)
                {
                    var log = new LoginLog(user.Id);
                    Context.LoginLogs.Add(log);
                    Context.SaveChanges();
                    
                    return Redirect("/");
                }
                else
                {
                    ViewBag.Message = "Неправильный пароль. Проверьте правильность ввода.";
                }

                return View();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                ViewBag.Message = $"Неизвестная ошибка. Дополнительная информация: {ex.Message}";
                return View();
            }


        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet("Block")]
        public IActionResult Block(string userId)
        {
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);
            user.LockoutEnabled = true;
            user.GetNotifications = false;
            Context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetCurrentUserInfo")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return BadRequest("Необходимо выполнить вход");
            }

            var currentUser = Context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var userModel = new UserViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                SurName = currentUser.SurName,
                Name = currentUser.UserName,
                Roles = await UserManager.GetRolesAsync(currentUser)
            };

            userModel.FullName = userModel.GetShortNameForm();

            return Ok(userModel);
        }

        protected string GeneratePassword()
        {
            string password = "";

            Random random = new Random();

            for (int i = 1; i <= 5; i++)
            {
                password += random.Next(0, 9);
            }

            return password;
        }

        protected string GenerateUsername(string firstName, string lastName)
        {
            #region traslit
            Dictionary<string, string> letters = new Dictionary<string, string>();
            letters.Add("а", "a");
            letters.Add("б", "b");
            letters.Add("в", "v");
            letters.Add("г", "g");
            letters.Add("д", "d");
            letters.Add("е", "e");
            letters.Add("ё", "yo");
            letters.Add("ж", "zh");
            letters.Add("з", "z");
            letters.Add("и", "i");
            letters.Add("й", "j");
            letters.Add("к", "k");
            letters.Add("л", "l");
            letters.Add("м", "m");
            letters.Add("н", "n");
            letters.Add("о", "o");
            letters.Add("п", "p");
            letters.Add("р", "r");
            letters.Add("с", "s");
            letters.Add("т", "t");
            letters.Add("у", "u");
            letters.Add("ф", "f");
            letters.Add("х", "h");
            letters.Add("ц", "c");
            letters.Add("ч", "ch");
            letters.Add("ш", "sh");
            letters.Add("щ", "sch");
            letters.Add("ъ", "j");
            letters.Add("ы", "i");
            letters.Add("ь", "j");
            letters.Add("э", "e");
            letters.Add("ю", "yu");
            letters.Add("я", "ya");
            #endregion

            firstName = firstName.ToLower();
            lastName = lastName.ToLower();

            foreach (var pair in letters)
            {
                firstName = firstName.Replace(pair.Key, pair.Value);
                lastName = lastName.Replace(pair.Key, pair.Value);
            }

            Random random = new Random();

            return firstName[0].ToString() + lastName[0] + lastName[1] + lastName[2] + random.Next(1, 9);
        }

    }
}