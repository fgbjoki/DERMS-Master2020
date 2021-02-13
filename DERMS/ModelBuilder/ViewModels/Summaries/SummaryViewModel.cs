using ClientUI.CustomControls;
using ClientUI.Models;
using ClientUI.SummaryCreator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;

namespace ClientUI.ViewModels.Summaries
{
    public abstract class SummaryViewModel<T> : ContentViewModel
        where T : IdentifiedObject
    {
        private Timer timer;
        private T selectedItem;
        private GIDMappedObservableCollection<T> items;

        protected SummaryViewModel(string summaryName, ContentType contentType) : base(summaryName, contentType)
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Elapsed += FetchSummaryItems;
            timer.Interval = 1000 * 10; // 10 seconds;

            Items = new GIDMappedObservableCollection<T>();
        }

        public GIDMappedObservableCollection<T> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
            }
        }

        public T SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        public override void StartProcessing()
        {
            timer.Enabled = true;
        }

        public override void StopProcessing()
        {
            timer.Enabled = false;
        }

        protected void FetchSummaryItems(object sender, ElapsedEventArgs e)
        {
            List<T> entities = GetEntitiesFromService();

            if (entities?.Count == 0)
            {
                return;
            }

            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => UpdateEntities(entities)));         
        }

        protected abstract List<T> GetEntitiesFromService();

        private void UpdateEntities(List<T> entities)
        {
            foreach (var entity in entities)
            {
                Items.AddOrUpdateEntity(entity);
            }
        }
    }
}
