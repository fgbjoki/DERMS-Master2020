using Core.Common.AbstractModel;
using Core.Common.GDA;
using Common.ServiceInterfaces;
using System.Collections.Generic;
using System.ServiceModel;
using System;

namespace FTN.ESI.SIMES.CIM.CIMAdapter
{
	public class NetworkModelGDAProxy : ClientBase<INetworkModelGDAContract>, INetworkModelGDAContract
	{
		public NetworkModelGDAProxy(string endpointName)
			: base(endpointName)
		{
		}

		public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
		{
			return Channel.GetValues(resourceId, propIds);
		}

		public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
		{
			return Channel.GetExtentValues(entityType, propIds); 
		}	

		
		public bool IteratorClose(int id)
		{
			return Channel.IteratorClose(id);
		}

		public List<ResourceDescription> IteratorNext(int n, int id)
		{
			return Channel.IteratorNext(n, id);
		}

		public int IteratorResourcesLeft(int id)
		{
			return Channel.IteratorResourcesLeft(id);
		}

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            return Channel.GetExtentValues(entityType, propIds, gids);
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds)
        {
            throw new NotImplementedException();
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds, List<long> gids)
        {
            throw new NotImplementedException();
        }
    }
}
