using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Forecast.Production.ProductionSeriesManager
{
    public class DisplayTypeControl : BindableBase
    {
        private bool displayTotal;
        private bool displaySolar;
        private bool displayWind;
        private bool displayEntity;

        public DisplayTypeControl()
        {
            DisplayAll();
        }

        public bool DisplayTotal
        {
            get { return displayTotal; }
            set
            {
                if (displayTotal != value)
                {
                    SetProperty(ref displayTotal, value);
                }
            }
        }

        public bool DisplaySolar
        {
            get { return displaySolar; }
            set
            {
                if (displaySolar != value)
                {
                    SetProperty(ref displaySolar, value);
                }
            }
        }

        public bool DisplayWind
        {
            get { return displayWind; }
            set
            {
                if (displayWind != value)
                {
                    SetProperty(ref displayWind, value);
                }
            }
        }

        public bool DisplayEntity
        {
            get { return displayEntity; }
            set
            {
                if (displayEntity != value)
                {
                    SetProperty(ref displayEntity, value);
                }
            }
        }

        public void DisplayAll()
        {
            DisplayTotal = DisplaySolar = DisplayWind = true;
        }

        public void Display(PowerType powerType)
        {
            switch (powerType)
            {
                case PowerType.Total:
                    DisplayTotal = true;
                    break;
                case PowerType.Wind:
                    DisplayWind = true;
                    break;
                case PowerType.Solar:
                    DisplaySolar = true;
                    break;
                case PowerType.Entity:
                    DisplayEntity = true;
                    break;
                default:
                    break;
            }
        }

        public void HideAll()
        {
            DisplaySolar = DisplayTotal = DisplayWind = DisplayEntity= false;
        }
    }
}
