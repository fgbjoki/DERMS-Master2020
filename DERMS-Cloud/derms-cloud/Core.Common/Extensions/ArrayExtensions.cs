using System;

namespace Core.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static byte[] Append(this byte[] firstArray, byte[] secondArray)
        {
            byte[] appendedArray = new byte[firstArray.Length + secondArray.Length];

            Buffer.BlockCopy(firstArray, 0, appendedArray, 0, firstArray.Length);
            Buffer.BlockCopy(secondArray, 0, appendedArray, firstArray.Length, secondArray.Length);

            return appendedArray;
        }

        public static byte[] Append(this byte[] firstArray, byte secondArray)
        {
            byte[] appendedArray = new byte[firstArray.Length + 1];

            Buffer.BlockCopy(firstArray, 0, appendedArray, 0, firstArray.Length);
            appendedArray[firstArray.Length] = secondArray;

            return appendedArray;
        }
    }
}
