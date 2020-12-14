using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class ConnectivityNodeContainer : PowerSystemResource
    {
        public ConnectivityNodeContainer(long globalId) : base(globalId)
        {

        }

        protected ConnectivityNodeContainer(ConnectivityNodeContainer copyObject) : base(copyObject)
        {
            ConnectivityNodes = copyObject.ConnectivityNodes;
        }


        public List<long> ConnectivityNodes { get; set; } = new List<long>();

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.CONNECTIVIRYNODECONTAINER_CONNECTIVITYNODES:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONNECTIVIRYNODECONTAINER_CONNECTIVITYNODES:
                    property.SetValue(ConnectivityNodes);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return ConnectivityNodes?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                    ConnectivityNodes.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (ConnectivityNodes?.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONNECTIVIRYNODECONTAINER_CONNECTIVITYNODES] = new List<long>(ConnectivityNodes);
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER:
                    if (!ConnectivityNodes.Remove(globalId))
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
            ConnectivityNodeContainer compareObject = x as ConnectivityNodeContainer;

            return compareObject != null && CompareHelper.CompareLists(ConnectivityNodes, compareObject.ConnectivityNodes) && base.Equals(x);
        }

        public override object Clone()
        {
            return new ConnectivityNodeContainer(this);
        }
    }
}
