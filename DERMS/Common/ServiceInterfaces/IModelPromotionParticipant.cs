using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces
{
    /// <summary>
    /// Interface ment for component which participates in model promotion.
    /// </summary>
    [ServiceContract]
    public interface IModelPromotionParticipant
    {
        /// <summary>
        /// Defines GIDs of inserted, updater and deleted entities and validates given lists.
        /// </summary>
        /// <param name="insertedEntities">GIDs of inserted entities.</param>
        /// <param name="updatedEntities">GIDs of updated entities.</param>
        /// <param name="deletedEntities">GIDs of deleted entities.</param>
        /// <returns><b>True</b> if the upcoming model promotion has valid entities, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities);
    }
}
