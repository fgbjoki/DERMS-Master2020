using Common.SCADA;

namespace NetworkDynamicsService.RemotePointProcessors
{
    interface IValueChangedProcessor
    {
        void ProcessChangedValue(RemotePointFieldValue fieldValue);
    }
}
