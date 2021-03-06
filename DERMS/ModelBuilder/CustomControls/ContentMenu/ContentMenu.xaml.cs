﻿using System;
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

namespace ClientUI.CustomControls
{
    /// <summary>
    /// Interaction logic for ContentMenu.xaml
    /// </summary>
    public partial class ContentMenu : UserControl
    {
        public ContentMenu()
        {
            InitializeComponent();
            DataContext = new ContentMenuViewModel();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            TextBlockSummaries.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            TextBlockSummaries.Visibility = Visibility.Visible;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ContentMenuViewModel viewModel = DataContext as ContentMenuViewModel;
            if (viewModel == null)
            {
                return;
            }

            viewModel.SelectedItem = e.NewValue as ContentItem;
        }
    }
}
