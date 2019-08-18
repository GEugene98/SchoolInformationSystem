namespace WorkScheduler.ViewModels
{
    public class FileViewModel : DictionaryViewModel<long>
    {
        public double SizeMb { get; set; }
        public string Extension { get; set; }
    }
}