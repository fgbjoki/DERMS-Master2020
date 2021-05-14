using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientUI.Views
{
    /// <summary>
    /// Interaction logic for SchemaView.xaml
    /// </summary>
    public partial class SchemaView : UserControl
    {
        public SchemaView()
        {
            InitializeComponent();

            PreviewMouseWheel += SchemaView_PreviewMouseWheel;
        }

        private void SchemaView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            double deltaScale = (e.Delta * 0.0005);

            if (deltaScale != 0 && deltaScale + schemaScale.ScaleX > 0)
            {
                schemaScale.ScaleX += deltaScale;
                schemaScale.ScaleY += deltaScale;
            }
        }
    }
}
