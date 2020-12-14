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

            ImportGeographicalRegions();
            ImportSubGeographicalRegions();
            ImportSubstations();
			ImportEnergySources();
			ImportEnergyConsumers();
            ImportDEREnergyStorages();
            ImportSolarGenerators();
            ImportWindGenerators();
            ImportBreakers();
            ImportConnectivityNodes();
            ImportTerminals();
            ImportDiscretes();
            ImportAnalogs();

            importHelper.ExecuteReferenceAddition();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region Import

        #region GeographicalRegion
        private void ImportGeographicalRegions()
        {
            SortedDictionary<string, object> cimGeographicalRegions = concreteModel.GetAllObjectsOfType("DERMS.GeographicalRegion");
            if (cimGeographicalRegions != null)
            {
                foreach (KeyValuePair<string, object> cimGeographicalRegionPair in cimGeographicalRegions)
                {
                    GeographicalRegion cimGeographicalRegion = cimGeographicalRegionPair.Value as GeographicalRegion;

                    ResourceDescription rd = CreateGeographicalRegionResourceDescription(cimGeographicalRegion);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("GeographicalRegion ID = ").Append(cimGeographicalRegion.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("GeographicalRegion ID = ").Append(cimGeographicalRegion.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateGeographicalRegionResourceDescription(GeographicalRegion geographicalRegion)
        {
            ResourceDescription rd = null;
            if (geographicalRegion != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.GEOGRAPHICALREGION, importHelper.CheckOutIndexForDMSType(DMSType.GEOGRAPHICALREGION));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(geographicalRegion.ID, gid);

                DERMSConverter.PopulateGeographicalRegionProperties(geographicalRegion, rd);
            }
            return rd;
        }

        #endregion GeographicalRegion

        #region SubGeographicalRegion

        private void ImportSubGeographicalRegions()
        {
            SortedDictionary<string, object> cimSubGeographicalRegions = concreteModel.GetAllObjectsOfType("DERMS.SubGeographicalRegion");
            if (cimSubGeographicalRegions != null)
            {
                foreach (KeyValuePair<string, object> cimSubGeographicalRegionPair in cimSubGeographicalRegions)
                {
                    SubGeographicalRegion cimSubGeographicalRegion = cimSubGeographicalRegionPair.Value as SubGeographicalRegion;

                    ResourceDescription rd = CreateSubGeographicalRegionResourceDescription(cimSubGeographicalRegion);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SubGeographicalRegion ID = ").Append(cimSubGeographicalRegion.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SubGeographicalRegion ID = ").Append(cimSubGeographicalRegion.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSubGeographicalRegionResourceDescription(SubGeographicalRegion subGeographicalRegion)
        {
            ResourceDescription rd = null;
            if (subGeographicalRegion != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SUBGEOGRAPHICALREGION, importHelper.CheckOutIndexForDMSType(DMSType.SUBGEOGRAPHICALREGION));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(subGeographicalRegion.ID, gid);

                DERMSConverter.PopulateSubGeographicalRegionProperties(subGeographicalRegion, rd, importHelper, report);
            }
            return rd;
        }

        #endregion SubGeographicalRegion

        #region Substations

        private void ImportSubstations()
        {
            SortedDictionary<string, object> cimSubstations = concreteModel.GetAllObjectsOfType("DERMS.Substation");
            if (cimSubstations != null)
            {
                foreach (KeyValuePair<string, object> cimSubstationPair in cimSubstations)
                {
                    Substation cimSubstation = cimSubstationPair.Value as Substation;

                    ResourceDescription rd = CreateSubstationResourceDescription(cimSubstation);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Substation ID = ").Append(cimSubstation.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Substation ID = ").Append(cimSubstation.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSubstationResourceDescription(Substation cimSubstation)
        {
            ResourceDescription rd = null;
            if (cimSubstation != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SUBSTATION, importHelper.CheckOutIndexForDMSType(DMSType.SUBSTATION));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSubstation.ID, gid);

                DERMSConverter.PopulateSubstationProperties(cimSubstation, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Substations

        #region EnergySources

        private void ImportEnergySources()
        {
            SortedDictionary<string, object> cimEnergySources = concreteModel.GetAllObjectsOfType("DERMS.EnergySource");
            if (cimEnergySources != null)
            {
                foreach (KeyValuePair<string, object> cimEnergySourcePair in cimEnergySources)
                {
                    EnergySource cimEnergySource = cimEnergySourcePair.Value as EnergySource;
                    

                    ResourceDescription rd = CreateEnergySourceResourceDescription(cimEnergySource);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("EnergySource ID = ").Append(cimEnergySource.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("EnergySource ID = ").Append(cimEnergySource.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateEnergySourceResourceDescription(EnergySource cimEnergySource)
        {
            ResourceDescription rd = null;
            if (cimEnergySource != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ENERGYSOURCE, importHelper.CheckOutIndexForDMSType(DMSType.ENERGYSOURCE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimEnergySource.ID, gid);

                DERMSConverter.PopulateEnergySourceProperties(cimEnergySource, rd, importHelper, report);
            }
            return rd;
        }

        #endregion EnergySources

        #region EnergyConsumers

        private void ImportEnergyConsumers()
        {
            SortedDictionary<string, object> cimEnergyConsumers = concreteModel.GetAllObjectsOfType("DERMS.EnergyConsumer");
            if (cimEnergyConsumers != null)
            {
                foreach (KeyValuePair<string, object> cimLocationPair in cimEnergyConsumers)
                {
                    EnergyConsumer energyConsumer = cimLocationPair.Value as EnergyConsumer;

                    ResourceDescription rd = CreateLocationResourceDescription(energyConsumer);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("EnergyConsumer ID = ").Append(energyConsumer.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateLocationResourceDescription(EnergyConsumer cimEnergyConsumer)
        {
            ResourceDescription rd = null;
            if (cimEnergyConsumer != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ENERGYCONSUMER, importHelper.CheckOutIndexForDMSType(DMSType.ENERGYCONSUMER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimEnergyConsumer.ID, gid);

                ////populate ResourceDescription
                DERMSConverter.PopulateEnergyConsumerProperties(cimEnergyConsumer, rd, importHelper, report);
            }
            return rd;
        }

        #endregion EnergyConsumers

        #region DEREnergyStorages

        private void ImportDEREnergyStorages()
        {
            SortedDictionary<string, object> cimEnergyStorages = concreteModel.GetAllObjectsOfType("DERMS.EnergyStorage");
            if (cimEnergyStorages != null)
            {
                foreach (KeyValuePair<string, object> cimEnergyStoragePair in cimEnergyStorages)
                {
                    EnergyStorage cimEnergyStorage = cimEnergyStoragePair.Value as EnergyStorage;

                    ResourceDescription rd = CreateEnergyStorageDescription(cimEnergyStorage);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("EnergyStorage ID = ").Append(cimEnergyStorage.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("EnergyStorage ID = ").Append(cimEnergyStorage.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateEnergyStorageDescription(EnergyStorage energyStorage)
        {
            ResourceDescription rd = null;
            if (energyStorage != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ENERGYSTORAGE, importHelper.CheckOutIndexForDMSType(DMSType.ENERGYSTORAGE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(energyStorage.ID, gid);

                DERMSConverter.PopulateEnergyStorageProperties(energyStorage, rd, importHelper, report);
            }
            return rd;
        }

        #endregion DEREnergyStorages

        #region SolarGenerators

        private void ImportSolarGenerators()
        {
            SortedDictionary<string, object> cimSolarGenerators = concreteModel.GetAllObjectsOfType("DERMS.SolarGenerator");
            if (cimSolarGenerators != null)
            {
                foreach (KeyValuePair<string, object> cimSolarGeneratorPair in cimSolarGenerators)
                {
                    SolarGenerator cimSolarGenerator = cimSolarGeneratorPair.Value as SolarGenerator;

                    ResourceDescription rd = CreateSolarGeneratorDescription(cimSolarGenerator);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SolarGenerator ID = ").Append(cimSolarGenerator.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SolarGenerator ID = ").Append(cimSolarGenerator.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSolarGeneratorDescription(SolarGenerator cimSolarGenerator)
        {
            ResourceDescription rd = null;
            if (cimSolarGenerator != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SOLARGENERATOR, importHelper.CheckOutIndexForDMSType(DMSType.SOLARGENERATOR));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSolarGenerator.ID, gid);

                DERMSConverter.PopulateSolarGeneratorProperties(cimSolarGenerator, rd, importHelper, report);
            }
            return rd;
        }

        #endregion

        #region WindGenerators

        private void ImportWindGenerators()
        {
            SortedDictionary<string, object> cimWindGenerators = concreteModel.GetAllObjectsOfType("DERMS.WindGenerator");
            if (cimWindGenerators != null)
            {
                foreach (KeyValuePair<string, object> cimWindGeneratorPair in cimWindGenerators)
                {
                    WindGenerator cimWindGenerator = cimWindGeneratorPair.Value as WindGenerator;

                    ResourceDescription rd = CreateWindGeneratorDescription(cimWindGenerator);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("WindGenerator ID = ").Append(cimWindGenerator.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("WindGenerator ID = ").Append(cimWindGenerator.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateWindGeneratorDescription(WindGenerator windGenerator)
        {
            ResourceDescription rd = null;
            if (windGenerator != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SOLARGENERATOR, importHelper.CheckOutIndexForDMSType(DMSType.SOLARGENERATOR));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(windGenerator.ID, gid);

                DERMSConverter.PopulateWindGeneratorProperties(windGenerator, rd, importHelper, report);
            }
            return rd;
        }

        #endregion WindGenerators

        #region Breakers
        private void ImportBreakers()
        {
            SortedDictionary<string, object> cimBreakers = concreteModel.GetAllObjectsOfType("DERMS.Breaker");
            if (cimBreakers != null)
            {
                foreach (KeyValuePair<string, object> cimBreakerPair in cimBreakers)
                {
                    Breaker cimBreaker = cimBreakerPair.Value as Breaker;

                    ResourceDescription rd = CreateBreakerDescription(cimBreaker);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Breaker ID = ").Append(cimBreaker.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Breaker ID = ").Append(cimBreaker.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateBreakerDescription(Breaker breaker)
        {
            ResourceDescription rd = null;
            if (breaker != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BREAKER, importHelper.CheckOutIndexForDMSType(DMSType.BREAKER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(breaker.ID, gid);

                DERMSConverter.PopulateBreakerProperties(breaker, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Breakers

        #region ACLineSegment

        private void ImportACLineSegment()
        {
            SortedDictionary<string, object> cimACLineSegments = concreteModel.GetAllObjectsOfType("DERMS.ACLineSegment");
            if (cimACLineSegments != null)
            {
                foreach (KeyValuePair<string, object> cimACLineSegmentPair in cimACLineSegments)
                {
                    ACLineSegment cimACLineSegment = cimACLineSegmentPair.Value as ACLineSegment;

                    ResourceDescription rd = CreateACLineSegmentDescription(cimACLineSegment);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("ACLineSegment ID = ").Append(cimACLineSegment.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("ACLineSegment ID = ").Append(cimACLineSegment.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateACLineSegmentDescription(ACLineSegment ACLineSegment)
        {
            ResourceDescription rd = null;
            if (ACLineSegment != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLINESEG, importHelper.CheckOutIndexForDMSType(DMSType.ACLINESEG));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(ACLineSegment.ID, gid);

                DERMSConverter.PopulateACLineSegmentProperties(ACLineSegment, rd, importHelper, report);
            }
            return rd;
        }

        #endregion ACLineSegment

        #region ConnectivityNodes

        private void ImportConnectivityNodes()
        {
            SortedDictionary<string, object> cimConnectivityNodes = concreteModel.GetAllObjectsOfType("DERMS.ConnectivityNode");
            if (cimConnectivityNodes != null)
            {
                foreach (KeyValuePair<string, object> cimConnectivityNodePair in cimConnectivityNodes)
                {
                    ConnectivityNode cimConnectivityNode = cimConnectivityNodePair.Value as ConnectivityNode;

                    ResourceDescription rd = CreateConnectivityNodeDescription(cimConnectivityNode);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("ConnectivityNode ID = ").Append(cimConnectivityNode.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("ConnectivityNode ID = ").Append(cimConnectivityNode.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateConnectivityNodeDescription(ConnectivityNode connectivityNode)
        {
            ResourceDescription rd = null;
            if (connectivityNode != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTIVITYNODE, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTIVITYNODE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(connectivityNode.ID, gid);

                DERMSConverter.PopulateConnectivityNodeProperties(connectivityNode, rd, importHelper, report);
            }
            return rd;
        }

        #endregion ConnectivityNodes

        #region Terminals

        private void ImportTerminals()
        {
            SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("DERMS.Terminal");
            if (cimTerminals != null)
            {
                foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
                {
                    Terminal cimTerminal = cimTerminalPair.Value as Terminal;

                    ResourceDescription rd = CreateTerminalDescription(cimTerminal);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTerminalDescription(Terminal terminal)
        {
            ResourceDescription rd = null;
            if (terminal != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(terminal.ID, gid);

                DERMSConverter.PopulateTerminalProperties(terminal, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Terminal

        #region Discrete

        private void ImportDiscretes()
        {
            SortedDictionary<string, object> cimDiscretes = concreteModel.GetAllObjectsOfType("DERMS.Discrete");
            if (cimDiscretes != null)
            {
                foreach (KeyValuePair<string, object> cimDiscretePair in cimDiscretes)
                {
                    Discrete cimDiscrete = cimDiscretePair.Value as Discrete;

                    ResourceDescription rd = CreateDiscreteDescription(cimDiscrete);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Discrete ID = ").Append(cimDiscrete.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Discrete ID = ").Append(cimDiscrete.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateDiscreteDescription(Discrete discrete)
        {
            ResourceDescription rd = null;
            if (discrete != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MEASUREMENTDISCRETE, importHelper.CheckOutIndexForDMSType(DMSType.MEASUREMENTDISCRETE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(discrete.ID, gid);

                DERMSConverter.PopulateDiscreteProperties(discrete, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Discrete

        #region Analog

        private void ImportAnalogs()
        {
            SortedDictionary<string, object> cimAnalogs = concreteModel.GetAllObjectsOfType("DERMS.Analog");
            if (cimAnalogs != null)
            {
                foreach (KeyValuePair<string, object> cimAnalogPair in cimAnalogs)
                {
                    Analog cimAnalog = cimAnalogPair.Value as Analog;

                    ResourceDescription rd = CreateAnalogDescription(cimAnalog);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Analog ID = ").Append(cimAnalog.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Analog ID = ").Append(cimAnalog.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateAnalogDescription(Analog analog)
        {
            ResourceDescription rd = null;
            if (analog != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MEASUREMENTANALOG, importHelper.CheckOutIndexForDMSType(DMSType.MEASUREMENTANALOG));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(analog.ID, gid);

                DERMSConverter.PopulateAnalogProperties(analog, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Analog

        #endregion Import
    }
}

