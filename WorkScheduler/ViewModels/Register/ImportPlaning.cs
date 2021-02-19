using System;
namespace WorkScheduler.ViewModels.Register
{
    public class ImportPlaning
    {
        public string DateRange { get; set; }
        public string NameRange { get; set; }
        public string HoursRange { get; set; }
        public string CommentRange { get; set; }

        public int AcademicYearId { get; set; }
        public int AssociationId { get; set; }
        public int GroupId { get; set; }
    }
}
