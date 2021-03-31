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

        public SchemaNode(long globalId, string imageSource) : base (50, 50)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            Children = new ObservableCollection<SchemaNode>();

            this.imageSource = imageSource;
        }

        public ObservableCollection<SchemaNode> Children { get; set; }

        public long GlobalId { get; private set; }

        public DMSType DMSType { get; set; }

        public bool Energized
        {
            get { return isEnergized; }
            set { SetProperty(ref isEnergized, value); }
        }

        public bool DoesConduct
        {
            get { return doesConduct; }
            set
            {
                SetProperty(ref doesConduct, value);
                Outline = doesConduct ? green : red;
            }
        }

        public string ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public ICommand OnDoubleClick { get; set; }

        private void Control1_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            OnDoubleClick.Execute(null);
        }
    }
}
