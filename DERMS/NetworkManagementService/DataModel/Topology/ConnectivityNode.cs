using NetworkManagementService.DataModel.Core;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Topology
{
    public class ConnectivityNode : IdentifiedObject
    {
        public ConnectivityNode(long globalId) : base(globalId)
        {

        }

        protected ConnectivityNode(ConnectivityNode copyObject) : base(copyObject)
        {
            ConnectivityNodeContainer = copyObject.ConnectivityNodeContainer;
            Terminals = copyObject.Terminals;
        }

        public long ConnectivityNodeContainer { get; set; }
        public List<long> Terminals { get; set; } = new List<long>(2);

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                case ModelCode.CONNECTIVITYNODE_TERMINALS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                    property.SetValue(ConnectivityNodeContainer);
                    break;
                case ModelCode.CONNECTIVITYNODE_TERMINALS:
                    property.SetValue(Terminals);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                    ConnectivityNodeContainer = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return Terminals?.Count > 0 || ConnectivityNodeContainer > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    Terminals.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Terminals != null && Terminals.Count > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Target))
            {
                references[ModelCode.CONNECTIVITYNODE_TERMINALS] = new List<long>(Terminals);
            }

            if (ConnectivityNodeContainer > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Reference))
            {
                references[ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER] = new List<long>(1) { ConnectivityNodeContainer };
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    if (!Terminals.Remove(globalId))
                    {
                        // LOG
                        //CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            ConnectivityNode compareObject = x as ConnectivityNode;

            return compareObject != null && CompareHelper.CompareLists(Terminals, compareObject.Terminals) && ConnectivityNodeContainer == compareObject.ConnectivityNodeContainer
                && base.Equals(x);
        }

        public override object Clone()
        {
            return new ConnectivityNode(this);
        }
    }
}
