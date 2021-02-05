using System;
namespace WorkScheduler.ViewModels.Monitoring
{
    public class OrganizationViewModel : DictionaryViewModel<int>
    {
        public int SchoolId { get; set; }
        public DictionaryViewModel<int> School { get; set; }
    }
}
