using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace UnitTests.SCADA.ModbusMessages
{
    [TestFixture]
    public class ModbusMessageCreationTests
    {
        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadCoils)]
        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadDiscreteInputs)]
        public void ModbusReadDigitalRequestMessage_ConstructorValidTest(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            // Assign & Act & Assert
            ModbusReadRequestMessage digitalRequest = new ModbusReadDigitalRequestMessage(startingAddress, quantity, transactionIdentifier, functionCode);
        }

        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadInputRegisters)]
        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadHoldingRegisters)]
        public void ModbusReadDigitalRequestMessage_ConstructorShouldThrowExceptionTest(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            // Assign & Act & Assert
            ModbusReadRequestMessage digitalRequest;
            Assert.Throws<ArgumentException>(() => digitalRequest = new ModbusReadDigitalRequestMessage(startingAddress, quantity, transactionIdentifier, functionCode));
        }

        [TestCase((ushort)1, (byte)1)]
        [TestCase((ushort)8, (byte)1)]
        [TestCase((ushort)9, (byte)2)]
        [TestCase((ushort)16, (byte)2)]
        [TestCase((ushort)17, (byte)3)]
        public void ModbusReadDigitalRequestMessage_ValidationSuccessfulTest(ushort quantity, byte expectedByteCount)
        {
            // Assign
            short length = (short)(3 + expectedByteCount);
            int rawDataSize = 8 + expectedByteCount;
            ushort transactionIdentifier = 1;
            ModbusFunctionCode functionCode = ModbusFunctionCode.ReadCoils;

            IEnumerable<byte> rawResponse = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)transactionIdentifier)).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)0))).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length))).Append(
                (byte)1).Append(
                (byte)functionCode).Append(
                (byte)Math.Ceiling((double)quantity / 8));

            for (int i = 0; i < expectedByteCount; i++)
            {
                rawResponse = rawResponse.Append((byte)0);
            }

            ModbusReadResponseMessage responseMessage = new ModbusReadResponseMessage();
            responseMessage.ConvertMessageFromBytes(rawResponse.ToArray());

            ModbusReadRequestMessage digitalRequest = new ModbusReadDigitalRequestMessage(0, quantity, transactionIdentifier, functionCode);

            // Act
            bool isValid = digitalRequest.ValidateResponse(responseMessage);

            // Assert
            Assert.True(isValid);
        }

        [TestCase((ushort)1, (byte)4)]
        [TestCase((ushort)8, (byte)2)]
        [TestCase((ushort)9, (byte)3)]
        [TestCase((ushort)16, (byte)4)]
        [TestCase((ushort)17, (byte)5)]
        public void ModbusReadDigitalRequestMessage_ValidationFailedTest(ushort quantity, byte byteCount)
        {
            // Assign
            short length = (short)(3 + byteCount);
            int rawDataSize = 8 + byteCount;
            ushort transactionIdentifier = 1;
            ModbusFunctionCode functionCode = ModbusFunctionCode.ReadCoils;

            IEnumerable<byte> rawResponse = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)transactionIdentifier)).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)0))).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length))).Append(
                (byte)1).Append(
                (byte)functionCode).Append(
                (byte)Math.Ceiling((double)byteCount));

            for (int i = 0; i < byteCount; i++)
            {
                rawResponse = rawResponse.Append((byte)0);
            }

            ModbusReadResponseMessage responseMessage = new ModbusReadResponseMessage();
            responseMessage.ConvertMessageFromBytes(rawResponse.ToArray());

            ModbusReadRequestMessage digitalRequest = new ModbusReadDigitalRequestMessage(0, quantity, transactionIdentifier, functionCode);

            // Act
            bool isValid = digitalRequest.ValidateResponse(responseMessage);

            // Assert
            Assert.False(isValid);
        }

        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadInputRegisters)]
        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadHoldingRegisters)]
        public void ModbusReadAnalogRequestMessage_ConstructorValidTest(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            // Assign & Act & Assert
            ModbusReadRequestMessage analogRequest = new ModbusReadAnalogRequestMessage(startingAddress, quantity, transactionIdentifier, functionCode);
        }

        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadCoils)]
        [TestCase((ushort)0, (ushort)1, (ushort)1, ModbusFunctionCode.ReadDiscreteInputs)]
        public void ModbusReadAnalogRequestMessage_ConstructorShouldThrowExceptionTest(ushort startingAddress, ushort quantity, ushort transactionIdentifier, ModbusFunctionCode functionCode)
        {
            // Assign & Act & Assert
            ModbusReadRequestMessage analogRequest;
            Assert.Throws<ArgumentException>(() => analogRequest = new ModbusReadAnalogRequestMessage(startingAddress, quantity, transactionIdentifier, functionCode));
        }

        [TestCase((ushort)1, (byte)2)]
        [TestCase((ushort)2, (byte)4)]
        [TestCase((ushort)3, (byte)6)]
        [TestCase((ushort)10, (byte)20)]
        public void ModbusReadAnalogRequestMessage_ValidationSuccessfulTest(ushort quantity, byte expectedByteCount)
        {
            // Assign
            short length = (short)(3 + expectedByteCount);
            int rawDataSize = 8 + expectedByteCount;
            ushort transactionIdentifier = 1;
            byte[] values = new byte[quantity];
            ModbusFunctionCode functionCode = ModbusFunctionCode.ReadHoldingRegisters;

            IEnumerable<byte> rawResponse = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)transactionIdentifier)).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)0))).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length))).Append(
                (byte)1).Append(
                (byte)functionCode).Append(
                (byte)Math.Ceiling((double)quantity * 2));

            for (int i = 0; i < expectedByteCount; i++)
            {
                rawResponse = rawResponse.Append((byte)0);
            }

            ModbusReadResponseMessage responseMessage = new ModbusReadResponseMessage();
            responseMessage.ConvertMessageFromBytes(rawResponse.ToArray());

            ModbusReadRequestMessage analogRequest = new ModbusReadAnalogRequestMessage(0, quantity, transactionIdentifier, functionCode);

            // Act
            bool isValid = analogRequest.ValidateResponse(responseMessage);

            // Assert
            Assert.True(isValid);
        }

        [TestCase((ushort)1, (byte)1)]
        [TestCase((ushort)2, (byte)3)]
        [TestCase((ushort)3, (byte)5)]
        [TestCase((ushort)10, (byte)21)]
        public void ModbusReadAnalogRequestMessage_ValidationFailedTest(ushort quantity, byte expectedByteCount)
        {
            // Assign
            short length = (short)(3 + expectedByteCount);
            int rawDataSize = 8 + expectedByteCount;
            ushort transactionIdentifier = 1;
            ModbusFunctionCode functionCode = ModbusFunctionCode.ReadHoldingRegisters;

            IEnumerable<byte> rawResponse = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)transactionIdentifier)).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)0))).Concat(
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length))).Append(
                (byte)1).Append(
                (byte)functionCode).Append(
                (byte)Math.Ceiling((double)expectedByteCount));

            for (int i = 0; i < expectedByteCount; i++)
            {
                rawResponse = rawResponse.Append((byte)0);
            }

            ModbusReadResponseMessage responseMessage = new ModbusReadResponseMessage();
            responseMessage.ConvertMessageFromBytes(rawResponse.ToArray());

            ModbusReadRequestMessage analogRequest = new ModbusReadAnalogRequestMessage(0, quantity, transactionIdentifier, functionCode);

            // Act
            bool isValid = analogRequest.ValidateResponse(responseMessage);

            // Assert
            Assert.False(isValid);
        }
    }
}
