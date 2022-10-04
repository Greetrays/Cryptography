using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace lab1
{
    class DigitalSignature
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();
        private Criptographic _criptographic = new Criptographic();

        public bool SignElGamal(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            BigInteger p = _criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger g = _criptoHelper.FindPrimitiveRootEuler(p) % p;
            BigInteger x = random.Next(2, (int)p - 1);
            BigInteger y = BigInteger.Pow(g, (int)x) % p;

            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(binaryFile);

            BigInteger k = random.Next(1, (int)p - 1);

            while (_criptoHelper.CheckForMutualSimplicity(k, p - 1) == false)
            {
                k = random.Next(1, (int)p - 1);
            }

            BigInteger inverseK = _criptographic.FindGCDEvklid(k, p - 1)[2];

            if (inverseK < 0)
            {
                inverseK = (inverseK + (p - 1)) % (p - 1);
            }

            BigInteger r = BigInteger.Pow(g, (int)k) % p;
            List<BigInteger> u = new List<BigInteger>();
            List<BigInteger> s = new List<BigInteger>();

            WriteSignFile(binaryFile, s, r, finalPath);

            for (int i = 0; i < hash.Length; i++)
            {
                u.Add(((hash[i] - x * r) % (p - 1) + (p - 1)) % (p - 1));
                s.Add(k * u[i] % (p - 1));
            }

            for (int i = 0; i < hash.Length; i++)
            {
                if ((BigInteger.Pow(y, (int)r) % p * BigInteger.Pow(r, (int)s[i]) % p) % p != BigInteger.Pow(g, (int)hash[i]) % p)
                {
                    return false;
                }
            }

            Console.WriteLine("S Подпись");
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("R подпись");
            Console.WriteLine(r);

            return true;
        }

        public bool SignRSA(byte[] binaryFile, string finalPath)
        {
            Random random = new Random();
            BigInteger p = _criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger q = _criptoHelper.GetPrimeRandomNumber(257, 3);
            BigInteger n = p * q;
            BigInteger fi = (p - 1) * (q - 1);
            BigInteger d = random.Next(1, (int)fi);

            while (_criptoHelper.CheckForMutualSimplicity(d, fi) == false)
            {
                d = random.Next(1, (int)fi);
            }

            BigInteger c = _criptographic.FindGCDEvklid(d, fi)[2];

            if (c < 0)
            {
                c = (c + fi) % fi;           
            }

            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(binaryFile);

            List<BigInteger> s = new List<BigInteger>();

            foreach (var item in hash)
            {
                s.Add(BigInteger.Pow(item, (int)c) % n);
            }

            WriteSignFile(binaryFile, s, finalPath);

            Console.WriteLine($"S подпись");

            foreach (var item in s)
            {
                Console.Write(item);
            }

            List<BigInteger> w = new List<BigInteger>();

            for (int i = 0; i < s.Count; i++)
            {
                w.Add(BigInteger.Pow(s[i], (int)d) % n);

                if (w[i] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool SignGOST(byte[] binaryFile, string finalPath)
        {
            int degree = 3;
            Random random = new Random();
            BigInteger p;
            BigInteger q = random.Next(257, (int)MathF.Pow(10, degree));

            while (_criptoHelper.CheckForSimplicity(q) == false)
            {
                q = random.Next(1, (int)MathF.Pow(10, 5));
            }

            BigInteger b = random.Next(1, (int)MathF.Pow(10, degree));

            while (_criptoHelper.CheckForSimplicity(b * q + 1) == false)
            {
                b = random.Next(0, (int)MathF.Pow(10, degree));
            }

            p = b * q + 1;

            BigInteger a = random.Next(2, (int)MathF.Pow(10, degree));

            while (_criptographic.RaiseDegreeModulo(a, b, p) > 1)
            {
                a = random.Next(1, (int)MathF.Pow(10, degree));
            }

            BigInteger x = random.Next(1, (int)q);
            BigInteger y = _criptographic.RaiseDegreeModulo(a, x, p);

            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(binaryFile);

            BigInteger k = random.Next(1, (int)q);
            BigInteger r = _criptographic.RaiseDegreeModulo(a, k, p) % q;
            List<BigInteger> s = new List<BigInteger>();

            foreach (var item in hash)
            {
                s.Add((k * item + x * r) % q);
            }
            
            List<BigInteger> u1 = new List<BigInteger>();

            for (int i = 0; i < hash.Length; i++)
            {
                BigInteger inverseHash = _criptographic.FindGCDEvklid(hash[i], q)[2];

                if (inverseHash < 0)
                {
                    inverseHash = (inverseHash + q) % q;
                }

                u1.Add(s[i] * inverseHash % q);
            }

            List<BigInteger> u2 = new List<BigInteger>();

            for (int i = 0; i < hash.Length; i++)
            {
                BigInteger inverseHash = _criptographic.FindGCDEvklid(hash[i], q)[2];

                if (inverseHash < 0)
                {
                    inverseHash = (inverseHash + q) % q;
                }

                u2.Add((-r * inverseHash % q + q) % q);
            }

            List<BigInteger> v = new List<BigInteger>();

            for (int i = 0; i < hash.Length; i++)
            {
                v.Add((BigInteger.Pow(a, (int)u1[i]) * BigInteger.Pow((int)y, (int)u2[i]) % p) % q);

                if (v[i] != r)
                {
                    return false;
                }
            }

            Console.WriteLine("S Подпись");
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("R подпись");
            Console.WriteLine(r);

            return true;
        }

        private void WriteSignFile(byte[] binaryFile, List<BigInteger> s, BigInteger r, string finalPath)
        {
            List<BigInteger> signetBinaryFile = new List<BigInteger>();

            foreach (var item in binaryFile)
            {
                signetBinaryFile.Add(item);
            }

            byte[] tempBinaryInteger;

            foreach (var item in s)
            {
                tempBinaryInteger = BitConverter.GetBytes((int)item);

                for (int i = 0; i < tempBinaryInteger.Length; i++)
                    signetBinaryFile.Add(tempBinaryInteger[i]);
            }

            tempBinaryInteger = BitConverter.GetBytes((int)r);

            for (int i = 0; i < tempBinaryInteger.Length; i++)
                signetBinaryFile.Add(tempBinaryInteger[i]);

            byte[] signetFile = new byte[signetBinaryFile.Count];

            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(signetBinaryFile, ref signetFile));
        }

        private void WriteSignFile(byte[] binaryFile, List<BigInteger> s, string finalPath)
        {
            List<BigInteger> signetBinaryFile = new List<BigInteger>();

            foreach (var item in binaryFile)
            {
                signetBinaryFile.Add(item);
            }

            byte[] tempBinaryInteger;

            foreach (var item in s)
            {
                tempBinaryInteger = BitConverter.GetBytes((int)item);

                for (int i = 0; i < tempBinaryInteger.Length; i++)
                    signetBinaryFile.Add(tempBinaryInteger[i]);
            }

            byte[] signetFile = new byte[signetBinaryFile.Count];

            _criptoHelper.WriteFile(finalPath, _criptoHelper.OverwriteBinaryFile(signetBinaryFile, ref signetFile));
        }
    }
}
