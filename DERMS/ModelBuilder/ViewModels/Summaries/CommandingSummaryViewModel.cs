using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using ClientUI.Events.OpenCommandingWindow;
using ClientUI.Models;

namespace ClientUI.ViewModels.Summaries
{
    public abstract class CommandingSummaryViewModel<TSummaryItem, TDTO, TEventType, TEventArgType> : SummaryViewModel<TSummaryItem>
        where TSummaryItem : IdentifiedObject
        where TEventType : OpenCommandingWindowEvent<TEventArgType>, new()
    {
        public CommandingSummaryViewModel(string summaryName, ContentType contentType) : base(summaryName, contentType)
        {
        }

        protected abstract TEventArgType TransformGridDataToEventArg(TSummaryItem selectedItem);

        protected void RaiseEventForCommandingWindow(TSummaryItem selectedItem)
        {
            TEventArgType args = TransformGridDataToEventArg(selectedItem);
            SummaryManager.Instance.EventAggregator.GetEvent<TEventType>().Publish(args);
        }
    }
}
