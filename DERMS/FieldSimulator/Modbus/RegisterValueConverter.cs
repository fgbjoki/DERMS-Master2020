using System;

namespace FieldSimulator.Modbus
{
    public class RegisterValueConverter
    {
        public Tuple<short, short> SplitValue(float floatValue)
        {
            unsafe
            {
                short* iterator = (short*)(&floatValue);
                short firstPart = *iterator++;
                short secondPart = *iterator;

                return new Tuple<short, short>(firstPart, secondPart);
            }
        }

        public float ConvertToFloat(short value1, short value2)
        {
            float value;
            unsafe
            {
                short* valuePointer = (short*)&value;
                *valuePointer = value1;
                *(valuePointer + 1) = value2;
            }

            return value;
        }
    }
}
