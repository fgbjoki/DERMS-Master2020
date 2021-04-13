using CIM.Model;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public abstract class BaseCreator<DependentType, NewType> : IEntityCreator
        where DependentType : DERMS.IdentifiedObject
        where NewType : IdentifiedObject
    {
        protected ImportHelper importHelper;
        protected DMSType dmsType;

        public BaseCreator(DMSType dmsType, ImportHelper importHelper)
        {
            this.dmsType = dmsType;
            this.importHelper = importHelper;
        }

        public void CreateNewEntities(ConcreteModel concreteModel, EntityStorage entityStorage)
        {
            Dictionary<long, IdentifiedObject> newObjects = new Dictionary<long, IdentifiedObject>();
            entityStorage.Storage[DMSType] = newObjects;

            foreach (var concrete in concreteModel.GetAllObjectsOfType(typeof(DependentType).FullName).Values)
            {
                NewType newEntity = InstantiateNewEntity(GetNewGid());
                newEntity.Update(concrete as DependentType);

                newObjects.Add(newEntity.GlobalId, newEntity);
            }
        }

        protected abstract NewType InstantiateNewEntity(long globalId);

        protected virtual void AddObjectReferences(EntityStorage entityStorage)
        {

        }

        protected long GetNewGid()
        {
            return ModelCodeHelper.CreateGlobalId(0, (short)DMSType, importHelper.CheckOutIndexForDMSType(DMSType));
        }

        public DMSType DMSType { get { return dmsType; } }
    }
}
