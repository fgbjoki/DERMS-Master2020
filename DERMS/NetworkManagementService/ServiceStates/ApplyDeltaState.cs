using System.Collections.Generic;
using Common.AbstractModel;

namespace NetworkManagementService.ServiceStates
{
    class ApplyDeltaState : ServiceState
    {
        internal ApplyDeltaState() : base(ServiceStateEnum.ApplyDelta)
        {
        }

        public override ServiceState ApplyDelta(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            return this;
        }

        public override ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            temporaryModel = null;

            return new IdleState();
        }

        public override ServiceState Prepare(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            currentWorkingModel = temporaryModel;

            return new PrepareState();
        }

        public override ServiceState Rollback(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            return new RollbackState().Rollback(ref currentModel, ref currentWorkingModel, ref temporaryModel);
        }
    }
}
