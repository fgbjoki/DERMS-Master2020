﻿using Common.AbstractModel;
using Common.GDA;
using System;
using System.Collections.Generic;

namespace NetworkManagementService.DataModel.Core
{
    public enum TypeOfReference : short
    {
        Reference = 1,
        Target = 2,
        Both = 3,
    }

    public class IdentifiedObject : ICloneable
    {
        /// <summary>
        /// Model Resources Description
        /// </summary>
        private static ModelResourcesDesc resourcesDescs = new ModelResourcesDesc();

        /// <summary>
        /// Global id of the identified object (SystemId - 4 nibls, DMSType - 4 nibls, FragmentId - 8 nibls)
        /// </summary>
        private long globalId;

        /// <summary>
        /// Name of identified object
        /// </summary>		
        private string name = string.Empty;

        /// <summary>
        /// Mrid (source) id of identified object
        /// </summary>		
        private string mrid = string.Empty;

        /// <summary>
        /// Description of identified object
        /// </summary>		
        private string description = string.Empty;

        /// <summary>
        /// Initializes a new instance of the IdentifiedObject class.
        /// </summary>		
        /// <param name="globalId">Global id of the entity.</param>
        public IdentifiedObject(long globalId)
        {
            this.globalId = globalId;
        }

        /// <summary>
        /// Copy constructor for <see cref="ICloneable"/>.
        /// <remark>This will be used for Shallow Copy durning model promotion.</remark>
        /// </summary>
        /// <param name="identifiedObject"><see cref="IdentifiedObject"/> to copy.</param>
        protected IdentifiedObject(IdentifiedObject identifiedObject)
        {
            Mrid = identifiedObject.mrid;
            Name = identifiedObject.name;
            Description = identifiedObject.description;
        }

        /// <summary>
        /// Gets or sets global id of the entity (identified object).
        /// </summary>			
        public long GlobalId
        {
            get
            {
                return globalId;
            }

            set
            {
                globalId = value;
            }
        }

        /// <summary>
        /// Gets or sets name of the entity (identified object).
        /// </summary>			
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets mrid of the entity (identified object).
        /// </summary>			
        public string Mrid
        {
            get { return mrid; }
            set { mrid = value; }
        }

        /// <summary>
        /// Gets or sets description of the entity (identified object).
        /// </summary>			
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public static bool operator ==(IdentifiedObject x, IdentifiedObject y)
        {
            if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
            {
                return true;
            }
            else if ((Object.ReferenceEquals(x, null) && !Object.ReferenceEquals(y, null)) || (!Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null)))
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }

        public static bool operator !=(IdentifiedObject x, IdentifiedObject y)
        {
            return !(x == y);
        }

        public override bool Equals(object x)
        {
            if (Object.ReferenceEquals(x, null))
            {
                return false;
            }
            else
            {
                IdentifiedObject io = (IdentifiedObject)x;
                return ((io.GlobalId == this.GlobalId) && (io.name == this.name) && (io.mrid == this.mrid) &&
                        (io.description == this.description));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation		

        public virtual bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.IDOBJ_GID:
                case ModelCode.IDOBJ_NAME:
                case ModelCode.IDOBJ_DESCRIPTION:
                case ModelCode.IDOBJ_MRID:
                    return true;

                default:
                    return false;
            }
        }

        public virtual void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.IDOBJ_GID:
                    property.SetValue(globalId);
                    break;

                case ModelCode.IDOBJ_NAME:
                    property.SetValue(name);
                    break;

                case ModelCode.IDOBJ_MRID:
                    property.SetValue(mrid);
                    break;

                case ModelCode.IDOBJ_DESCRIPTION:
                    property.SetValue(description);
                    break;

                default:
                    string message = string.Format("Unknown property id = {0} for entity (GID = 0x{1:x16}).", property.Id.ToString(), this.GlobalId);
                    
                    // TODO LOG

                    throw new Exception(message);
            }
        }

        public virtual void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.IDOBJ_NAME:
                    name = property.AsString();
                    break;

                case ModelCode.IDOBJ_DESCRIPTION:
                    description = property.AsString();
                    break;

                case ModelCode.IDOBJ_MRID:
                    mrid = property.AsString();
                    break;

                default:
                    string message = string.Format("Unknown property id = {0} for entity (GID = 0x{1:x16}).", property.Id.ToString(), this.GlobalId);
                    
                    // TODO LOG

                    throw new Exception(message);
            }
        }

        #endregion IAccess implementation

        #region IReference implementation	

        public virtual bool IsReferenced
        {
            get
            {
                return false;
            }
        }

        public virtual void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            return;
        }

        public virtual void AddReference(ModelCode referenceId, long globalId)
        {
            string message = string.Format("Can not add reference {0} to entity (GID = 0x{1:x16}).", referenceId, this.GlobalId);
            // TODO LOG
            throw new Exception(message);
        }

        public virtual void RemoveReference(ModelCode referenceId, long globalId)
        {
            string message = string.Format("Can not remove reference {0} from entity (GID = 0x{1:x16}).", referenceId, this.GlobalId);
            // TODO LOG
            throw new ModelException(message);
        }

        #endregion IReference implementation

        #region utility methods

        public void GetReferences(Dictionary<ModelCode, List<long>> references)
        {
            GetReferences(references, TypeOfReference.Target | TypeOfReference.Reference);
        }

        public ResourceDescription GetAsResourceDescription(bool onlySettableAttributes)
        {
            ResourceDescription rd = new ResourceDescription(globalId);
            List<ModelCode> props = new List<ModelCode>();

            if (onlySettableAttributes == true)
            {
                props = resourcesDescs.GetAllSettablePropertyIdsForEntityId(globalId);
            }
            else
            {
                props = resourcesDescs.GetAllPropertyIdsForEntityId(globalId);
            }

            return rd;
        }

        public ResourceDescription GetAsResourceDescription(List<ModelCode> propIds)
        {
            ResourceDescription rd = new ResourceDescription(globalId);

            for (int i = 0; i < propIds.Count; i++)
            {
                rd.AddProperty(GetProperty(propIds[i]));
            }

            return rd;
        }

        public virtual Property GetProperty(ModelCode propId)
        {
            Property property = new Property(propId);
            GetProperty(property);
            return property;
        }

        public virtual object Clone()
        {
            return new IdentifiedObject(this);
        }

        #endregion utility methods
    }

}
