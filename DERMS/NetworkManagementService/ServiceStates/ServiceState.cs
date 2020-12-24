using Common.AbstractModel;
using System;
using System.Collections.Generic;

namespace NetworkManagementService.ServiceStates
{
    internal enum ServiceStateEnum
    {
        Idle,
        ApplyDelta,
        Prepare,
        Commit,
        Rollback
    }

    internal abstract class ServiceState
    {
        protected ServiceState(ServiceStateEnum serviceState)
        {
            CurrentState = serviceState;
        }

        public ServiceStateEnum CurrentState { get; private set; }

        public abstract ServiceState ChangeToIdleState(ref Dictionary<DMSType, Container> currentModel,
                                                       ref Dictionary<DMSType, Container> currentWorkingModel,
                                                       ref Dictionary<DMSType, Container> temporaryModel);

        public virtual ServiceState ApplyDelta(ref Dictionary<DMSType, Container> currentModel,
                                                ref Dictionary<DMSType, Container> currentWorkingModel,
                                                ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.ApplyDelta.ToString()}.");
        }

        public virtual ServiceState Prepare(ref Dictionary<DMSType, Container> currentModel,
                                             ref Dictionary<DMSType, Container> currentWorkingModel,
                                             ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.Prepare.ToString()}.");
        }

        public virtual ServiceState Rollback(ref Dictionary<DMSType, Container> currentModel,
                                              ref Dictionary<DMSType, Container> currentWorkingModel,
                                              ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.Rollback.ToString()}.");
        }

        public virtual ServiceState Commit(ref Dictionary<DMSType, Container> currentModel,
                                            ref Dictionary<DMSType, Container> currentWorkingModel,
                                            ref Dictionary<DMSType, Container> temporaryModel)
        {
            throw new Exception($"Cannot change from {CurrentState.ToString()} to {ServiceStateEnum.Commit.ToString()}.");
        }
    }
}
