using ClientUI.Events.ContentMenu;
using ClientUI.Events.Schema;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.SchemaNodeLocator
{
    public class SchemaNodeLocator
    {
        private ISchemaNodeLocator schemaNodeLocator;

        public SchemaNodeLocator(ISchemaNodeLocator schemaNodeLocator)
        {
            this.schemaNodeLocator = schemaNodeLocator;
            SummaryManager.Instance.EventAggregator.GetEvent<SchemaNodeLocateEvent>().Subscribe(LocateSchemaNode);
        }

        protected virtual void LocateSchemaNode(SchemaNodeLocateEventArgs args)
        {
            SummaryManager.Instance.EventAggregator.GetEvent<OpenSummaryEvent>().Publish(new OpenSummaryEvetnArgs() { ContentType = ContentType.BrowseSchema });
            schemaNodeLocator.FindNode(args.LocatingEntityGid);
        }
    }
}
