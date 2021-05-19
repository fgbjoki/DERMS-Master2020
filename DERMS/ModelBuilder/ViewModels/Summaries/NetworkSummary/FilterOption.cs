using Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Summaries.NetworkSummary
{
    public class FilterOption
    {
        public FilterOption(string name, DMSType dmsType)
        {
            Name = name;
            DMSType = dmsType;
        }

        public string Name { get; set; }
        public DMSType DMSType { get; set; }
    }
}
