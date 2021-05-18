using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using Common.UIDataTransferObject.NetworkModel;

namespace UIAdapter.NetworkModel
{
    public interface IDTOCreator
    {
        List<ModelCode> NeededModelCodes { get; }

        NetworkModelEntityDTO CreateEntityDTO(ResourceDescription rd);

        void ConnectDependentDTO(NetworkModelEntityDTO entity, NetworkModelEntityDTO depedencyDTO);

        List<long> GetDependentEntities(ResourceDescription rd);
    }
}