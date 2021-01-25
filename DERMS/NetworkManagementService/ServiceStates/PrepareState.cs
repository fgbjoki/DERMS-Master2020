using System.Collections.Generic;
using Common.AbstractModel;

namespace NetworkManagementService.ServiceStates
{
    internal class PrepareState : ServiceState
    {
        internal PrepareState() : base(ServiceStateEnum.Prepare)
        {
        }

        public override ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            currentWorkingModel = currentModel;

            temporaryModel = null;

            return new IdleState();
        }

        public override ServiceState Commit(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            return new CommitState().Commit(ref currentModel, ref currentWorkingModel, ref temporaryModel);
        }

        public override ServiceState Rollback(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            return new RollbackState().Rollback(ref currentModel, ref currentWorkingModel, ref temporaryModel);
        }
    }
}
