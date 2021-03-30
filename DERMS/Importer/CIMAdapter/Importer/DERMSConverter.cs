using Common.AbstractModel;
using Common.GDA;
using DERMS;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class DERMSConverter
	{

        //#region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(DERMS.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.DescriptionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_DESCRIPTION, cimIdentifiedObject.Description));
                }
            }
        }

        public static void PopulateEnergyConsumerProperties(EnergyConsumer cimEnergyConsumer, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEnergyConsumer != null) && (rd != null))
            {
                DERMSConverter.PopulateConductingEquipmentProperties(cimEnergyConsumer, rd, importHelper, report);

                if (cimEnergyConsumer.PfixedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYCONSUMER_PFIXED, cimEnergyConsumer.Pfixed));
                }
            }
        }

        public static void PopulateGeographicalRegionProperties(GeographicalRegion geographicalRegion, ResourceDescription rd)
        {
            if ((geographicalRegion != null) && (rd != null))
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(geographicalRegion, rd);
            }
        }

        public static void PopulateSubGeographicalRegionProperties(SubGeographicalRegion subGeographicalRegion, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((subGeographicalRegion != null) && (rd != null))
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(subGeographicalRegion, rd);

                if (subGeographicalRegion.RegionHasValue)
                {
                    long gid = importHelper.GetMappedGID(subGeographicalRegion.Region.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(subGeographicalRegion.GetType().ToString()).Append(" rdfID = \"").Append(subGeographicalRegion.ID);
                        report.Report.Append("\" - Failed to set reference to Region: rdfID \"").Append(subGeographicalRegion.Region.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.SUBGEOGRAPHICALREGION_REGION, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(subGeographicalRegion.GetType().ToString()).Append(" rdfID = \"").Append(subGeographicalRegion.ID);
                    report.Report.Append("\" - Failed to set reference to Region: rdfID \"").Append(subGeographicalRegion.Region.ID).AppendLine(" \" is not mapped to GID!");
                }
            }
        }

        private static void PopulatePowerSystemResourceProperties(DERMS.PowerSystemResource psr, ResourceDescription rd)
        {
            if ((psr != null) && (rd != null))
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(psr, rd);
            }
        }

        private static void PopulateConnectivityNodeContainerProperties(ConnectivityNodeContainer cnc, ResourceDescription rd)
        {
            if ((cnc != null) && (rd != null))
            {
                DERMSConverter.PopulatePowerSystemResourceProperties(cnc, rd);
            }
        }

        private static void PopulateEquipmentContainerProperties(EquipmentContainer eq, ResourceDescription rd)
        {
            if ((eq != null) && (rd != null))
            {
                DERMSConverter.PopulateConnectivityNodeContainerProperties(eq, rd);
            }
        }

        public static void PopulateSubstationProperties(Substation substation, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((substation != null) && (rd != null))
            {
                DERMSConverter.PopulateEquipmentContainerProperties(substation, rd);

                if (substation.RegionHasValue)
                {
                    long gid = importHelper.GetMappedGID(substation.Region.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(substation.GetType().ToString()).Append(" rdfID = \"").Append(substation.ID);
                        report.Report.Append("\" - Failed to set reference to SubGeographicalRegion: rdfID \"").Append(substation.Region.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.SUBSTATION_REGION, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(substation.GetType().ToString()).Append(" rdfID = \"").Append(substation.ID);
                    report.Report.Append("\" - Failed to set reference to SubGeographicalRegion: rdfID \"").Append(substation.Region.ID).AppendLine(" \" is not mapped to GID!");
                }
            }
        }

        public static void PopulateEnergySourceProperties(EnergySource energySource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((energySource != null) && (rd != null))
            {
                DERMSConverter.PopulateConductingEquipmentProperties(energySource, rd, importHelper, report);

                if (energySource.ActivePowerHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYSOURCE_ACTIVEPOWER, energySource.ActivePower));
                }
            }
        }

        private static void PopulateEquipmentProperties(DERMS.Equipment equipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((equipment != null) && (rd != null))
            {
                DERMSConverter.PopulatePowerSystemResourceProperties(equipment, rd);

                if (equipment.EquipmentContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(equipment.EquipmentContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(equipment.EquipmentContainer.GetType().ToString()).Append(" rdfID = \"").Append(equipment.ID);
                        report.Report.Append("\" - Failed to set reference to EquipmentContainer: rdfID \"").Append(equipment.EquipmentContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.EQUIPMENT_EQCONTAINER, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(equipment.EquipmentContainer.GetType().ToString()).Append(" rdfID = \"").Append(equipment.ID);
                    report.Report.Append("\" - Failed to set reference to EquipmentContainer: rdfID \"").Append(equipment.EquipmentContainer.ID).AppendLine(" \" is not mapped to GID!");
                }
            }
        }

        private static void PopulateConductingEquipmentProperties(DERMS.ConductingEquipment conductingEqupment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (conductingEqupment != null && rd != null)
            {
                DERMSConverter.PopulateEquipmentProperties(conductingEqupment, rd, importHelper, report);
            }
        }

        private static void PopulateDERProperties(DER der, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (der != null && rd != null)
            {
                DERMSConverter.PopulateConductingEquipmentProperties(der, rd, importHelper, report);

                if (der.ActivePowerHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DER_ACTIVEPOWER, der.ActivePower));
                }
                else
                {
                    report.Report.Append($"{der.GetType().ToString()} will have default value of property \"ActivePower\" = 0\n");
                    rd.AddProperty(new Property(ModelCode.DER_ACTIVEPOWER, 0f));
                }

                if (der.NominalPowerHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DER_NOMINALPOWER, der.NominalPower));
                }

                if (der.SetPointHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DER_SETPOINT, der.SetPoint));
                }
                else
                {
                    report.Report.Append($"{der.GetType().ToString()} will have default value of property \"SetPoint\" = 0\n");
                    rd.AddProperty(new Property(ModelCode.DER_SETPOINT, 0f));
                }
            }
        }

        public static void PopulateEnergyStorageProperties(EnergyStorage energyStorage, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((energyStorage != null) && (rd != null))
            {
                DERMSConverter.PopulateDERProperties(energyStorage, rd, importHelper, report);

                if (energyStorage.CapacityHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_CAPACITY, energyStorage.Capacity));
                }

                if (energyStorage.StateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_STATE, (short)energyStorage.State));
                }
                else
                {
                    report.Report.Append($"EnergyStorage with id = {energyStorage.MRID} will have default value of property \"EnergyStorageState\" = {Common.AbstractModel.EnergyStorageState.Idle}\n");
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_STATE, (short)Common.AbstractModel.EnergyStorageState.Idle));
                }

                if (energyStorage.StateOfChargeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_STATEOFCHARGE, energyStorage.StateOfCharge));
                }
                else
                {
                    report.Report.Append($"EnergyStorage with id = {energyStorage.MRID} will have default value of property \"StateOfCharge\" = 1\n");
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_STATEOFCHARGE, 1f));
                }
            }
        }

        private static void PopulateDERGeneratorProperties(Generator generator, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (generator != null && rd != null)
            {
                DERMSConverter.PopulateDERProperties(generator, rd, importHelper, report);

                if (generator.DeltaPowerHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.GENERATOR_DELTAPOWER, generator.DeltaPower));
                }
                else
                {
                    report.Report.Append($"{generator.GetType().ToString()} with id = {generator.MRID} will have default value of property \"DeltaPower\" = 0\n");
                    rd.AddProperty(new Property(ModelCode.GENERATOR_DELTAPOWER, 0f));
                }

                if (generator.StorageHasValue)
                {
                    long gid = importHelper.GetMappedGID(generator.Storage.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(generator.Storage.GetType().ToString()).Append(" rdfID = \"").Append(generator.ID);
                        report.Report.Append("\" - Failed to set reference to EnergyStorage: rdfID \"").Append(generator.Storage.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.GENERATOR_ENERGYSTORAGE, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(generator.Storage.GetType().ToString()).Append(" rdfID = \"").Append(generator.ID);
                    report.Report.Append("\" - Failed to set reference to EnergyStorage since it is not defined\n");
                }
            }
        }

        public static void PopulateSolarGeneratorProperties(SolarGenerator solarGenerator, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((solarGenerator != null) && (rd != null))
            {
                DERMSConverter.PopulateDERGeneratorProperties(solarGenerator, rd, importHelper, report);
            }
        }

        public static void PopulateWindGeneratorProperties(WindGenerator windGenerator, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((windGenerator != null) && (rd != null))
            {
                DERMSConverter.PopulateDERGeneratorProperties(windGenerator, rd, importHelper, report);

                if (windGenerator.StartUpSpeedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.WINDGENERATOR_STARTUPSPEED, windGenerator.StartUpSpeed));
                }
                if (windGenerator.CutOutSpeedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.WINDGENERATOR_CUTOUTSPEED, windGenerator.CutOutSpeed));
                }
                if (windGenerator.NominalSpeedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.WINDGENERATOR_NOMINALSPEED, windGenerator.NominalSpeed));
                }
            }
        }

        private static void PopulateSwitchProperties(Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (cimSwitch != null && rd != null)
            {
                DERMSConverter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);

                if(cimSwitch.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMALOPEN, cimSwitch.NormalOpen));
                }
            }
        }

        private static void PopulateProtectedSwitchProperties(ProtectedSwitch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (cimSwitch != null && rd != null)
            {
                DERMSConverter.PopulateSwitchProperties(cimSwitch, rd, importHelper, report);
            }
        }

        public static void PopulateBreakerProperties(Breaker breaker, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((breaker != null) && (rd != null))
            {
                DERMSConverter.PopulateProtectedSwitchProperties(breaker, rd, importHelper, report);
            }
        }

        private static void PopulateConductorProperties(Conductor conductor, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (conductor != null && rd != null)
            {
                DERMSConverter.PopulateConductingEquipmentProperties(conductor, rd, importHelper, report);
            }
        }

        public static void PopulateACLineSegmentProperties(ACLineSegment acLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (acLineSegment != null && rd != null)
            {
                DERMSConverter.PopulateConductorProperties(acLineSegment, rd, importHelper, report);
            }
        }

        public static void PopulateConnectivityNodeProperties(ConnectivityNode connectivityNode, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (connectivityNode != null && rd != null)
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(connectivityNode, rd);

                if (connectivityNode.ConnectivityNodeContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(connectivityNode.ConnectivityNodeContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(connectivityNode.ConnectivityNodeContainer.GetType().ToString()).Append(" rdfID = \"").Append(connectivityNode.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNodeContainer: rdfID \"").Append(connectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(connectivityNode.ConnectivityNodeContainer.GetType().ToString()).Append(" rdfID = \"").Append(connectivityNode.ID);
                    report.Report.Append("\" - Failed to set reference to ConnectivityNodeContainer: rdfID \"").Append(connectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                }
            }
        }

        public static void PopulateTerminalProperties(Terminal terminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (terminal != null && rd != null)
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(terminal, rd);

                if (terminal.ConnectivityNodeHasValue)
                {
                    long gid = importHelper.GetMappedGID(terminal.ConnectivityNode.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(terminal.ConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNode: rdfID \"").Append(terminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTIVITYNODE, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(terminal.ConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                    report.Report.Append("\" - Failed to set reference to ConnectivityNode: rdfID \"").Append(terminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                }

                if (terminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(terminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(terminal.ConductingEquipment.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                        report.Report.Append("\" - Failed to set reference to ConductingEquipment: rdfID \"").Append(terminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.TERMINAL_CONDUCTINGEQ, gid));
                    }
                }
                else
                {
                    report.Report.Append("WARNING: Convert ").Append(terminal.ConductingEquipment.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                    report.Report.Append("\" - Failed to set reference to ConductingEquipment: rdfID \"").Append(terminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                }
            }
        }

        public static void PopulateMeasurementProperties(Measurement measurement, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (measurement != null && rd != null)
            {
                DERMSConverter.PopulateIdentifiedObjectProperties(measurement, rd);

                if (measurement.PowerSystemResourceHasValue)
                {
                    long gid = importHelper.GetMappedGID(measurement.PowerSystemResource.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(measurement.PowerSystemResource.GetType().ToString()).Append(" rdfID = \"").Append(measurement.ID);
                        report.Report.Append("\" - Failed to set reference to PowerSystemResource: rdfID \"").Append(measurement.PowerSystemResource.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.MEASUREMENT_PSR, gid));
                    }
                }
                else
                {
                    {
                        report.Report.Append("WARNING: Convert ").Append(measurement.PowerSystemResource.GetType().ToString()).Append(" rdfID = \"").Append(measurement.ID);
                        report.Report.Append("\" - Failed to set reference to PowerSystemResource: rdfID \"").Append(measurement.PowerSystemResource.ID).AppendLine(" \" is not mapped to GID!");
                    }
                }

                if (measurement.TerminalHasValue)
                {
                    long gid = importHelper.GetMappedGID(measurement.Terminal.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(measurement.Terminal.GetType().ToString()).Append(" rdfID = \"").Append(measurement.ID);
                        report.Report.Append("\" - Failed to set reference to Terminal: rdfID \"").Append(measurement.Terminal.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.MEASUREMENT_TERMINAL, gid));
                    }
                }

                if (measurement.MeasurementAddressHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_ADDRESS, measurement.MeasurementAddress));
                }

                if (measurement.DirectionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_DIRECTION, (short)SignalDirectionMap(measurement.Direction)));
                }

                if (measurement.MeasurementTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTYPE, (short)MeasurementTypeMap(measurement.MeasurementType)));
                }
            }
        }

        public static void PopulateDiscreteProperties(Discrete discrete, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (discrete != null && rd != null)
            {
                DERMSConverter.PopulateMeasurementProperties(discrete, rd, importHelper, report);

                if (discrete.MaxValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_MAXVALUE, discrete.MaxValue));
                }

                if (discrete.MinValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_MAXVALUE, discrete.MaxValue));
                }

                if(discrete.CurrentValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE, discrete.CurrentValue));
                }

                rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_DOM, (int)0));
            }
        }

        public static void PopulateAnalogProperties(Analog analog, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if (analog != null && rd != null)
            {
                DERMSConverter.PopulateMeasurementProperties(analog, rd, importHelper, report);

                if (analog.MaxValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_MAXVALUE, analog.MaxValue));
                }

                if (analog.MinValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_MINVALUE, analog.MaxValue));
                }

                if (analog.CurrentValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_CURRENTVALUE, analog.CurrentValue));
                }
            }
        }

        public static void PopulateProperties<T>(T cimObj, ResourceDescription rd, ImportHelper helper, TransformAndLoadReport report)
        {
            if (typeof(T) == typeof(EnergyConsumer))
                PopulateEnergyConsumerProperties(cimObj as EnergyConsumer, rd, helper, report);
            if (typeof(T) == typeof(ACLineSegment))
                PopulateACLineSegmentProperties(cimObj as ACLineSegment, rd, helper, report);
            if (typeof(T) == typeof(Analog))
                PopulateAnalogProperties(cimObj as Analog, rd, helper, report);
            if (typeof(T) == typeof(Breaker))
                PopulateBreakerProperties(cimObj as Breaker, rd, helper, report);
            if (typeof(T) == typeof(ConductingEquipment))
                PopulateConductingEquipmentProperties(cimObj as DERMS.ConductingEquipment, rd, helper, report);
            if (typeof(T) == typeof(Conductor))
                PopulateConductorProperties(cimObj as Conductor, rd, helper, report);
            if (typeof(T) == typeof(ConnectivityNode))
                PopulateConnectivityNodeProperties(cimObj as ConnectivityNode, rd, helper, report);
            if (typeof(T) == typeof(ConnectivityNodeContainer))
                PopulateConnectivityNodeContainerProperties(cimObj as ConnectivityNodeContainer, rd);
            if (typeof(T) == typeof(Generator))
                PopulateDERGeneratorProperties(cimObj as Generator, rd, helper, report);
            if (typeof(T) == typeof(DER))
                PopulateDERProperties(cimObj as DER, rd, helper, report);
            if (typeof(T) == typeof(Discrete))
                PopulateDiscreteProperties(cimObj as Discrete, rd, helper, report);
            if (typeof(T) == typeof(EnergyConsumer))
                PopulateEnergyConsumerProperties(cimObj as EnergyConsumer, rd, helper, report);
            if (typeof(T) == typeof(EnergySource))
                PopulateEnergySourceProperties(cimObj as EnergySource, rd, helper, report);
            if (typeof(T) == typeof(EnergyStorage))
                PopulateEnergyStorageProperties(cimObj as EnergyStorage, rd, helper, report);
            if (typeof(T) == typeof(EquipmentContainer))
                PopulateEquipmentContainerProperties(cimObj as EquipmentContainer, rd);
            if (typeof(T) == typeof(Equipment))
                PopulateEquipmentProperties(cimObj as DERMS.Equipment, rd, helper, report);
            if (typeof(T) == typeof(GeographicalRegion))
                PopulateGeographicalRegionProperties(cimObj as GeographicalRegion, rd);
            if (typeof(T) == typeof(SubGeographicalRegion))
                PopulateSubGeographicalRegionProperties(cimObj as SubGeographicalRegion, rd, helper, report);
            if (typeof(T) == typeof(IdentifiedObject))
                PopulateIdentifiedObjectProperties(cimObj as DERMS.IdentifiedObject, rd);
            if (typeof(T) == typeof(Measurement))
                PopulateMeasurementProperties(cimObj as Measurement, rd, helper, report);
            if (typeof(T) == typeof(PowerSystemResource))
                PopulatePowerSystemResourceProperties(cimObj as DERMS.PowerSystemResource, rd);
            if (typeof(T) == typeof(ProtectedSwitch))
                PopulateProtectedSwitchProperties(cimObj as ProtectedSwitch, rd, helper, report);
            if (typeof(T) == typeof(Substation))
                PopulateSubstationProperties(cimObj as Substation, rd, helper, report);
            if (typeof(T) == typeof(SolarGenerator))
                PopulateSolarGeneratorProperties(cimObj as SolarGenerator, rd, helper, report);
            if (typeof(T) == typeof(Switch))
                PopulateSwitchProperties(cimObj as Switch, rd, helper, report);
            if (typeof(T) == typeof(WindGenerator))
                PopulateWindGeneratorProperties(cimObj as WindGenerator, rd, helper, report);
            if (typeof(T) == typeof(Terminal))
                PopulateTerminalProperties(cimObj as Terminal, rd, helper, report);
        }
                    
        #region Enums convert
        private static Common.AbstractModel.SignalDirection SignalDirectionMap(DERMS.SignalDirection signalDirection)
        {
            switch (signalDirection)
            {
                case DERMS.SignalDirection.Read:
                    return Common.AbstractModel.SignalDirection.Read;
                case DERMS.SignalDirection.ReadWrite:
                    return Common.AbstractModel.SignalDirection.ReadWrite;
                case DERMS.SignalDirection.Write:
                    return Common.AbstractModel.SignalDirection.Write;
                default:
                    return 0;
            }
        }

        private static Common.AbstractModel.MeasurementType MeasurementTypeMap(DERMS.MeasurementType measurementType)
        {
            switch (measurementType)
            {
                case DERMS.MeasurementType.ActiveEnergy:
                    return Common.AbstractModel.MeasurementType.ActiveEnergy;
                case DERMS.MeasurementType.ActivePower:
                    return Common.AbstractModel.MeasurementType.ActivePower;
                case DERMS.MeasurementType.Discrete:
                    return Common.AbstractModel.MeasurementType.Discrete;
                case DERMS.MeasurementType.Humidity:
                    return Common.AbstractModel.MeasurementType.Humidity;
                case DERMS.MeasurementType.Money:
                    return Common.AbstractModel.MeasurementType.Money;
                case DERMS.MeasurementType.Percent:
                    return Common.AbstractModel.MeasurementType.Percet;
                case DERMS.MeasurementType.SkyCover:
                    return Common.AbstractModel.MeasurementType.SkyCover;
                case DERMS.MeasurementType.Status:
                    return Common.AbstractModel.MeasurementType.Status;
                case DERMS.MeasurementType.SunshineMinutes:
                    return Common.AbstractModel.MeasurementType.SunshineMinutes;
                case DERMS.MeasurementType.Temperature:
                    return Common.AbstractModel.MeasurementType.Temperature;
                case DERMS.MeasurementType.Time:
                    return Common.AbstractModel.MeasurementType.Time;
                case DERMS.MeasurementType.Unitless:
                    return Common.AbstractModel.MeasurementType.Unitless;
                case DERMS.MeasurementType.WindSpeed:
                    return Common.AbstractModel.MeasurementType.WindSpeed;
                case DERMS.MeasurementType.DeltaPower:
                    return Common.AbstractModel.MeasurementType.DeltaPower;
                default:
                    return 0;
            }
        }
        #endregion Enums convert
    }
}
