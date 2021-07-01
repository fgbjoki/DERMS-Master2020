using Core.Common.AbstractModel;
using Core.Common.Communication.ServiceFabric.NMS;
using System;
using System.Collections.Generic;

namespace Core.Common.GDA
{
    public class GDAProxy
    {
        private static readonly int iteratorGetResources = 100;
        GDAWCFClient gdaProxy;

        public GDAProxy()
        {
            gdaProxy = new GDAWCFClient();
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            ResourceDescription resultRd = new ResourceDescription();
            try
            {
                resultRd = gdaProxy.GetValues(resourceId, propIds);
            }
            catch (Exception e)
            {
                //Logger.Logger.Instance.Log(e);
                return null;
            }

            return resultRd;
        }

        public List<ResourceDescription> GetExtentValues(ModelCode type, List<ModelCode> propertyIds)
        {
            List<ResourceDescription> resultRds = new List<ResourceDescription>();
            int iteratorId = 0;
            try
            {
                iteratorId = gdaProxy.GetExtentValues(type, propertyIds);

                if (iteratorId == 0)
                {
                    return resultRds;
                }

                while (gdaProxy.IteratorResourcesLeft(iteratorId) > 0)
                {
                    List<ResourceDescription> rds = gdaProxy.IteratorNext(iteratorGetResources, iteratorId);

                    if (rds.Count > 0)
                    {
                        resultRds.AddRange(rds);
                    }
                }
            }
            catch (Exception e)
            {
                //Logger.Logger.Instance.Log(e);
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
                iteratorId = gdaProxy.GetExtentValues(type, propertyIds, gids);

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
                iteratorId = gdaProxy.GetExtentValues(type, propertyIds, gids);

                IterateThroughEntities(iteratorId, resultRds);
            }
            catch (Exception e)
            {
                
            }

            return resultRds;
        }

        private void IterateThroughEntities(int iteratorId, List<ResourceDescription> resultRds)
        {
            if (iteratorId == 0)
            {
                return;
            }
            while (gdaProxy.IteratorResourcesLeft(iteratorId) > 0)
            {
                List<ResourceDescription> rds = gdaProxy.IteratorNext(iteratorGetResources, iteratorId);

                if (rds.Count > 0)
                {
                    resultRds.AddRange(rds);
                }
            }
        }
    }
}
