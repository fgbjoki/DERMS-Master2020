using Common.AbstractModel;
using System.Collections.Generic;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation
{
    enum CurrentTraverser
    {
        CONNECTIVITYNODE,
        CONDUCTINGEQUIPMENT
    }

    public class ConnectivityObjectExplorer : IConnectivityObjectTraverser
    {
        private IConnectivityObjectTraverser connectivityNodeTraverser;
        private IConnectivityObjectTraverser conductingEquipmentTraverser;

        private CurrentTraverser currentTraverser;

        private ModelResourcesDesc modelResourceDesc;

        public ConnectivityObjectExplorer(ModelResourcesDesc modelResourceDesc)
        {
            this.modelResourceDesc = modelResourceDesc;

            connectivityNodeTraverser = new ConnectivityNodeTraverser();
            conductingEquipmentTraverser = new ConductingEquipmentTraverser();

            currentTraverser = CurrentTraverser.CONDUCTINGEQUIPMENT;
        }

        public List<ConnectivityNodeTraverseWrapper> ExploreNeighbourObjects(ConnectivityNodeTraverseWrapper topologyObject)
        {
            IConnectivityObjectTraverser traverser = GetCoorespondingTraverser(topologyObject.TopologyObject.GlobalId);

            List<ConnectivityNodeTraverseWrapper> objects = traverser.ExploreNeighbourObjects(topologyObject);

            return objects;
        }

        private void SwapTraverser()
        {
            if (currentTraverser == CurrentTraverser.CONDUCTINGEQUIPMENT)
            {
                currentTraverser = CurrentTraverser.CONNECTIVITYNODE;
            }
            else
            {
                currentTraverser = CurrentTraverser.CONDUCTINGEQUIPMENT;
            }
        }

        private IConnectivityObjectTraverser GetCoorespondingTraverser(long topologyObjectGid)
        {
            ModelCode objectModelCode = modelResourceDesc.GetModelCodeFromId(topologyObjectGid);

            if (ModelResourcesDesc.InheritsFrom(ModelCode.CONDUCTINGEQ, objectModelCode))
            {
                return conductingEquipmentTraverser;
            }
            else
            {
                return connectivityNodeTraverser;
            }
        }
    }
}
