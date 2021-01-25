using System;

namespace FieldProcessor.TCPCommunicationHandler.Collection
{
    public class CircularMemoryBuffer
    {
        private int head;
        private int tail;

        private int capacity;

        private byte[] array;

        public CircularMemoryBuffer(int capacity)
        {
            head = 0;
            tail = 0;

            this.capacity = capacity;

            array = new byte[capacity];
        }

        public int Size { get; private set; }

        public void Put(byte[] data)
        {
            if (data.Length == 0)
            {
                return;
            }

            if (capacity - Size < data.Length)
            {
                throw new ArgumentOutOfRangeException("data", "There is not enough space in the circular buffer.");
            }

            int remainingBytesToSave = data.Length;
            while (remainingBytesToSave > 0)
            {
                int continualSpace = capacity - tail - remainingBytesToSave;
                int bytesSaved = 0;

                if (IsPartialManipulation(tail >= head, continualSpace))
                {
                    bytesSaved = remainingBytesToSave;
                    CopyBlock(data, data.Length - remainingBytesToSave, array, tail, remainingBytesToSave, ref tail);
                }
                else
                {
                    bytesSaved = capacity - tail;
                    CopyBlock(data, data.Length - remainingBytesToSave, array, tail, capacity - tail, ref tail);
                }
                remainingBytesToSave -= bytesSaved;
            }

            Size += data.Length;
        }

        public void Put(byte[] data, int count)
        {
            if (count <= 0)
            {
                return;
            }

            if (capacity - Size < count)
            {
                throw new ArgumentOutOfRangeException("data", "There is not enough space in the circular buffer.");
            }

            int remainingBytesToSave = count;
            while (remainingBytesToSave > 0)
            {
                int continualSpace = capacity - tail - remainingBytesToSave;
                int bytesSaved = 0;

                if (IsPartialManipulation(tail >= head, continualSpace))
                {
                    bytesSaved = remainingBytesToSave;
                    CopyBlock(data, count - remainingBytesToSave, array, tail, remainingBytesToSave, ref tail);
                }
                else
                {
                    bytesSaved = capacity - tail;
                    CopyBlock(data, count - remainingBytesToSave, array, tail, capacity - tail, ref tail);
                }
                remainingBytesToSave -= bytesSaved;
            }

            Size += count;
        }

        public byte[] Get(int amount)
        {
            if (Size == 0)
            {
                return new byte[0];
            }

            if (amount > Size)
            {
                throw new ArgumentOutOfRangeException("amount", "Given amount was larger than the collection size");
            }

            byte[] returnValue = new byte[amount];
            int remainingBytesToRead = amount;

            while (remainingBytesToRead > 0)
            {
                int continualSpace = capacity - head - remainingBytesToRead;
                int bytesSaved = 0;

                if (IsPartialManipulation(head > tail,continualSpace))
                {
                    bytesSaved = remainingBytesToRead;
                    CopyBlock(array, head, returnValue, returnValue.Length - remainingBytesToRead, remainingBytesToRead, ref head);
                }
                else
                {
                    bytesSaved = capacity - head;
                    CopyBlock(array, head, returnValue, returnValue.Length - remainingBytesToRead, capacity - head, ref head);
                }

                remainingBytesToRead -= bytesSaved;
            }

            Size -= amount;

            return returnValue;
        }

        private void CopyBlock(Array src, int offsetSrc, Array dest, int offsetDest, int count, ref int pointer)
        {
            Buffer.BlockCopy(src, offsetSrc, dest, offsetDest, count);

            pointer += count;
            Rewind(ref pointer);
        }

        private void Rewind(ref int pointer)
        {
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