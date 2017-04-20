using PropertyChanged;

namespace ProccessApp.Model
{
    [ImplementPropertyChanged]
    public class ListBoxItem
    {
        public string Name { get; set; }
    }
}