using ProccessApp.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProccessApp
{
    [ImplementPropertyChanged]
    public class ProcessesControl
    {
        public int Count { get; set; }
        public ListBoxItem SelectedProcess { get; set; }
        public ObservableCollection<ListBoxItem> Processes { get; set; } = new ObservableCollection<ListBoxItem>();
        public ProcessesControl()
        {
            Task.Factory.StartNew(ObserveProcesses);
        }

        private async void ObserveProcesses()
        {
            while (true)
            {
                var listProcesses = Process.GetProcesses();
                Count = listProcesses.Count();
                var listData = listProcesses.Select(process => new ListBoxItem { Name = process.ProcessName }).ToList();
                if (!CheckEqual(listData))
                    Processes = new ObservableCollection<ListBoxItem>(listData);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public bool CheckEqual(IList<ListBoxItem> list)
        {
            return list.Count == Processes.Count;
        }

        public ICommand CloseCommand => new CommandHandler(CloseProcess);

        private void CloseProcess()
        {
            if (SelectedProcess == null)
                return;
            var listProcesses = Process.GetProcessesByName(SelectedProcess.Name);
            listProcesses.ToList().ForEach(process => process.Kill());
        }
        private void CloseProcessByName(string processName)
        {
            if (SelectedProcess == null)
                return;
            var listProcesses = Process.GetProcessesByName(processName);
            listProcesses.ToList().ForEach(process => process.Kill());
        }
    }
}