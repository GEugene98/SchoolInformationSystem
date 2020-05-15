using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler.ViewModels
{
    public class UserViewModel : DictionaryViewModel<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }

        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Activity { get; set; }
    }

    public static class UserModelHelper
    {
        public static string GetShortNameForm(this UserViewModel userModel)
        {
            return $"{userModel.LastName} {userModel.FirstName[0]}.{userModel.SurName[0]}.";
        }
    }
}
