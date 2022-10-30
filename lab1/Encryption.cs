using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace lab1
{
    class Encryption
    {
        private Criptographic _criptographic = new Criptographic();
        private CriptoHelper _criptoHelper = new CriptoHelper();

        public void EncryptShammirCipher(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            BigInteger cA = random.Next(0, (int)Math.Pow(10, 5));
            BigInteger dA = random.Next(0, (int)Math.Pow(10, 5));

            BigInteger q = random.Next(2, (int)BigInteger.Pow(10, 5));
            BigInteger p = 2 * q + 1;

            while (_criptoHelper.CheckForSimplicity(p) == false)
            {
                q = random.Next(256, (int)Math.Pow(10, 5));

                if (_criptoHelper.CheckForSimplicity(q))
                {
                    p = 2 * q + 1;
                }
            }

            while (cA * dA % (p - 1) != 1)
            {
                cA = random.Next(0, (int)Math.Pow(10, 5));
                dA = random.Next(0, (int)Math.Pow(10, 5));
            }

            BigInteger cB = random.Next(0, (int)Math.Pow(10, 5));
            BigInteger dB = random.Next(0, (int)Math.Pow(10, 5));

            while (cB * dB % (p - 1) != 1)
            {
                cB = random.Next(0, (int)Math.Pow(10, 5));
                dB = random.Next(0, (int)Math.Pow(10, 5));
            }

            List<BigInteger> bigIntBynaryFile = _criptoHelper.ConverBinaryInBigInteger(binaryFile);

            List<BigInteger> x1 = EncryptKeys(bigIntBynaryFile, cA, p);
            _criptoHelper.WriteFile(@"Lab2\Shamir\HUI1.jpg", _criptoHelper.OverwriteBinaryFile(x1, ref binaryFile));
            List<BigInteger> x2 = EncryptKeys(x1, cB, p);
            _criptoHelper.WriteFile(@"Lab2\Shamir\HUI2.jpg", _criptoHelper.OverwriteBinaryFile(x2, ref binaryFile));
            List<BigInteger> x3 = EncryptKeys(x2, dA, p);
            _criptoHelper.WriteFile(@"Lab2\Shamir\HUI3.jpg", _criptoHelper.OverwriteBinaryFile(x3, ref binaryFile));
            List<BigInteger> x4 = EncryptKeys(x3, dB, p);
            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(x4, ref binaryFile));
        }

        public void EncryptElGamalCipher(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            BigInteger p = _criptoHelper.GetPrimeRandomNumber(257);
            BigInteger cA = random.Next(2, (int)p - 1);
            BigInteger cB = random.Next(2, (int)p - 1);

            BigInteger g = _criptoHelper.FindPrimitiveRootEuler(p) % p;

            BigInteger dA = _criptographic.RaiseDegreeModulo(g, cA, p);
            BigInteger dB = _criptographic.RaiseDegreeModulo(g, cB, p);

            BigInteger k = random.Next(1, (int)p - 2);

            while (_criptoHelper.CheckForMutualSimplicity(k, p - 1) == false)
            {
                k = random.Next(1, (int)p - 2);
            }

            BigInteger r = _criptographic.RaiseDegreeModulo(g, k, p);
            List<BigInteger> e = new List<BigInteger>();

            for (int i = 0; i < binaryFile.Length; i++)
            {
                e.Add(binaryFile[i] * dB % p);
            }

            List<BigInteger> newBinaryFile = new List<BigInteger>();

            for (int i = 0; i < binaryFile.Length; i++)
            {
                newBinaryFile.Add(e[i] * BigInteger.Pow(r, (int)p - 1 - (int)cB) % p);
            }

            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(newBinaryFile, ref binaryFile));
        }

        public void EncryptVernamCipher(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            List<BigInteger> keys = new List<BigInteger>();

            for (int i = 0; i < binaryFile.Length; i++)
            {
                keys.Add(random.Next(1, (int)BigInteger.Pow(10, 5)));
            }

            List<BigInteger> e = ArrayXOR(_criptoHelper.ConverBinaryInBigInteger(binaryFile), keys, binaryFile.Length);
            List<BigInteger> newBinaryFile = ArrayXOR(e, keys, binaryFile.Length);
            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(newBinaryFile, ref binaryFile));
        }

        public void EncryptRSACipher(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            BigInteger pA = (BigInteger)_criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger qA = (BigInteger)_criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger nA = pA * qA;
            BigInteger fiA = (pA - 1) * (qA - 1);
            BigInteger dA = random.Next(1, (int)fiA);

            while (_criptoHelper.CheckForMutualSimplicity(dA, fiA) == false)
            {
                dA = random.Next(1, (int)fiA);
            }

            BigInteger cA = (BigInteger)_criptographic.FindGCDEvklid(dA, fiA)[2];

            if (cA < 0)
            {
                cA = (cA + fiA) % fiA;
            }

            BigInteger pB = (BigInteger)_criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger qB = (BigInteger)_criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger nB = pB * qB;
            BigInteger fiB = (pB - 1) * (qB - 1);
            BigInteger dB = random.Next(1, (int)fiB);

            while (_criptoHelper.CheckForMutualSimplicity(dB, fiB) == false)
            {
                dB = random.Next(1, (int)fiB);
            }

            BigInteger cB = (BigInteger)_criptographic.FindGCDEvklid(dB, fiB)[2];

            if (cB < 0)
            {
                cB = (cB + fiB) % fiB;
            }

            List<BigInteger> bigIntBynaryFile = _criptoHelper.ConverBinaryInBigInteger(binaryFile);
            List<BigInteger> e = EncryptKeys(bigIntBynaryFile, dB, nB);
            List<BigInteger> w = EncryptKeys(e, cB, nB);

            byte[] nByte = new byte[w.Count];

            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(w, ref nByte));

            /*
            Console.WriteLine("Отковертировали");
            List<BigInteger> e = EncryptKeys(bigIntBynaryFile, cA, nA);
            Console.WriteLine("Прошли е");
            List<BigInteger> f = EncryptKeys(e, dB, nB);
            Console.WriteLine("Прошли ф");
            List<BigInteger> u = EncryptKeys(f, cB, nB);
            Console.WriteLine("Прошли у");
            List<BigInteger> w = EncryptKeys(u, dA, nA);
            Console.WriteLine("Прошли в");
            */
        }

        private List<BigInteger> ArrayXOR(List<BigInteger> arrayA, List<BigInteger> arrayB, BigInteger lenght)
        {
            List<BigInteger> newArray = new List<BigInteger>();

            for (int i = 0; i < lenght; i++)
            {
                newArray.Add(arrayA[i] ^ arrayB[i]);
            }

            return newArray;
        }

        private List<BigInteger> EncryptKeys(List<BigInteger> data, BigInteger degree, BigInteger module)
        {
            List<BigInteger> keys = new List<BigInteger>();

            for (int i = 0; i < data.Count; i++)
            {
                keys.Add(_criptographic.RaiseDegreeModulo(data[i], degree, module));
            }

            return keys;
        }
    }
}