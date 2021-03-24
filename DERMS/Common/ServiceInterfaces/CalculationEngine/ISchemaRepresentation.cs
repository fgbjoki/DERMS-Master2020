using Common.DataTransferObjects.CalculationEngine;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface ISchemaRepresentation
    {
        [OperationContract]
        IEnumerable<long> GetSchemaSources();

        [OperationContract]
        SchemaGraphChanged GetSchema(long sourceId);
    }
}
