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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(subGeographicalRegion.Region.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.SUBGEOGRAPHICALREGION_REGION, gid));
                    }
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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(substation.Region.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.SUBGEOGRAPHICALREGION_REGION, gid));
                    }
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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(equipment.EquipmentContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.EQUIPMENT_EQCONTAINER, gid));
                    }
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

                if (der.NominalPowerHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DER_NOMINALPOWER, der.NominalPower));
                }

                if (der.SetPointHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DER_SETPOINT, der.SetPoint));
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

                if (energyStorage.StateOfChargeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ENERGYSTORAGE_CAPACITY, energyStorage.StateOfCharge));
                }

                if (energyStorage.GeneratorHasValue)
                {
                    importHelper.AddReferenceToMissingResourceDescription(rd, ModelCode.ENERGYSTORAGE_GENERATOR, energyStorage.Generator.ID);
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

                if (generator.StorageHasValue)
                {
                    long gid = importHelper.GetMappedGID(generator.Storage.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(generator.Storage.GetType().ToString()).Append(" rdfID = \"").Append(generator.ID);
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(generator.Storage.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.GENERATOR_ENERGYSTORAGE, gid));
                    }
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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(connectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER, gid));
                    }
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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(terminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTIVITYNODE, gid));
                    }
                }

                if (terminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(terminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(terminal.ConductingEquipment.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(terminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.TERMINAL_CONDUCTINGEQ, gid));
                    }
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
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(measurement.PowerSystemResource.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    else
                    {
                        rd.AddProperty(new Property(ModelCode.MEASUREMENT_PSR, gid));
                    }
                }

                if (measurement.TerminalHasValue)
                {
                    long gid = importHelper.GetMappedGID(measurement.Terminal.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(measurement.Terminal.GetType().ToString()).Append(" rdfID = \"").Append(measurement.ID);
                        report.Report.Append("\" - Failed to set reference to BaseVoltage: rdfID \"").Append(measurement.Terminal.ID).AppendLine(" \" is not mapped to GID!");
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
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_DIRECTION, (short)measurement.Direction));
                }

                if (measurement.MeasurementTypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_MEASUREMENTYPE, (short)measurement.MeasurementType));
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

                if (discrete.CurrentOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_CURRENTOPEN, discrete.CurrentOpen));
                }

                if (discrete.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_NORMALOPEN, discrete.NormalOpen));
                }
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
                    rd.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_CURRENTVALUE, analog.CurrentValueHasValue));
                }
            }
        }

        #region Enums convert
        // TODO

        //public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
        //{
        //	switch (phases)
        //	{
        //		case FTN.PhaseCode.A:
        //			return PhaseCode.A;
        //		case FTN.PhaseCode.AB:
        //			return PhaseCode.AB;
        //		case FTN.PhaseCode.ABC:
        //			return PhaseCode.ABC;
        //		case FTN.PhaseCode.ABCN:
        //			return PhaseCode.ABCN;
        //		case FTN.PhaseCode.ABN:
        //			return PhaseCode.ABN;
        //		case FTN.PhaseCode.AC:
        //			return PhaseCode.AC;
        //		case FTN.PhaseCode.ACN:
        //			return PhaseCode.ACN;
        //		case FTN.PhaseCode.AN:
        //			return PhaseCode.AN;
        //		case FTN.PhaseCode.B:
        //			return PhaseCode.B;
        //		case FTN.PhaseCode.BC:
        //			return PhaseCode.BC;
        //		case FTN.PhaseCode.BCN:
        //			return PhaseCode.BCN;
        //		case FTN.PhaseCode.BN:
        //			return PhaseCode.BN;
        //		case FTN.PhaseCode.C:
        //			return PhaseCode.C;
        //		case FTN.PhaseCode.CN:
        //			return PhaseCode.CN;
        //		case FTN.PhaseCode.N:
        //			return PhaseCode.N;
        //		case FTN.PhaseCode.s12N:
        //			return PhaseCode.ABN;
        //		case FTN.PhaseCode.s1N:
        //			return PhaseCode.AN;
        //		case FTN.PhaseCode.s2N:
        //			return PhaseCode.BN;
        //		default: return PhaseCode.Unknown;
        //	}
        //}

        //public static TransformerFunction GetDMSTransformerFunctionKind(FTN.TransformerFunctionKind transformerFunction)
        //{
        //	switch (transformerFunction)
        //	{
        //		case FTN.TransformerFunctionKind.voltageRegulator:
        //			return TransformerFunction.Voltreg;
        //		default:
        //			return TransformerFunction.Consumer;
        //	}
        //}

        //public static WindingType GetDMSWindingType(FTN.WindingType windingType)
        //{
        //	switch (windingType)
        //	{
        //		case FTN.WindingType.primary:
        //			return WindingType.Primary;
        //		case FTN.WindingType.secondary:
        //			return WindingType.Secondary;
        //		case FTN.WindingType.tertiary:
        //			return WindingType.Tertiary;
        //		default:
        //			return WindingType.None;
        //	}
        //}

        //public static WindingConnection GetDMSWindingConnection(FTN.WindingConnection windingConnection)
        //{
        //	switch (windingConnection)
        //	{
        //		case FTN.WindingConnection.D:
        //			return WindingConnection.D;
        //		case FTN.WindingConnection.I:
        //			return WindingConnection.I;
        //		case FTN.WindingConnection.Z:
        //			return WindingConnection.Z;
        //		case FTN.WindingConnection.Y:
        //			return WindingConnection.Y;
        //		default:
        //			return WindingConnection.Y;
        //	}
        //}
        #endregion Enums convert
    }
}
