﻿using System;
using Common.SCADA.FieldProcessor;
using FieldProcessor.Model;
using Common.Logger;
using Common.ComponentStorage;

namespace FieldProcessor.CommandingProcessor
{
    public class SingleWriteRegisterRequestCommandCreator : SingleWriteRequestCommandCreator
    {
        public SingleWriteRegisterRequestCommandCreator(IStorage<RemotePoint> storage) : base(storage, ModbusFunctionCode.WriteSingleRegister)
        {
        }

        protected override byte[] GetCommandedValue(ChangeRemotePointValueCommand command, RemotePoint remotePoint)
        {
            if (command == null)
            {
                return null;
            }

            if (remotePoint == null)
            {
                Logger.Instance.Log($"Cannot find entity with gid: 0x{remotePoint.GlobalId:X16}.");
                return null;
            }

            if (remotePoint.Type != RemotePointType.HoldingRegister)
            {
                Logger.Instance.Log($"Cannot command {remotePoint.Type.ToString()} remote point type.");
                return null;
            }

            return BitConverter.GetBytes((short)command.CommandingValue);
        }
    }
}
