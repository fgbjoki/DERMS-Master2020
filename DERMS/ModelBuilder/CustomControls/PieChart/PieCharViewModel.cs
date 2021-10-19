using ClientUI.ViewModels;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CustomControls.PieChart
{
    public class PieCharViewModel : BaseViewModel
    {
        public PieCharViewModel()
        {
            
        }

        public SeriesCollection SeriesCollection { get; set; }

        public virtual string CreateLabel(ChartPoint point)
        {
            return point.Y.ToString();
        }

    }
}
