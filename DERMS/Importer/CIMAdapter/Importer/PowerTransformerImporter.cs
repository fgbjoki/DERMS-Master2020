using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;
using Common.GDA;
using Common.AbstractModel;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
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

			//// import all concrete model types (DMSType enum)
            // TODO

			//ImportBaseVoltages();
			//ImportLocations();
			//ImportPowerTransformers();
			//ImportTransformerWindings();
			//ImportWindingTests();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		//private void ImportBaseVoltages()
		//{
		//	SortedDictionary<string, object> cimBaseVoltages = concreteModel.GetAllObjectsOfType("FTN.BaseVoltage");
		//	if (cimBaseVoltages != null)
		//	{
		//		foreach (KeyValuePair<string, object> cimBaseVoltagePair in cimBaseVoltages)
		//		{
		//			FTN.BaseVoltage cimBaseVoltage = cimBaseVoltagePair.Value as FTN.BaseVoltage;

		//			ResourceDescription rd = CreateBaseVoltageResourceDescription(cimBaseVoltage);
		//			if (rd != null)
		//			{
		//				delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
		//				report.Report.Append("BaseVoltage ID = ").Append(cimBaseVoltage.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		//			}
		//			else
		//			{
		//				report.Report.Append("BaseVoltage ID = ").Append(cimBaseVoltage.ID).AppendLine(" FAILED to be converted");
		//			}
		//		}
		//		report.Report.AppendLine();
		//	}
		//}

		//private ResourceDescription CreateBaseVoltageResourceDescription(FTN.BaseVoltage cimBaseVoltage)
		//{
		//	ResourceDescription rd = null;
		//	if (cimBaseVoltage != null)
		//	{
		//		long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BASEVOLTAGE, importHelper.CheckOutIndexForDMSType(DMSType.BASEVOLTAGE));
		//		rd = new ResourceDescription(gid);
		//		importHelper.DefineIDMapping(cimBaseVoltage.ID, gid);

		//		////populate ResourceDescription
		//		PowerTransformerConverter.PopulateBaseVoltageProperties(cimBaseVoltage, rd);
		//	}
		//	return rd;
		//}
		
		//private void ImportLocations()
		//{
		//	SortedDictionary<string, object> cimLocations = concreteModel.GetAllObjectsOfType("FTN.Location");
		//	if (cimLocations != null)
		//	{
		//		foreach (KeyValuePair<string, object> cimLocationPair in cimLocations)
		//		{
		//			FTN.Location cimLocation = cimLocationPair.Value as FTN.Location;

		//			ResourceDescription rd = CreateLocationResourceDescription(cimLocation);
		//			if (rd != null)
		//			{
		//				delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
		//				report.Report.Append("Location ID = ").Append(cimLocation.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		//			}
		//			else
		//			{
		//				report.Report.Append("Location ID = ").Append(cimLocation.ID).AppendLine(" FAILED to be converted");
		//			}
		//		}
		//		report.Report.AppendLine();
		//	}
		//}

		//private ResourceDescription CreateLocationResourceDescription(FTN.Location cimLocation)
		//{
		//	ResourceDescription rd = null;
		//	if (cimLocation != null)
		//	{
		//		long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.LOCATION, importHelper.CheckOutIndexForDMSType(DMSType.LOCATION));
		//		rd = new ResourceDescription(gid);
		//		importHelper.DefineIDMapping(cimLocation.ID, gid);

		//		////populate ResourceDescription
		//		PowerTransformerConverter.PopulateLocationProperties(cimLocation, rd);
		//	}
		//	return rd;
		//}

		//private void ImportPowerTransformers()
		//{
		//	SortedDictionary<string, object> cimPowerTransformers = concreteModel.GetAllObjectsOfType("FTN.PowerTransformer");
		//	if (cimPowerTransformers != null)
		//	{
		//		foreach (KeyValuePair<string, object> cimPowerTransformerPair in cimPowerTransformers)
		//		{
		//			FTN.PowerTransformer cimPowerTransformer = cimPowerTransformerPair.Value as FTN.PowerTransformer;

		//			ResourceDescription rd = CreatePowerTransformerResourceDescription(cimPowerTransformer);
		//			if (rd != null)
		//			{
		//				delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
		//				report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		//			}
		//			else
		//			{
		//				report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).AppendLine(" FAILED to be converted");
		//			}
		//		}
		//		report.Report.AppendLine();
		//	}
		//}

		//private ResourceDescription CreatePowerTransformerResourceDescription(FTN.PowerTransformer cimPowerTransformer)
		//{
		//	ResourceDescription rd = null;
		//	if (cimPowerTransformer != null)
		//	{
		//		long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTR, importHelper.CheckOutIndexForDMSType(DMSType.POWERTR));
		//		rd = new ResourceDescription(gid);
		//		importHelper.DefineIDMapping(cimPowerTransformer.ID, gid);

		//		////populate ResourceDescription
		//		PowerTransformerConverter.PopulatePowerTransformerProperties(cimPowerTransformer, rd, importHelper, report);
		//	}
		//	return rd;
		//}

		//private void ImportTransformerWindings()
		//{
		//	SortedDictionary<string, object> cimTransformerWindings = concreteModel.GetAllObjectsOfType("FTN.TransformerWinding");
		//	if (cimTransformerWindings != null)
		//	{
		//		foreach (KeyValuePair<string, object> cimTransformerWindingPair in cimTransformerWindings)
		//		{
		//			FTN.TransformerWinding cimTransformerWinding = cimTransformerWindingPair.Value as FTN.TransformerWinding;

		//			ResourceDescription rd = CreateTransformerWindingResourceDescription(cimTransformerWinding);
		//			if (rd != null)
		//			{
		//				delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
		//				report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		//			}
		//			else
		//			{
		//				report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).AppendLine(" FAILED to be converted");
		//			}
		//		}
		//		report.Report.AppendLine();
		//	}
		//}

		//private ResourceDescription CreateTransformerWindingResourceDescription(FTN.TransformerWinding cimTransformerWinding)
		//{
		//	ResourceDescription rd = null;
		//	if (cimTransformerWinding != null)
		//	{
		//		long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTRWINDING, importHelper.CheckOutIndexForDMSType(DMSType.POWERTRWINDING));
		//		rd = new ResourceDescription(gid);
		//		importHelper.DefineIDMapping(cimTransformerWinding.ID, gid);

		//		////populate ResourceDescription
		//		PowerTransformerConverter.PopulateTransformerWindingProperties(cimTransformerWinding, rd, importHelper, report);
		//	}
		//	return rd;
		//}

		//private void ImportWindingTests()
		//{
		//	SortedDictionary<string, object> cimWindingTests = concreteModel.GetAllObjectsOfType("FTN.WindingTest");
		//	if (cimWindingTests != null)
		//	{
		//		foreach (KeyValuePair<string, object> cimWindingTestPair in cimWindingTests)
		//		{
		//			FTN.WindingTest cimWindingTest = cimWindingTestPair.Value as FTN.WindingTest;

		//			ResourceDescription rd = CreateWindingTestResourceDescription(cimWindingTest);
		//			if (rd != null)
		//			{
		//				delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
		//				report.Report.Append("WindingTest ID = ").Append(cimWindingTest.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		//			}
		//			else
		//			{
		//				report.Report.Append("WindingTest ID = ").Append(cimWindingTest.ID).AppendLine(" FAILED to be converted");
		//			}
		//		}
		//		report.Report.AppendLine();
		//	}
		//}

		//private ResourceDescription CreateWindingTestResourceDescription(FTN.WindingTest cimWindingTest)
		//{
		//	ResourceDescription rd = null;
		//	if (cimWindingTest != null)
		//	{
		//		long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.WINDINGTEST, importHelper.CheckOutIndexForDMSType(DMSType.WINDINGTEST));
		//		rd = new ResourceDescription(gid);
		//		importHelper.DefineIDMapping(cimWindingTest.ID, gid);

		//		////populate ResourceDescription
		//		PowerTransformerConverter.PopulateWindingTestProperties(cimWindingTest, rd, importHelper, report);
		//	}
		//	return rd;
		//}
		#endregion Import
	}
}

