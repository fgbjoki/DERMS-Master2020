using System;
using System.Windows;
using System.Windows.Input;
using ClientUI.Views.CommandingWindow;

namespace ClientUI.CustomControls
{
    public class CommandWindow : Window
    {
        public long GlobalId { get; set; }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
