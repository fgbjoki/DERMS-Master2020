using Common.UIDataTransferObject.Schema;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface ISchema
    {
        [OperationContract]
        SubSchemaDTO GetSchema(long energySourceId);

        [OperationContract]
        SubSchemaConductingEquipmentEnergized GetEquipmentStates(long energySourceId);

        [OperationContract]
        List<EnergySourceDTO> GetSubstations();

        [OperationContract]
        SchemaEnergyBalanceDTO GetEnergyBalance(long energySourceGid);

        [OperationContract]
        long SubStationContainsEntity(long entityGid);
    }
}
