using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.AbstractModel;

namespace NetworkManagementService.ServiceStates
{
    [DataContract]
    public sealed class RollbackState : ServiceState
    {
        public RollbackState() : base(ServiceStateEnum.Rollback)
        {
        }

        public override ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.Commit.ToString()}.");
        }

        public override ServiceState Rollback(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            currentWorkingModel = currentModel;
            temporaryModel = null;

            return new IdleState();
        }
    }
}
