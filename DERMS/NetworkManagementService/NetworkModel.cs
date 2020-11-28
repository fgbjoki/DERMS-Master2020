using Common.AbstractModel;
using Common.GDA;
using System;
using System.Collections.Generic;

namespace NetworkManagementService
{
    public class NetworkModel : IDisposable
    {
        /// <summary>
		/// Dictionaru which contains all data: Key - DMSType, Value - Container
		/// </summary>
		private Dictionary<DMSType, Container> networkDataModel;

        /// <summary>
        /// ModelResourceDesc class contains metadata of the model
        /// </summary>
        private ModelResourcesDesc resourcesDescs;

        /// <summary>
        /// Initializes a new instance of the Model class.
        /// </summary>
        public NetworkModel()
        {
            networkDataModel = new Dictionary<DMSType, Container>();
            resourcesDescs = new ModelResourcesDesc();
            Initialize();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            List<Delta> result = ReadDeltasFromDB();

            foreach (Delta delta in result)
            {
                try
                {
                    //foreach (ResourceDescription rd in delta.InsertOperations)
                    //{
                    //    InsertEntity(rd);
                    //}

                    //foreach (ResourceDescription rd in delta.UpdateOperations)
                    //{
                    //    UpdateEntity(rd);
                    //}

                    //foreach (ResourceDescription rd in delta.DeleteOperations)
                    //{
                    //    DeleteEntity(rd);
                    //}
                }
                catch (Exception ex)
                {
                    //CommonTrace.WriteTrace(CommonTrace.TraceError, "Error while applying delta (id = {0}) during service initialization. {1}", delta.Id, ex.Message);
                }
            }
        }

        private List<Delta> ReadDeltasFromDB()
        {
            // TODO QUERY FROM DB
            return new List<Delta>(0);
        }
    }
}
