using Core.Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NetworkManagementService.ServiceStates
{
    [DataContract]
    public enum ServiceStateEnum
    {
        [EnumMember]
        Idle,
        [EnumMember]
        ApplyDelta,
        [EnumMember]
        Prepare,
        [EnumMember]
        Commit,
        [EnumMember]
        Rollback
    }

    [DataContract]
    [KnownType(typeof(IdleState))]
    [KnownType(typeof(RollbackState))]
    [KnownType(typeof(CommitState))]
    [KnownType(typeof(PrepareState))]
    [KnownType(typeof(ApplyDeltaState))]
    public abstract class ServiceState
    {
        public ServiceState(ServiceStateEnum serviceState)
        {
            CurrentState = serviceState;
        }

        [DataMember]
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
