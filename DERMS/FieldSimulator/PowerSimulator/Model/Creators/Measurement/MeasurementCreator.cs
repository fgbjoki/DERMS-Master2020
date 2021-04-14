using FieldSimulator.PowerSimulator.Model.Measurements;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;
using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public abstract class MeasurementCreator<CIMType, NewType> : BaseCreator<CIMType, NewType>
        where CIMType : DERMS.Measurement
        where NewType : Measurement
    {
        public MeasurementCreator(DMSType dmsType, ImportHelper importHelper) : base(dmsType, importHelper)
        {
        }

        protected override void AddObjectReferences(NewType newEntity, EntityStorage entityStorage)
        {
            base.AddObjectReferences(newEntity, entityStorage);

            long conductingEquipmentGid = importHelper.GetMappedGID(newEntity.ConductingEquipmentID);

            DMSType conductingEquipmentType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(conductingEquipmentGid);

            ConductingEquipment conductingEquipment = entityStorage.Storage[conductingEquipmentType][conductingEquipmentGid] as ConductingEquipment;

            conductingEquipment.Measurements.Add(newEntity);

            newEntity.ConductingEquipment = conductingEquipment;
        }
    }
}
