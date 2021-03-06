﻿using Prism.Mvvm;
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
        protected readonly SolidColorBrush blue = new SolidColorBrush(Colors.Blue);
        protected readonly SolidColorBrush green = new SolidColorBrush(Colors.Green);

        private SolidColorBrush outline;

        public UINode(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public virtual SolidColorBrush Outline
        {
            get { return outline; }
            set
            {
                SetProperty(ref outline, value);
            }
        }
    }
}
