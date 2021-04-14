using FieldSimulator.PowerSimulator.Model.Connectivity;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;
using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class TerminalCreator : BaseCreator<DERMS.Terminal, Terminal>
    {
        public TerminalCreator(ImportHelper importHelper) : base(DMSType.TERMINAL, importHelper)
        {
        }

        protected override Terminal InstantiateNewEntity(long globalId)
        {
            return new Terminal(globalId);
        }

        protected override void AddObjectReferences(Terminal newEntity, EntityStorage entityStorage)
        {
            base.AddObjectReferences(newEntity, entityStorage);

            long connectivityNodeGid = importHelper.GetMappedGID(newEntity.ConnectivityNodeID);

            ConnectivityNode connectivityNode = entityStorage.Storage[DMSType.CONNECTIVITYNODE][connectivityNodeGid] as ConnectivityNode;
            newEntity.ConnectivityNode = connectivityNode;
            connectivityNode.Terminals.Add(newEntity);

            long conductingEquipmentGid = importHelper.GetMappedGID(newEntity.ConductingEquipmentID);
            DMSType conductingEquipmentType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(conductingEquipmentGid);

            ConductingEquipment conductingEquipment = entityStorage.Storage[conductingEquipmentType][conductingEquipmentGid] as ConductingEquipment;

            newEntity.ConductingEquipment = conductingEquipment;
            conductingEquipment.Terminals.Add(newEntity);
        }
    }
}
