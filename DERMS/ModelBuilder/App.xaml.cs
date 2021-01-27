using Prism;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels;
using ClientUI.Views;
using Prism.Events;

namespace ModelBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplicationBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            throw new NotImplementedException();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IEventAggregator>(new EventAggregator());
        }
    }
}
