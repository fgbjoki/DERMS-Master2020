using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientUI.Models.Schema
{
    public class UINode : BindableBase
    {
        public UINode(int width, int height)
        {
            Width = width;
            Height = height;

            Outline = new SolidColorBrush(Colors.White);
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public Brush Outline { get; set; }
    }
}
