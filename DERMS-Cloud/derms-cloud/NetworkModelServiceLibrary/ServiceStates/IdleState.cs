using System.Collections.Generic;
using Core.Common.AbstractModel;

namespace NetworkManagementService.ServiceStates
{
    internal sealed class IdleState : ServiceState
    {
        public IdleState() : base(ServiceStateEnum.Idle)
        {
        }

        public override ServiceState ApplyDelta(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            currentWorkingModel = currentModel;

            temporaryModel = new Dictionary<DMSType, Container>();
            foreach (KeyValuePair<DMSType, Container> containerPair in currentModel)
            {
                temporaryModel.Add(containerPair.Key, containerPair.Value.Clone());
            }

            return new ApplyDeltaState();
        }

        public override ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            return this;
        }
    }
}
