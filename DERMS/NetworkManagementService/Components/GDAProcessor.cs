using Common.ServiceInterfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using NetworkManagementService.DataModel.Core;
using NetworkManagementService.Components.GDA;

namespace NetworkManagementService.Components
{
    class GDAProcessor : INetworkModelGDAContract
    {
        private Dictionary<int, ResourceIterator> resourceItMap = new Dictionary<int, ResourceIterator>();
        private int resourceItId = 0;

        private IStorageComponent storageComponent;

        public GDAProcessor(IStorageComponent storageComponent)
        {
            this.storageComponent = storageComponent;
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Getting extent values for entity type = {0} .", entityType);
            int retVal = 0;
            ResourceIterator ri = null;

            try
            {
                List<long> globalIds = new List<long>();
                Dictionary<DMSType, List<ModelCode>> class2PropertyIDs = new Dictionary<DMSType, List<ModelCode>>();

                DMSType entityDmsType = ModelCodeHelper.GetTypeFromModelCode(entityType);

                globalIds = storageComponent.GetEntitiesIdByDMSType(entityDmsType);
                class2PropertyIDs.Add(entityDmsType, propIds);

                ri = new ResourceIterator(globalIds, class2PropertyIDs);

                retVal = AddIterator(ri);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Getting extent values for entity type = {0} succedded.", entityType);
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get extent values for entity type = {0}. {1}", entityType, ex.Message);
                throw new Exception(message);
            }

            return retVal;
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Getting extent values for entity type = {0} .", entityType);
            int retVal = 0;
            ResourceIterator ri = null;

            try
            {
                List<long> globalIds = new List<long>();
                Dictionary<DMSType, List<ModelCode>> class2PropertyIDs = new Dictionary<DMSType, List<ModelCode>>();

                DMSType entityDmsType = ModelCodeHelper.GetTypeFromModelCode(entityType);

                globalIds = storageComponent.GetEntitiesIdByDMSType(entityDmsType);
                globalIds = globalIds.Intersect(gids).ToList();

                class2PropertyIDs.Add(entityDmsType, propIds);

                ri = new ResourceIterator(globalIds, class2PropertyIDs);

                retVal = AddIterator(ri);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Getting extent values for entity type = {0} succedded.", entityType);
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get extent values for entity type = {0}. {1}", entityType, ex.Message);
                throw new Exception(message);
            }

            return retVal;
        }

        public int GetRelatedValues(long source, List<ModelCode> propIds, Association association)
        {
            ResourceIterator ri;
            int retVal = 0;

            try
            {
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, String.Format("Getting related values for source = 0x{0:x16}.", source));

                List<long> relatedGids = ApplyAssocioationOnSource(source, association);

                Dictionary<DMSType, List<ModelCode>> class2PropertyIDs = new Dictionary<DMSType, List<ModelCode>>();

                foreach (long relatedGid in relatedGids)
                {
                    DMSType entityDmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(relatedGid);

                    if (!class2PropertyIDs.ContainsKey(entityDmsType))
                    {
                        class2PropertyIDs.Add(entityDmsType, propIds);
                    }
                }

                ri = new ResourceIterator(relatedGids, class2PropertyIDs);

                // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, String.Format("Getting related values for source = 0x{0:x16} succeeded.", source));

                retVal = AddIterator(ri);
            }
            catch (Exception ex)
            {
                string message = String.Format("Failed to get related values for source GID = 0x{0:x16}. {1}.", source, ex.Message);

                // LOG CommonTrace.WriteTrace(CommonTrace.TraceError, message);

                throw new Exception(message);
            }

            return retVal;
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, String.Format("Getting values for GID = 0x{0:x16}.", globalId));

