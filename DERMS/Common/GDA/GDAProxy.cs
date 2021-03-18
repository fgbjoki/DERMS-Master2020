using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace Common.GDA
{
    public class GDAProxy
    {
        private static readonly int iteratorGetResources = 100;
        WCFClient<INetworkModelGDAContract> gdaProxy;

        public GDAProxy(string gdaEndpointName)
        {
            gdaProxy = new WCFClient<INetworkModelGDAContract>(gdaEndpointName);
        }

        public List<ResourceDescription> GetExtentValues(ModelCode type, List<ModelCode> propertyIds)
        {
            List<ResourceDescription> resultRds = new List<ResourceDescription>();
            int iteratorId = 0;
            try
            {
                iteratorId = gdaProxy.Proxy.GetExtentValues(type, propertyIds);

                if (iteratorId == 0)
                {
                    return resultRds;
                }

                while (gdaProxy.Proxy.IteratorResourcesLeft(iteratorId) > 0)
                {
                    List<ResourceDescription> rds = gdaProxy.Proxy.IteratorNext(iteratorGetResources, iteratorId);

                    if (rds.Count > 0)
                    {
                        resultRds.AddRange(rds);
                    }
                }
            }
            catch (Exception e)
            {
                Common.Logger.Logger.Instance.Log(e);
                return null;
            }

            return resultRds;
        }

        public List<ResourceDescription> GetExtentValues(ModelCode type, List<ModelCode> propertyIds, List<long> gids)
        {
            List<ResourceDescription> resultRds = new List<ResourceDescription>();
            int iteratorId = 0;
            try
            {
                iteratorId = gdaProxy.Proxy.GetExtentValues(type, propertyIds, gids);

                IterateThroughEntities(iteratorId, resultRds);
            }
            catch (Exception e)
            {
                return null;
            }

            return resultRds;
        }

        public List<ResourceDescription> GetExtentValues(DMSType type, List<ModelCode> propertyIds, List<long> gids)
        {
            List<ResourceDescription> resultRds = new List<ResourceDescription>();
            int iteratorId = 0;
            try
            {
                iteratorId = gdaProxy.Proxy.GetExtentValues(type, propertyIds, gids);

                IterateThroughEntities(iteratorId, resultRds);
            }
            catch (Exception e)
            {
                return null;
            }

            return resultRds;
        }

        public List<ResourceDescription> GetRelatedValues(long source, List<ModelCode> propIds, Association association)
        {
            List<ResourceDescription> resultRds = new List<ResourceDescription>();
            int iteratorId = 0;
            try
            {
                iteratorId = gdaProxy.Proxy.GetRelatedValues(source, propIds, association);

                IterateThroughEntities(iteratorId, resultRds);
            }
            catch (Exception e)
            {
                return null;
            }

            return resultRds;
        }

        private void IterateThroughEntities(int iteratorId, List<ResourceDescription> resultRds)
        {
            if (iteratorId == 0)
            {
                return;
            }

            try
            {
                while (gdaProxy.Proxy.IteratorResourcesLeft(iteratorId) > 0)
                {
                    List<ResourceDescription> rds = gdaProxy.Proxy.IteratorNext(iteratorGetResources, iteratorId);

                    if (rds.Count > 0)
                    {
                        resultRds.AddRange(rds);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
