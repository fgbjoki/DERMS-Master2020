using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;
using Common.GDA;
using Common.AbstractModel;
using DERMS;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class DERMSImporter
	{
		/// <summary> Singleton </summary>
		private static DERMSImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static DERMSImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new DERMSImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            Import<GeographicalRegion>(DMSType.GEOGRAPHICALREGION);
            Import<SubGeographicalRegion>(DMSType.SUBGEOGRAPHICALREGION);
            Import<Substation>(DMSType.SUBSTATION);
            Import<EnergySource>(DMSType.ENERGYSOURCE);
            Import<EnergyConsumer>(DMSType.ENERGYCONSUMER);
            Import<EnergyStorage>(DMSType.ENERGYSTORAGE);
            Import<SolarGenerator>(DMSType.SOLARGENERATOR);
            Import<WindGenerator>(DMSType.WINDGENERATOR);
            Import<Breaker>(DMSType.BREAKER);
            Import<ACLineSegment>(DMSType.ACLINESEG);
            Import<ConnectivityNode>(DMSType.CONNECTIVITYNODE);
            Import<Terminal>(DMSType.TERMINAL);
            Import<Discrete>(DMSType.MEASUREMENTDISCRETE);
            Import<Analog>(DMSType.MEASUREMENTANALOG);

            importHelper.ExecuteReferenceAddition();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region Import

        /// Generic method to import cim objects based on DMSType
        private void Import<T>(DMSType dmsType) where T : DERMS.IdentifiedObject
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType(typeof(T).FullName);
            if (cimObjects == null)
                return;

            foreach (var kvp in cimObjects)
            {
                T cimObj = (T)kvp.Value;
                var rd = CreateResourceDescription(cimObj, dmsType);

                if (rd == null)
                {
                    report.Report.Append($"{typeof(T).Name} ID = ").Append(cimObj.ID)
                        .AppendLine(" FAILED to be converted");
                    continue;
                }
                else
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append($"{typeof(T).Name} ID = ").Append(cimObj.ID)
                        .Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                report.Report.AppendLine();
            }
        }

        /// Generic method to create resource description based on DMSType
        private ResourceDescription CreateResourceDescription<T>(T cimObj, DMSType dmsType) where T : DERMS.IdentifiedObject
        {
            if (cimObj == null)
                return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)dmsType,
                importHelper.CheckOutIndexForDMSType(dmsType));

            var rd = new ResourceDescription(gid);

            importHelper.DefineIDMapping(cimObj.ID, gid);

            DERMSConverter.PopulateProperties(cimObj, rd, importHelper, report);

            return rd;
        }
        #endregion Import
    }
}

