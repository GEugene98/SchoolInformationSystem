using System;
namespace WorkScheduler.ViewModels.Register
{
    public class PlaningRecordViewModel: DictionaryViewModel<long>
    {
        public DateTime? Date { get; set; }
        public string Hours { get; set; }
        public string Comment { get; set; }

        public AssociationViewModel Association { get; set; }
        public GroupViewModel Group { get; set; }
        public AcademicYearViewModel AcademicYear { get; set; }
    }
}
