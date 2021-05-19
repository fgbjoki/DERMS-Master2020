using Common.UIDataTransferObject.NetworkModel;

namespace UIAdapter.NetworkModel
{
    public interface IDTOContainer
    {
        NetworkModelEntityDTO CreateDTO(long entityGid);
    }
}