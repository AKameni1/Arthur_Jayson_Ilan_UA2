using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Commands;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public class TabItemModel
    {
        public string Header { get; set; } = string.Empty;
        public string IconKind { get; set; } = string.Empty;
        public UserControl Content { get; set; } = new UserControl();
        public ICommand Command { get; set; } = new RelayCommand(o => { });
    }
}
