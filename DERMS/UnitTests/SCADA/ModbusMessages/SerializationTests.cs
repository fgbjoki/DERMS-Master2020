using FieldProcessor.ExtensionMethods;
using FieldProcessor.ModbusMessages;
using FieldProcessor.Model;
using NUnit.Framework;
using System;
using System.Net;

namespace UnitTests.SCADA.ModbusMessages
{
    [TestFixture]
    public class SerializationTests
    {
        private readonly int transactionIdentifierOffset = 0;
        private readonly int protocolIdentifierOffset = 2;
        private readonly int lengthOffset = 4;
        private readonly int unitIdentifierOffset = 6;
        private readonly int functionCodeOffeset = 7;
        private readonly int startingAddressOffset = 8;
        private readonly int quantityOffset = 10;

        public SerializationTests()
        {

        }

        [TestCase((ushort)1, (ushort)1, (ushort)1, ModbusFunctionCode.ReadCoils)]
        [TestCase((ushort)0x10, (ushort)0x20, (ushort)0x30, ModbusFunctionCode.ReadDiscreteInputs)]
        public void ModbusReadRequestMessage_SerializationTest(ushort expectedStartingAddress, ushort expectedQuantity, ushort expectedTransactionIdentifier, ModbusFunctionCode expectedFunctionCode)
        {
            ushort expectedLength = 6;
            byte expectedUnitIdentifier = 1;
            ushort expectedProtocolIdentifier = 0;

            // Assign
            IRequestMessage requestMessage = new ModbusReadRequestMessage(expectedStartingAddress, expectedQuantity, expectedTransactionIdentifier, expectedFunctionCode);

            // Act
            byte[] rawData = requestMessage.TransfromMessageToBytes();

            ushort transactionIdentifier = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, transactionIdentifierOffset));
            ushort protocolIdentifier = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, protocolIdentifierOffset));
            ushort length = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, lengthOffset));
            byte unitIdentifier = rawData[unitIdentifierOffset];
            ModbusFunctionCode functionCode = (ModbusFunctionCode)rawData[functionCodeOffeset];
            ushort startingAddress = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, startingAddressOffset));
            ushort quantity = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, quantityOffset));

            // Assert
            Assert.AreEqual(expectedTransactionIdentifier, transactionIdentifier);
            Assert.AreEqual(expectedProtocolIdentifier, protocolIdentifier);
            Assert.AreEqual(expectedLength, length);
            Assert.AreEqual(expectedUnitIdentifier, unitIdentifier);
            Assert.AreEqual(expectedFunctionCode, functionCode);
            Assert.AreEqual(expectedStartingAddress, startingAddress);
            Assert.AreEqual(expectedQuantity, quantity);
        }

        [TestCase((ushort)1, (ushort)1, (ushort)1, ModbusFunctionCode.WriteSingleCoil)]
        [TestCase((ushort)0x10, (ushort)0x21, (ushort)0x31, ModbusFunctionCode.WriteSingleRegister)]
        public void ModbusSingleWriteMessage_RequestSerializationTest(ushort expectedRemotePointAddress, ushort expectedRemotePointValue, ushort expectedTransactionIdentifier, ModbusFunctionCode expectedFunctionCode)
        {
            ushort expectedLength = 6;
            byte expectedUnitIdentifier = 1;
            ushort expectedProtocolIdentifier = 0;

            // Assign
            IRequestMessage requestMessage = new ModbusSingleWriteMessage(expectedRemotePointAddress, expectedRemotePointValue, expectedTransactionIdentifier, expectedFunctionCode);

            // Act
            byte[] rawData = requestMessage.TransfromMessageToBytes();

            ushort transactionIdentifier = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, transactionIdentifierOffset));
            ushort protocolIdentifier = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, protocolIdentifierOffset));
            ushort length = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, lengthOffset));
            byte unitIdentifier = rawData[unitIdentifierOffset];
            ModbusFunctionCode functionCode = (ModbusFunctionCode)rawData[functionCodeOffeset];
            ushort remotePointAddress = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, startingAddressOffset));
            ushort remotePointValue = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(rawData, quantityOffset));

            //Assert
            Assert.AreEqual(expectedTransactionIdentifier, transactionIdentifier);
            Assert.AreEqual(expectedProtocolIdentifier, protocolIdentifier);
            Assert.AreEqual(expectedLength, length);
            Assert.AreEqual(expectedUnitIdentifier, unitIdentifier);
            Assert.AreEqual(expectedFunctionCode, functionCode); ;
            Assert.AreEqual(expectedRemotePointAddress, remotePointAddress);
            Assert.AreEqual(expectedRemotePointValue, remotePointValue);
        }

        [TestCaseSource("ModbusReadResponseMessage_SerializationTestCases")]
        public void ModbusReadResponseMessage_SerializationTest(byte expectedByteCount, byte[] expectedValues, ushort expectedTransactionIdentifier, ModbusFunctionCode expectedFunctionCode)
        {
            ushort expectedLength = sizeof(byte) * 2;
            expectedLength += (ushort)expectedValues.Length;

            byte expectedUnitIdentifier = 1;
            ushort expectedProtocolIdentifier = 0;

            // Assign
            byte[] rawData = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedTransactionIdentifier)).
                Append(BitConverter.GetBytes(expectedProtocolIdentifier)).
                Append(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedLength))).
                Append(expectedUnitIdentifier).
                Append((byte)expectedFunctionCode).
                Append(expectedByteCount).
                Append(expectedValues);

            //Act
            IResponseMessage responseMessage = new ModbusReadResponseMessage();
            responseMessage.ConvertMessageFromBytes(rawData);

            ModbusReadResponseMessage modbusMessage = (ModbusReadResponseMessage)responseMessage;

            //Assert
            Assert.AreEqual(expectedTransactionIdentifier, modbusMessage.TransactionIdentifier);
            Assert.AreEqual(expectedProtocolIdentifier, modbusMessage.ProtocolIdentifier);
            Assert.AreEqual(expectedLength, modbusMessage.Length);
            Assert.AreEqual(expectedUnitIdentifier, modbusMessage.UnitIdentifier);
            Assert.AreEqual(expectedFunctionCode, modbusMessage.FunctionCode);
            Assert.AreEqual(expectedByteCount, modbusMessage.ByteCount);
            CollectionAssert.AreEqual(expectedValues, modbusMessage.Values);
        }

        [TestCaseSource("ModbusSingleWriteMessage_ResponseSerializationTestCases")]
        public void ModbusSingleWriteMessage_ResponseSerializationTest(ushort expectedRemotePointAddress, ushort expectedRemotePointValue, ushort expectedTransactionIdentifier, ModbusFunctionCode expectedFunctionCode)
        {
            ushort expectedLength = sizeof(byte) * 2;
            expectedLength += sizeof(ushort) * 2;

            byte expectedUnitIdentifier = 1;
            ushort expectedProtocolIdentifier = 0;

            //Assign
            byte[] rawData = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedTransactionIdentifier)).
                Append(BitConverter.GetBytes(expectedProtocolIdentifier)).
                Append(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedLength))).
                Append(expectedUnitIdentifier).
                Append((byte)expectedFunctionCode).
                Append(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedRemotePointAddress))).
                Append(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)expectedRemotePointValue)));

            //Act
            IResponseMessage responseMessage = new ModbusSingleWriteMessage();
            responseMessage.ConvertMessageFromBytes(rawData);

            ModbusSingleWriteMessage modbusMessage = (ModbusSingleWriteMessage)responseMessage;

            //Assert
            Assert.AreEqual(expectedTransactionIdentifier, modbusMessage.TransactionIdentifier);
            Assert.AreEqual(expectedProtocolIdentifier, modbusMessage.ProtocolIdentifier);
            Assert.AreEqual(expectedLength, modbusMessage.Length);
            Assert.AreEqual(expectedUnitIdentifier, modbusMessage.UnitIdentifier);
            Assert.AreEqual(expectedFunctionCode, modbusMessage.FunctionCode);
            Assert.AreEqual(expectedRemotePointAddress, modbusMessage.RemotePointAddress);
            Assert.AreEqual(expectedRemotePointValue, modbusMessage.RemotePointValue);
        }

        private static object[] ModbusReadResponseMessage_SerializationTestCases =
        {
            new object[] { (byte)2, new byte[2] { 1, 1, }, (ushort)1 , ModbusFunctionCode.ReadCoils },
            new object[] { (byte)3, new byte[3] { 1, 1, 0 }, (ushort)1 , ModbusFunctionCode.ReadDiscreteInputs },
        };

        private static object[] ModbusSingleWriteMessage_ResponseSerializationTestCases =
        {
            new object[] { (ushort)2, (ushort)2, (ushort)1 , ModbusFunctionCode.WriteSingleCoil },
            new object[] { (ushort)0x12, (ushort)0x11, (ushort)0x13, ModbusFunctionCode.WriteSingleRegister },
        };
    }
}
