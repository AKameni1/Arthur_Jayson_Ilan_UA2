using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class TabItemModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Header { get; set; } = string.Empty;
        public string IconKind { get; set; } = string.Empty;
        public UserControl Content { get; set; } = new UserControl();

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
