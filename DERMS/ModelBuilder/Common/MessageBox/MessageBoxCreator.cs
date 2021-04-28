using ClientUI.Views.MessageBox;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Common.MessageBox
{
    public class MessageBoxCreator
    {
        public static void Show(string title, string text, PackIconKind packIconKind)
        {
            MessageBoxView messageBoxView = new MessageBoxView()
            {
                DataContext = new MessageBoxViewModel(title, text, packIconKind)
            };

            Show(messageBoxView);
        }

        private static async void Show(MessageBoxView view)
        {
            var result = await DialogHost.Show(view, "RootDialog");
        }
    }
}
