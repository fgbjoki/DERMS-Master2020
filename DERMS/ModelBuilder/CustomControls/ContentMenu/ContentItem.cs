using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CustomControls
{
    public class ContentItem
    {
        public ContentItem(SummaryWrapper summaryInfo)
        {
            SummaryInfo = summaryInfo;
            Children = new ObservableCollection<ContentItem>();
        }

        public SummaryWrapper SummaryInfo { get; private set; }
        public ObservableCollection<ContentItem> Children { get; private set; }

        public void AddChild(ContentItem childItem)
        {
            Children.Add(childItem);
        }
    }
}
