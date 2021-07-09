using Core.Common.ReliableCollectionProxy;
using Microsoft.ServiceFabric.Data;
using System;

namespace MessageAggregatorService.MessageAggregator.MessageAggregation
{
    public class CircularMemoryBuffer
    {
        private static readonly string headVariable = "head";
        private static readonly string tailVariable = "tail";
        private static readonly string capacityVariable = "capacity";
        private static readonly string arrayVariable = "array";
        private static readonly string sizeVariable = "Size";

        private readonly IReliableStateManager stateManager;

        public CircularMemoryBuffer(int capacity, IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;

            ReliableVariableProxy.AddVariable(stateManager, 0, headVariable);
            ReliableVariableProxy.AddVariable(stateManager, 0, tailVariable);
            ReliableVariableProxy.AddVariable(stateManager, 0, sizeVariable);
            ReliableVariableProxy.AddVariable(stateManager, capacity, capacityVariable);
            ReliableVariableProxy.AddVariable(stateManager, new byte[capacity], arrayVariable);
        }

        public int Size { get; private set; }

        public void Put(byte[] data, int count)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                if (count <= 0)
                {
                    return;
                }

                int capacity = ReliableVariableProxy.GetVariable<int>(stateManager, capacityVariable, tx);

                if (capacity - Size < count)
                {
                    throw new ArgumentOutOfRangeException("data", "There is not enough space in the circular buffer.");
                }

                int remainingBytesToSave = count;
                while (remainingBytesToSave > 0)
                {
                    int tail = ReliableVariableProxy.GetVariable<int>(stateManager, tailVariable, tx);
                    int head = ReliableVariableProxy.GetVariable<int>(stateManager, headVariable, tx);
                    int continualSpace = capacity - tail - remainingBytesToSave;
                    int bytesSaved = 0;

                    if (IsPartialManipulation(tail >= head, continualSpace))
                    {
                        bytesSaved = remainingBytesToSave;
                        byte[] array = ReliableVariableProxy.GetVariable<byte[]>(stateManager, arrayVariable, tx);
                        CopyBlock(data, count - remainingBytesToSave, array, tail, remainingBytesToSave, ref tail, tx);
                        ReliableVariableProxy.SetVariable(stateManager, tail, tailVariable, tx);
                        ReliableVariableProxy.SetVariable(stateManager, array, arrayVariable, tx);
                    }
                    else
                    {
                        bytesSaved = capacity - tail;
                        byte[] array = ReliableVariableProxy.GetVariable<byte[]>(stateManager, arrayVariable);
                        CopyBlock(data, count - remainingBytesToSave, array, tail, capacity - tail, ref tail, tx);
                        ReliableVariableProxy.SetVariable(stateManager, tail, tailVariable, tx);
                        ReliableVariableProxy.SetVariable(stateManager, array, arrayVariable, tx);
                    }

                    remainingBytesToSave -= bytesSaved;
                }

                int size = ReliableVariableProxy.GetVariable<int>(stateManager, sizeVariable, tx);
                size += count;
                ReliableVariableProxy.SetVariable(stateManager, size, sizeVariable);

                tx.CommitAsync().GetAwaiter().GetResult();
            }
        }

        public byte[] Get(int amount)
        {
            using (var tx = stateManager.CreateTransaction())
            {
                int size = ReliableVariableProxy.GetVariable<int>(stateManager, sizeVariable, tx);
                if (size == 0)
                {
                    return new byte[0];
                }

                if (amount > size)
                {
                    throw new ArgumentOutOfRangeException("amount", "Given amount was larger than the collection size");
                }

                byte[] returnValue = new byte[amount];
                int remainingBytesToRead = amount;

                while (remainingBytesToRead > 0)
                {
                    int capacity = ReliableVariableProxy.GetVariable<int>(stateManager, capacityVariable, tx);
                    int tail = ReliableVariableProxy.GetVariable<int>(stateManager, tailVariable, tx);
                    int head = ReliableVariableProxy.GetVariable<int>(stateManager, headVariable, tx);

                    int continualSpace = capacity - head - remainingBytesToRead;
                    int bytesSaved = 0;

                    if (IsPartialManipulation(head > tail, continualSpace))
                    {
                        bytesSaved = remainingBytesToRead;
                        head = ReliableVariableProxy.GetVariable<int>(stateManager, headVariable, tx);
                        byte[] array = ReliableVariableProxy.GetVariable<byte[]>(stateManager, arrayVariable, tx);
                        CopyBlock(array, head, returnValue, returnValue.Length - remainingBytesToRead, remainingBytesToRead, ref head, tx);
                        ReliableVariableProxy.SetVariable(stateManager, head, headVariable);
                    }
                    else
                    {
                        bytesSaved = capacity - head;
                        head = ReliableVariableProxy.GetVariable<int>(stateManager, headVariable, tx);
                        byte[] array = ReliableVariableProxy.GetVariable<byte[]>(stateManager, arrayVariable, tx);
                        CopyBlock(array, head, returnValue, returnValue.Length - remainingBytesToRead, capacity - head, ref head, tx);
                        ReliableVariableProxy.SetVariable(stateManager, head, headVariable);
                    }

                    remainingBytesToRead -= bytesSaved;
                }

                size = ReliableVariableProxy.GetVariable<int>(stateManager, sizeVariable, tx);
                size += amount;
                ReliableVariableProxy.SetVariable(stateManager, size, sizeVariable);

                tx.CommitAsync().GetAwaiter().GetResult();

                return returnValue;
            }
        }

        private void CopyBlock(Array src, int offsetSrc, Array dest, int offsetDest, int count, ref int pointer, ITransaction tx)
        {
            Buffer.BlockCopy(src, offsetSrc, dest, offsetDest, count);

            pointer += count;
            Rewind(ref pointer, tx);
        }

        private void Rewind(ref int pointer, ITransaction tx)
        {
            int capacity = ReliableVariableProxy.GetVariable<int>(stateManager, capacityVariable, tx);
            if (pointer == capacity)
            {
                pointer = 0;
            }
        }

        private bool IsPartialManipulation(bool pointerCondition, int continualSpace)
        {
            // NAND logic
            return !(pointerCondition & continualSpace < 0);
        }
    }
}