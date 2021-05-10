using ClientUI.Common;
using Common.AbstractModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Models.Schema
{
    public class SchemaNode : UINode
    {
        private bool isEnergized;
        private bool doesConduct;

        private string imageSource;

        private bool located;

        public SchemaNode(long globalId, string imageSource) : base (60, 60)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            Children = new ObservableCollection<SchemaNode>();

            this.imageSource = imageSource;
        }

        public ObservableCollection<SchemaNode> Children { get; set; }

        public long GlobalId { get; private set; }

        public DMSType DMSType { get; set; }

        public ObservableCollection<ContextAction> ContextActions { get; set; }

        public bool Energized
        {
            get { return isEnergized; }
            set
            {
                SetProperty(ref isEnergized, value);
                Outline = isEnergized ? green : blue;
            }
        }

        public bool DoesConduct
        {
            get { return doesConduct; }
            set { SetProperty(ref doesConduct, value); }
        }

        public string ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public ICommand OnDoubleClick { get; set; }

        public bool Located
        {
            get { return located; }
            set
            {
                if (located != value)
                {
                    SetProperty(ref located, value);
                }
            }
        }
    }
}