            try
            {
                IdentifiedObject io = storageComponent.GetEntity(resourceId);

                ResourceDescription rd = new ResourceDescription(resourceId);

                Property property = null;

                // insert specified properties
                foreach (ModelCode propId in propIds)
                {
                    property = new Property(propId);
                    io.GetProperty(property);
                    rd.AddProperty(property);
                }

                // LOG CommonTrace.WriteTrace(CommonTrace.TraceVerbose, String.Format("Getting values for GID = 0x{0:x16} succedded.", globalId));

                return rd;
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get values for entity with GID = 0x{0:x16}. {1}", resourceId, ex.Message);
                // LOG
                throw new Exception(message);
            }
        }

        public bool IteratorClose(int id)
        {
            try
            {
                bool retVal = RemoveIterator(id);

                return retVal;
            }
            catch (Exception ex)
            {
                string message = string.Format("IteratorClose failed. Iterator ID = {0}. {1}", id, ex.Message);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            try
            {
                List<ResourceDescription> retVal = GetIterator(id).Next(n);

                return retVal;
            }
            catch (Exception ex)
            {
                string message = string.Format("IteratorNext failed. Iterator ID = {0}. Resources to fetch count = {1}. {2} ", id, n, ex.Message);
                
                throw new Exception(message);
            }
        }

        public int IteratorResourcesLeft(int id)
        {
            try
            {
                int resourcesLeft = GetIterator(id).ResourcesLeft();

                return resourcesLeft;
            }
            catch (Exception ex)
            {
                string message = string.Format("IteratorResourcesLeft failed. Iterator ID = {0}. {1}", id, ex.Message);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        public int IteratorResourcesTotal(int id)
        {
            try
            {
                int retVal = GetIterator(id).ResourcesTotal();
                return retVal;
            }
            catch (Exception ex)
            {
                string message = string.Format("IteratorResourcesTotal failed. Iterator ID = {0}. {1}", id, ex.Message);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        public bool IteratorRewind(int id)
        {
            try
            {
                GetIterator(id).Rewind();

                return true;
            }
            catch (Exception ex)
            {
                string message = string.Format("IteratorRewind failed. Iterator ID = {0}. {1}", id, ex.Message);
                // LOG CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        private List<long> ApplyAssocioationOnSource(long source, Association association)
        {
            List<long> relatedGids = new List<long>();

            if (association == null)
            {
                association = new Association();
            }

            IdentifiedObject io = storageComponent.GetEntity(source);

            if (!io.HasProperty(association.PropertyId))
            {
                throw new Exception(string.Format("Entity with GID = 0x{0:x16} does not contain prperty with Id = {1}.", source, association.PropertyId));
            }

            Property propertyRef = null;
            if (Property.GetPropertyType(association.PropertyId) == PropertyType.Reference)
            {
                propertyRef = io.GetProperty(association.PropertyId);
                long relatedGidFromProperty = propertyRef.AsReference();

                if (relatedGidFromProperty != 0)
                {
                    if (association.Type == 0 || (short)ModelCodeHelper.GetTypeFromModelCode(association.Type) == ModelCodeHelper.ExtractTypeFromGlobalId(relatedGidFromProperty))
                    {
                        relatedGids.Add(relatedGidFromProperty);
                    }
                }
            }
            else if (Property.GetPropertyType(association.PropertyId) == PropertyType.ReferenceVector)
            {
                propertyRef = io.GetProperty(association.PropertyId);
                List<long> relatedGidsFromProperty = propertyRef.AsReferences();

                if (relatedGidsFromProperty != null)
                {
                    foreach (long relatedGidFromProperty in relatedGidsFromProperty)
                    {
                        if (association.Type == 0 || (short)ModelCodeHelper.GetTypeFromModelCode(association.Type) == ModelCodeHelper.ExtractTypeFromGlobalId(relatedGidFromProperty))
                        {
                            relatedGids.Add(relatedGidFromProperty);
                        }
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("Association propertyId = {0} is not reference or reference vector type.", association.PropertyId));
            }

            return relatedGids;
        }

        private int AddIterator(ResourceIterator iterator)
        {
            lock (resourceItMap)
            {
                int iteratorId = ++resourceItId;
                resourceItMap.Add(iteratorId, iterator);
                return iteratorId;
            }
        }

        private ResourceIterator GetIterator(int iteratorId)
        {
            lock (resourceItMap)
            {
                if (resourceItMap.ContainsKey(iteratorId))
                {
                    return resourceItMap[iteratorId];
                }
                else
                {
                    throw new Exception(string.Format("Iterator with given ID = {0} doesn't exist.", iteratorId));
                }
            }
        }

        private bool RemoveIterator(int iteratorId)
        {
            lock (resourceItMap)
            {
                return resourceItMap.Remove(iteratorId);
            }
        }
    }
}
