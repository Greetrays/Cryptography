using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.IO;

namespace lab1
{
    class CriptoHelper
    {
        public void TrySwap(ref BigInteger numberA, ref BigInteger numberB)
        {
            if (numberB > numberA)
            {
                BigInteger temp = numberA;
                numberA = numberB;
                numberB = temp;
            }
        }

        public bool CheckForSimplicity(BigInteger number)
        {
            for (BigInteger i = 2; i <= number - 1; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckForMutualSimplicity(BigInteger numberA, BigInteger numberB)
        {
            TrySwap(ref numberA, ref numberB);

            for (BigInteger i = 2; i <= numberA; i++)
            {
                if (numberA % i == 0)
                {
                    if (numberB % i == 0)
                    {
                        return false;
                    }
                }

                if (numberB % i == 0)
                {
                    if (numberA % i == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public byte[] OverwriteBinaryFile(List<BigInteger> arrayBytes, ref byte[] binaryFile)
        {
            for (int i = 0; i < arrayBytes.Count; i++)
            {
                binaryFile[i] = (byte)arrayBytes[i];
            }

            return binaryFile;
        }

        public void WriteFile(string finalPath, byte[] binaryFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (FileStream file = new FileStream(finalPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.GetEncoding(1251)))
                {
                    writer.Write(Encoding.GetEncoding(1251).GetString(binaryFile, 0, binaryFile.Length));
                }
            }
        }

        public BigInteger GetPrimeRandomNumber(int upBound = 2, int downBound = 5)
        {
            Random random = new Random();
            BigInteger number = random.Next(upBound, (int)BigInteger.Pow(10, downBound));

            while (CheckForSimplicity(number) == false)
            {
                number = random.Next(upBound, (int)BigInteger.Pow(10, downBound));
            }

            return number;
        }

        public List<BigInteger> ConverBinaryInBigInteger(byte[] binaryArray)
        {
            List<BigInteger> newArray = new List<BigInteger>();

            foreach (var item in binaryArray)
            {
                newArray.Add(item);
            }

            return newArray;
        }

        public BigInteger FindPrimitiveRootEuler(BigInteger number)
        {
            BigInteger result = number;
            BigInteger en = Convert.ToUInt64(Math.Sqrt((int)number) + 1);

            for (BigInteger i = 2; i <= en; i++)
            {
                if ((number % i) == 0)
                {
                    while ((number % i) == 0)
                    {
                        number /= i;
                    }

                    result -= (result / i);
                }
            }

            if (number > 1)
            {
                result -= (result / number);
            }

            return result;
        }

        public byte[] ReadFile(string puth)
        {
            byte[] binaryFile = new byte[0];

            using (FileStream file = new FileStream(puth, FileMode.Open))
            {
                binaryFile = new byte[file.Length];
                file.Read(binaryFile, 0, (int)file.Length);
            }

            return binaryFile;
        }

        public void PrintError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Такого пункта не существует. Нажмите, чтобы повторить попытку . . .");
        }
    }
}
