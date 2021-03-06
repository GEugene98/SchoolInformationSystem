﻿using Microsoft.AspNetCore.Authorization;
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
            var roles = Context.Roles.Where(r => r.Name == model.Role);

            if (roles == null)
            {
                return BadRequest("Указанной роли пользователя не существует");
            }

            string username;
            User foundUser;

            Random rnd = new Random();

            do
            {
                username = GenerateUsername(model.FirstName, model.LastName) + rnd.Next(0, 10);
                foundUser = Context.Users.FirstOrDefault(u => u.UserName == username);
            }
            while (foundUser != null);

            var schoolId = Context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            User user = new User
            {
                UserName = username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SurName = model.SurName,
                SchoolId = schoolId
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

                if (await UserManager.IsInRoleAsync(user, "Администратор"))
                {
                    user.CanUseChecklists = user.CanConfirm = true;
                }

                if (await UserManager.IsInRoleAsync(user, "Директор"))
                {
                    user.CanSeeAllProtocols = user.CanSeeAllSchedules = user.CanSeeAllChecklists = user.CanUseChecklists = user.CanAccept = true;
                }

                Context.SaveChanges();

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
                    ViewBag.Message = "Не все обязательные поля были заполнены"; 
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
            return Redirect("/api/Account/Login");
        }

        [Authorize(Roles = "Директор")]
        [HttpGet("NewPassword")]
        public async Task<IActionResult> NewPassword(string userId)
        {
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);

            await UserManager.RemovePasswordAsync(user);

            var newPassword = GeneratePassword();

            await UserManager.AddPasswordAsync(user, newPassword);

            return Ok(newPassword);
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
                Roles = await UserManager.GetRolesAsync(currentUser),

                CanAccept = currentUser.CanAccept,
                CanConfirm = currentUser.CanConfirm,
                CanSeeAllChecklists = currentUser.CanSeeAllChecklists,
                CanSeeAllProtocols = currentUser.CanSeeAllProtocols,
                CanSeeAllSchedules = currentUser.CanSeeAllSchedules,
                CanUseChecklists = currentUser.CanUseChecklists
            };

            userModel.FullName = userModel.GetShortNameForm();

            return Ok(userModel);
        }

        [Authorize(Roles = "Директор,Администратор")]
        [HttpGet("SetPermission")]
        public IActionResult SetPermission(string userId, string permission, bool value)
        {
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);

            switch (permission)
            {
                case "CanAccept":
                    user.CanAccept = value;
                    break;
                case "CanConfirm":
                    user.CanConfirm = value;
                    break;
                case "CanSeeAllChecklists":
                    user.CanSeeAllChecklists = value;
                    break;
                case "CanSeeAllProtocols":
                    user.CanSeeAllProtocols = value;
                    break;
                case "CanSeeAllSchedules":
                    user.CanSeeAllSchedules = value;
                    break;
                case "CanUseChecklists":
                    user.CanUseChecklists = value;
                    break;
                default:
                    break;
            }

            Context.Users.Update(user);

            Context.SaveChanges();

            return Ok();
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

            return firstName[0].ToString() + lastName[0] + lastName[1] + lastName[2];
        }

    }
}