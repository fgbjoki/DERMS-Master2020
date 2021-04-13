using FieldSimulator.PowerSimulator.Model.Connectivity;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class ConnectivityNodeCreator : BaseCreator<DERMS.ConnectivityNode, ConnectivityNode>
    {
        public ConnectivityNodeCreator(ImportHelper importHelper) : base(DMSType.CONNECTIVITYNODE, importHelper)
        {
        }

        protected override ConnectivityNode InstantiateNewEntity(long globalId)
        {
            return new ConnectivityNode(globalId);
        }
    }
}
