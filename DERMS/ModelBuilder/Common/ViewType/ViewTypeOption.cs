using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Common.ViewType
{
    public enum ViewTypeEnum
    {
        Default,
        Grid,
        Tile
    }
    public class ViewTypeOption
    {
        public ViewTypeOption(string name, ViewTypeEnum viewType)
        {
            Name = name;
            ViewType = viewType;
        }

        public string Name { get; set; }
        public ViewTypeEnum ViewType { get; set; }
    }
}
