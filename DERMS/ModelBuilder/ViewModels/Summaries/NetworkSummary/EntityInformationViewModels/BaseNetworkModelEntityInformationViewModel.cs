using ClientUI.Models;
using ClientUI.Models.NetworkModel;
using Common.Communication;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.NetworkModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public abstract class BaseNetworkModelEntityInformationViewModel : BaseViewModel
    {
        private long globalId;

        private string name;
        private string description;

        private Visibility visibility;

        public virtual void PopulateFields(NetworkModelEntityDTO entity)
        {
            GlobalId = entity.GlobalId;
            Name = entity.Name;
            Description = entity.Description;
        }

        public long GlobalId
        {
            get { return globalId; }
            set
            {
                if (globalId != value)
                {
                    SetProperty(ref globalId, value);
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    SetProperty(ref name, value);
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    SetProperty(ref description, value);
                }
            }
        }

        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                if (visibility != value)
                {
                    SetProperty(ref visibility, value);
                }
            }
        }
    }
}
