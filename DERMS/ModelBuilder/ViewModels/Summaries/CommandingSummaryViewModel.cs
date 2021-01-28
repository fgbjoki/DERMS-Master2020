using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using ClientUI.Events.OpenCommandingWindow;

namespace ClientUI.ViewModels.Summaries
{
    public abstract class CommandingSummaryViewModel<T, TEventType, TEventArgType> : SummaryViewModel<T>
        where TEventType : OpenCommandingWindowEvent<TEventArgType>, new()
    {
        public CommandingSummaryViewModel(string summaryName, ContentType contentType) : base(summaryName, contentType)
        {
        }

        protected abstract TEventArgType TransformGridDataToEventArg(T selectedItem);

        protected void RaiseEventForCommandingWindow(T selectedItem)
        {
            TEventArgType args = TransformGridDataToEventArg(selectedItem);
            SummaryManager.Instance.EventAggregator.GetEvent<TEventType>().Publish(args);
        }
    }
}
