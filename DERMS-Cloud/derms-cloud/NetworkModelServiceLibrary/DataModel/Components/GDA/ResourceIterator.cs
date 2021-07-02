﻿using Common.ServiceInterfaces;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NetworkManagementService.Components.GDA
{
    [DataContract]
    public class ResourceIterator
    {
        private List<long> globalDs = new List<long>();
        private Dictionary<DMSType, List<ModelCode>> class2PropertyIDs = new Dictionary<DMSType, List<ModelCode>>();

        private int lastReadIndex = 0; // index of the last read resource description
        private int maxReturnNo = 5000;

        [DataMember]
        public List<long> GlobalDs { get => globalDs; set => globalDs = value; }
        [DataMember]
        public Dictionary<DMSType, List<ModelCode>> Class2PropertyIDs { get => class2PropertyIDs; set => class2PropertyIDs = value; }

        public ResourceIterator(List<long> globalIDs, Dictionary<DMSType, List<ModelCode>> class2PropertyIDs)
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

        public List<ResourceDescription> Next(int n, INetworkModelGDAContract gdaProcessor)
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

                List<ResourceDescription> result = CollectData(resultIDs, gdaProcessor);

                return result;
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get next set of ResourceDescription iterators. {0}", ex.Message);
                // LOG 
                throw new Exception(message);
            }
        }

        public List<ResourceDescription> GetRange(int index, int n, INetworkModelGDAContract gdaProcessor)
        {
            try
            {
                if (n > maxReturnNo)
                {
                    n = maxReturnNo;
                }

                List<long> resultIDs = globalDs.GetRange(index, n);

                List<ResourceDescription> result = CollectData(resultIDs, gdaProcessor);

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

        private List<ResourceDescription> CollectData(List<long> resultIDs, INetworkModelGDAContract gdaProcessor)
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
