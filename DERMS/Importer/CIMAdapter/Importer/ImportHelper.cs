﻿using Common.AbstractModel;
using Common.GDA;
using System.Collections.Generic;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    /// <summary>
    /// Represents a <see cref="ResourceDescription"/> with a missing reference for given <see cref="ModelCode"/>, look at <see cref="ImportHelper.missingReferences"/>.
    /// </summary>
    internal struct MissingReferenceUnit
    {
        public ResourceDescription Rd;
        public ModelCode ModelCode;

        public MissingReferenceUnit(ResourceDescription rd, ModelCode modelCode)
        {
            this.Rd = rd;
            this.ModelCode = modelCode;
        }
    }

	/// <summary>
	/// ImportHelper
	/// </summary>
	public class ImportHelper
	{
		private Dictionary<DMSType, int> typeCounter = new Dictionary<DMSType, int>();

        /// <summary>
        /// References such as one(optional) to one(mandatory)
        /// </summary>
        private Dictionary<string, MissingReferenceUnit> missingReferences = new Dictionary<string, MissingReferenceUnit>();
		private Dictionary<string, long> rdfIDtoGIDMapping = new Dictionary<string, long>();


		/// <summary>
		/// Method calculates the next negative index for given DMSType.
		/// </summary>
		/// <param name="dmsType"></param>
		/// <returns></returns>
		public int CheckOutIndexForDMSType(DMSType dmsType)
		{
			int counter = -1;
			lock (typeCounter)
			{
				int temp;
				if (typeCounter.TryGetValue(dmsType, out temp))
				{
					counter = temp - 1;
					typeCounter[dmsType] = counter;
				}
				else
				{
					typeCounter.Add(dmsType, counter);
				}
			}
			return counter;
		}

		/// <summary>
		/// Method inserts or updates the id mapping: CIM rdfID to NMS gid.
		/// </summary>
		/// <param name="rdfID"></param>
		/// <param name="gid"></param>
		public void DefineIDMapping(string rdfID, long gid)
		{
			if (rdfIDtoGIDMapping.ContainsKey(rdfID))
			{
				rdfIDtoGIDMapping[rdfID] = gid;
			}
			else
			{
				rdfIDtoGIDMapping.Add(rdfID, gid);
			}
		}

		/// <summary>
		/// Method returns the value of gid that is mapped to given rdfID. If there is no mapping defined, method returns -1.
		/// </summary>
		/// <param name="rdfID">CIM rdfID value</param>
		/// <returns>gid or -1 (if mapping not defined)</returns>
		public long GetMappedGID(string rdfID)
		{
			long gid = -1;
			if (!rdfIDtoGIDMapping.TryGetValue(rdfID, out gid))
			{
				gid = -1;
			}
			return gid;
		}

        /// <summary>
        /// Remembers <see cref="ResourceDescription"/> which are missing references.
        /// </summary>
        /// <param name="rd">Which is missing a reference.</param>
        /// <param name="modelCode">Of a missing reference.</param>
        /// <param name="missingRdfId">RDFID of a missing entity.</param>
        public void AddReferenceToMissingResourceDescription(ResourceDescription rd, ModelCode modelCode, string missingRdfId)
        {
            missingReferences.Add(missingRdfId, new MissingReferenceUnit(rd, modelCode));
        }

        /// <summary>
        /// Adds missing references to resource descriptions added by <see cref="AddReferenceToMissingResourceDescription(ResourceDescription, ModelCode, string)"/>. 
        /// </summary>
        public void ExecuteReferenceAddition()
        {
            foreach (KeyValuePair<string, MissingReferenceUnit> missingReference in missingReferences)
            {
                MissingReferenceUnit unit = missingReference.Value;

                unit.Rd.AddProperty(new Property(unit.ModelCode, rdfIDtoGIDMapping[missingReference.Key]));
            }
        }
	}
}
