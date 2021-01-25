using System;
using System.Collections.Generic;
using FieldProcessor.Model;
using FieldProcessor.RemotePointAddressCollector;
using FieldProcessor.ModbusMessages;
using System.Timers;
using FieldProcessor.MessageValidation;
using FieldProcessor.SimulatorState;

namespace FieldProcessor.PollingRequestCreator
{
    public class PollingInvoker : IDisposable
    {
        private readonly int acquisitionTimer = 10000; // 2 seconds

        private Dictionary<RemotePointType, ModbusReadRequestMessage> remotePointToMessageMapper;

        private IRemotePointRangeAddressCollector remotePointAddressRangeCollector;
        private ICommandSender commandSender;

        private ISimulatorState simulatorStateNotifier;

        private Timer pollTimer;

        public PollingInvoker(IRemotePointRangeAddressCollector remotePointAddressRangeCollector, ICommandSender commandSender, ISimulatorState simulatorStateNotifier)
        {
            this.remotePointAddressRangeCollector = remotePointAddressRangeCollector;
            this.commandSender = commandSender;

            this.simulatorStateNotifier = simulatorStateNotifier;
            simulatorStateNotifier.ConnectedEvent += SimulatorConnectedHandler;
            simulatorStateNotifier.DisconnectedEvent += SimulatorDisconnectedHandler;

            remotePointToMessageMapper = new Dictionary<RemotePointType, ModbusReadRequestMessage>()
            {
                { RemotePointType.Coil, new ModbusReadDigitalRequestMessage(0, 0, 0, ModbusFunctionCode.ReadCoils) },
                { RemotePointType.DiscreteInput, new ModbusReadDigitalRequestMessage(0, 0, 0, ModbusFunctionCode.ReadDiscreteInputs) },
                { RemotePointType.HoldingRegister, new ModbusReadAnalogRequestMessage(0, 0, 0, ModbusFunctionCode.ReadHoldingRegisters) },
                { RemotePointType.InputRegister, new ModbusReadAnalogRequestMessage(0, 0, 0, ModbusFunctionCode.ReadInputRegisters) },
            };

            pollTimer = new Timer(acquisitionTimer);
            pollTimer.Elapsed += CreatePollRequests;
            pollTimer.AutoReset = true;
            pollTimer.Enabled = false;
        }

        private void SimulatorDisconnectedHandler()
        {
            pollTimer.Stop();
        }

        private void SimulatorConnectedHandler()
        {
            pollTimer.Start();
        }

        private void CreatePollRequests(object sender, ElapsedEventArgs e)
        {
            pollTimer.Enabled = false;
            foreach (var pair in remotePointToMessageMapper)
            {
                List<AddressRange> addressRanges = remotePointAddressRangeCollector.GetAddressRanges(pair.Key);

                foreach (AddressRange addressRange in addressRanges)
                {
                    if (addressRange.RangeSize == 0)
                    {
                        continue;
                    }

                    ModbusReadRequestMessage requestMessage = remotePointToMessageMapper[pair.Key];

                    requestMessage.StartingAddress = addressRange.StartAddress;
                    requestMessage.Quantity = addressRange.RangeSize;
                    requestMessage.TransactionIdentifier = 0;
                    
                    commandSender.SendCommand(requestMessage);
                }
            }

            pollTimer.Enabled = true;
        }

        public void Dispose()
        {
            simulatorStateNotifier.ConnectedEvent -= SimulatorConnectedHandler;
            simulatorStateNotifier.DisconnectedEvent -= SimulatorDisconnectedHandler;

            pollTimer.Stop();
            pollTimer.Elapsed -= CreatePollRequests;
        }
    }
}
