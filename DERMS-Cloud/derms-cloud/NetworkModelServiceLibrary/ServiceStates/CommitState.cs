using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.AbstractModel;

namespace NetworkManagementService.ServiceStates
{
    [DataContract]
    public sealed class CommitState : ServiceState
    {
        public CommitState() : base(ServiceStateEnum.Commit)
        {
        }

        public override ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.Idle.ToString()}.");
        }

        public override ServiceState Commit(ref Dictionary<DMSType, Container> currentModel, ref Dictionary<DMSType, Container> currentWorkingModel, ref Dictionary<DMSType, Container> temporaryModel)
        {
            currentModel = temporaryModel;
            currentWorkingModel = currentModel;

            return new IdleState();
        }
    }
}
