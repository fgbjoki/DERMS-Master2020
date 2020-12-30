using Common.GDA;

namespace NetworkManagementService.Components
{
    internal interface IDeltaProcessor
    {
        /// <summary>
        /// Applies given delta on the current model.
        /// </summary>
        /// <param name="delta">Delat to apply.</param>
        /// <param name="isInitializing">Defines is the service initializing (reading deltas from database) or is the apply delta called from another service.</param>
        /// <returns></returns>
        UpdateResult ApplyDelta(Delta delta, bool isInitializing = false);
    }
}