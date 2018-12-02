using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_32
{
    class Conversions
    {
        public static string Extract_Value(float value)
        {
            string result;
            string partInt;
            string partFra;

            if (value < 0)
            {
                result = "1";
                value = Math.Abs(value);
            }
            else
                result = "0";

            // Pega a parte inteira
            int _partInt = (int)(value);
            partInt = _partInt.ToString();
            if (partInt.Length < 2)
                partInt = "0" + partInt;

            // Pega a parte fracinaria
            float _partFra = value - _partInt;
            partFra = Convert.ToString(_partFra);

            if (partFra.Length == 1)
                partFra = "0" + partFra;
            else if (partFra.Length == 3)
            {
                partFra = partFra.Substring(partFra.IndexOf(',') + 1, 1);
                partFra += "0";
            }
            else
                partFra = partFra.Substring(partFra.IndexOf(',') + 1, 2);

            result += partInt + partFra;

            return result;
        }

        public static string GetStringFromAsciiHex(String input)
        {
            if (input.Length % 2 != 0)
                throw new ArgumentException("input");

            byte[] bytes = new byte[input.Length / 2];

            for (int i = 0; i < input.Length; i += 2)
            {
                // Split the string into two-bytes strings which represent a hexadecimal value, and convert each value to a byte
                String hex = input.Substring(i, 2);
                bytes[i / 2] = Convert.ToByte(hex, 16);
            }

            return System.Text.ASCIIEncoding.ASCII.GetString(bytes);
        }
    }
}
