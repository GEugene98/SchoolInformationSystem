using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DictionaryController : Controller
    {
        protected Context Db;
        protected UserManager<User> UserManager;

        public DictionaryController(Context context, UserManager<User> userManager)
        {
            Db = context;
            UserManager = userManager;
        }

        [HttpGet("AcademicYears")]
        public IActionResult AcademicYears()
        {
            var academicYears = Db.AcademicYears.Select(a => new AcademicYearViewModel
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToList();

            return Ok(academicYears);
        }

        [HttpGet("ConfirmationForms")]
        public IActionResult ConfirmationForms()
        {
            var confForms = Db.ConfirmationForms.Select(c => new ConfirmationFormViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

            return Ok(confForms);
        }

        [HttpGet("Responsibles")]
        public async Task<IActionResult> Responsibles()
        {
            var responsibles = new List<UserViewModel>();

            foreach (var user in Db.Users)
            {
                var userRoles = (await UserManager.GetRolesAsync(user)).Where(r => r != "");

                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SurName = user.SurName,
                    Roles = userRoles
                };

                userModel.FullName = userModel.GetShortNameForm();

                responsibles.Add(userModel);
            }

            return Ok(responsibles
                .Where(r => r.LastName != "Тест")
                .OrderBy(r => r.LastName));
        }

        [HttpGet("Roles")]
        public IActionResult Roles()
        {
            var roles = Db.Roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            });

            return Ok(roles);
        }

        [HttpGet("Activities")]
        public IActionResult Activities()
        {
            var activities = Db.Activities.Select(a => new ActivityViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Color = a.Color
            })
            .ToList();

            return Ok(activities);
        }

        [HttpGet("AddActivity")]
        public IActionResult AddActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity != null)
            {
                return BadRequest("Такое направление деятельности уже существует");
            }

            Db.Activities.Add(new Activity
            {
                Id = activity.Id,
                Name = activity.Name,
                Color = activity.Color
            });

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("EditActivity")]
        public IActionResult EditActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity == null)
            {
                return BadRequest("Такого направления деятельности не существует или оно было удалено");
            }

            existingActivity.Name = activity.Name;
            existingActivity.Color = activity.Color;

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("DeleteActivity")]
        public IActionResult DeleteActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity == null)
            {
                return BadRequest("Такого направления деятельности не существует или оно было удалено");
            }

            if (Db.WorkSchedules.FirstOrDefault(ws => ws.ActivityId == existingActivity.Id) != null)
            {
                return BadRequest("Существуют планы, которые ссылаются на выбранное направление деятельности. Удаление невозможно.");
            }

            Db.Activities.Remove(existingActivity);

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("Users")]
        async public Task<IActionResult> Users()
        {
            var users = Db.Users.Where(u => !u.LockoutEnabled).ToList();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {

                var roles = await UserManager.GetRolesAsync(user);

                var u = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SurName = user.SurName,
                    Name = user.UserName,
                    Roles = roles
                };

                userViewModels.Add(u);
            }

            return Ok(userViewModels);
        }
    }
}