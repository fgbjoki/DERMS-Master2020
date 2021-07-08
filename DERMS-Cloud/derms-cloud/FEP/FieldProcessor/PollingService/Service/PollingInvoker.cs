using Core.Common.AbstractModel;
using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.ModbusMessages.RequestMessages;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using PollingService.Service.RemotePointAddressCollector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace PollingService.Service
{
    public class PollingInvoker : IPollingInvoker
    {
        private static readonly string aquisitionConfigurationName = "AquisitionInterval";

        private readonly IFEPStorage fepStorage;
        private readonly ICommandSender commandSender;
        private readonly IRemotePointRangeAddressCollector remotePointRangeAddressCollector;

        private readonly Dictionary<RemotePointType, ModbusReadRequestMessage> remotePointToMessageMapper;

        private Action<string> logAction;

        public PollingInvoker(IFEPStorage fepStorage, ICommandSender commandSender, Action<string> logAction)
        {
            this.fepStorage = fepStorage;
            this.commandSender = commandSender;
            this.logAction = logAction;

            remotePointRangeAddressCollector = new RemotePointRangeAddressCollector();

            remotePointToMessageMapper = new Dictionary<RemotePointType, ModbusReadRequestMessage>()
            {
                { RemotePointType.Coil, new ModbusReadDigitalRequestMessage(0, 0, 0, ModbusFunctionCode.ReadCoils) },
                { RemotePointType.DiscreteInput, new ModbusReadDigitalRequestMessage(0, 0, 0, ModbusFunctionCode.ReadDiscreteInputs) },
                { RemotePointType.HoldingRegister, new ModbusReadAnalogRequestMessage(0, 0, 0, ModbusFunctionCode.ReadHoldingRegisters) },
                { RemotePointType.InputRegister, new ModbusReadAnalogRequestMessage(0, 0, 0, ModbusFunctionCode.ReadInputRegisters) },
            };
        }

        public async Task StartAquisition(CancellationToken cancellationToken)
        {
            var aquisitionInterval = GetAquisitionInterval();
            List<DMSType> neededDMSTypes = new List<DMSType>(2) { DMSType.MEASUREMENTANALOG, DMSType.MEASUREMENTDISCRETE };

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(aquisitionInterval);
                Log("Acquisition started...");

                List<RemotePoint> remotePoints = null;
                try
                {
                    remotePoints = await fepStorage.GetEntities(neededDMSTypes);
                }
                catch (Exception e)
                {
                    Log(e.Message);
                    continue;
                }

                if (remotePoints?.Count == 0)
                {
                    Log("Aquisition aborted, there aren't any remote points available.");
                    continue;
                }

                Dictionary<RemotePointType, List<AddressRange>> addressRanges = remotePointRangeAddressCollector.CreateAddressRanges(remotePoints);
                PerformAcquisition(addressRanges);

                Log("Acquisition finished...");
            }
        }

        private void PerformAcquisition(Dictionary<RemotePointType, List<AddressRange>> addressRanges)
        {
            foreach (var pair in addressRanges)
            { 
                foreach (AddressRange addressRange in pair.Value)
                {
                    if (addressRange.RangeSize == 0)
                    {
                        continue;
                    }

                    ModbusReadRequestMessage requestMessage = remotePointToMessageMapper[pair.Key];

                    requestMessage.StartingAddress = addressRange.StartAddress;
                    requestMessage.Quantity = addressRange.RangeSize;
                    requestMessage.TransactionIdentifier = 0;


                    bool successful;
                    try
                    {
                        successful = commandSender.SendCommand(requestMessage);
                    }
                    catch (Exception e)
                    {
                        successful = false;
                    }

                    if (!successful)
                    {
                        Log($"Acquisition failed on DMSType: {pair.Key}");
                    }
                    else
                    {
                        Log($"Scan command for DMSType: {pair.Key} successfuly sent.");
                    }
                }
            }
        }

        private int GetAquisitionInterval()
        {
            int acquisitionInterval;
            if (!int.TryParse(ConfigurationManager.AppSettings[aquisitionConfigurationName], out acquisitionInterval))
            {
                acquisitionInterval = 5;
            }

            return acquisitionInterval * 1000;
        }

        private void Log(string text)
        {
            logAction(text);
        }
    }
}
