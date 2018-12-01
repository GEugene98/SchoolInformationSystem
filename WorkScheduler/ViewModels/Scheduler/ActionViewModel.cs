using System;
using System.Collections.Generic;
using System.Linq;
using WorkScheduler.Models.Enums;

namespace WorkScheduler.ViewModels
{
    public class ActionViewModel : DictionaryViewModel<int>
    {
        public DateTime Date { get; set; }
        public IEnumerable<UserViewModel> Responsibles { get; set; }
        public ActionStatus Status { get; set; }
        public ConfirmationFormViewModel ConfirmationForm { get; set; }
        public int ConfirmationFormId { get; set; }
        public ActivityViewModel Activity { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? EndDate { get; set; }

        public string ScheduleName { get; set; }
        public string AuthorName { get; set; }
        public string AcademicYearName { get; set; }
    }

    public static class ActionModelHelper
    {
        public static string GetResponsiblesShortNameForms(this ActionViewModel actionModel)
        {
            var result = "";

            foreach (var responsible in actionModel.Responsibles)
            {
                result += $"{responsible.GetShortNameForm()}, ";
            }

            return result.Substring(0, result.Count() - 2);
        }

        public static string GetLocalizatedStatus(this ActionViewModel action)
        {
            switch (action.Status)
            {
                case ActionStatus.New: return "Новая запись";
                case ActionStatus.NeedConfirm: return "На согласовании";
                case ActionStatus.Confirmed: return "Согласовано";
                case ActionStatus.Accepted: return "Утверждено";
                case ActionStatus.CanceledConfirming: return "Отклонено при согласовании";
                case ActionStatus.CanceledAccepting: return "Отклонено при утверждении";
            }

            return null;
        }
    }
}
