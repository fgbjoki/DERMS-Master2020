using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Common.MessageBox
{
    public class MessageBoxViewModel : BindableBase
    {
        public MessageBoxViewModel(string text, string title, PackIconKind icon)
        {
            Text = text;
            Title = title;
            PackIcon = icon;
        }

        public string Text { get; private set; }
        public string Title { get; private set; }
        public PackIconKind PackIcon { get; private set; }
    }
}
