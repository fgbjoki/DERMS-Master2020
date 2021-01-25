﻿using Common.AbstractModel;
using Common.GDA;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace NetworkManagementService.Components.GDA
{
    public class ResourceIterator
    {
        private INetworkModelGDAContract gdaProcessor = null;

        private List<long> globalDs = new List<long>();
        private Dictionary<DMSType, List<ModelCode>> class2PropertyIDs = new Dictionary<DMSType, List<ModelCode>>();

        private int lastReadIndex = 0; // index of the last read resource description
        private int maxReturnNo = 5000;


        public ResourceIterator(INetworkModelGDAContract gdaProcessor)
        {
            this.gdaProcessor = gdaProcessor;
        }

        public ResourceIterator(List<long> globalIDs, Dictionary<DMSType, List<ModelCode>> class2PropertyIDs, INetworkModelGDAContract gdaProcessor) : this(gdaProcessor)
        {
            this.globalDs = globalIDs;
            this.class2PropertyIDs = class2PropertyIDs;
        }

        public int ResourcesLeft()
        {
            return globalDs.Count - lastReadIndex;
        }

        public int ResourcesTotal()
        {
            return globalDs.Count;
        }

        public List<ResourceDescription> Next(int n)
        {
            try
            {
                if (n < 0)
                {
                    return null;
                }

                if (n > maxReturnNo)
                {
                    n = maxReturnNo;
                }

                List<long> resultIDs;

                if (ResourcesLeft() < n)
                {
                    resultIDs = globalDs.GetRange(lastReadIndex, globalDs.Count - lastReadIndex);
                    lastReadIndex = globalDs.Count;
                }
                else
                {
                    resultIDs = globalDs.GetRange(lastReadIndex, n);
                    lastReadIndex += n;
                }

                List<ResourceDescription> result = CollectData(resultIDs);

                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get next set of ResourceDescription iterators. {0}", ex.Message);
                // LOG 
                throw new Exception(message);
            }
        }

        public List<ResourceDescription> GetRange(int index, int n)
        {
            try
            {
                if (n > maxReturnNo)
                {
                    n = maxReturnNo;
                }

                List<long> resultIDs = globalDs.GetRange(index, n);

                List<ResourceDescription> result = CollectData(resultIDs);

                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get range of ResourceDescription iterators. index:{0}, count:{1}. {2}", index, n, ex.Message);
                // LOG 
                throw new Exception(message);
            }
        }

        public void Rewind()
        {
            lastReadIndex = 0;
        }

        private List<ResourceDescription> CollectData(List<long> resultIDs)
        {
            try
            {
                List<ResourceDescription> result = new List<ResourceDescription>();

                List<ModelCode> propertyIds = null;
                foreach (long globalId in resultIDs)
                {
                    propertyIds = class2PropertyIDs[(DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId)];
                    result.Add(gdaProcessor.GetValues(globalId, propertyIds));
                }

                return result;
            }
            catch (Exception ex)
            {
                //LOG CommonTrace.WriteTrace(CommonTrace.TraceError, "Collecting ResourceDescriptions failed. Exception: " + ex.Message);
                throw;
            }
        }
    }
}
