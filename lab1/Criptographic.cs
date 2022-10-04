using System;
using System.Collections.Generic;
using System.Numerics;

namespace lab1
{
    class Criptographic
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();

        public BigInteger RaiseDegreeModulo(BigInteger number, BigInteger degree, BigInteger module)
        {
            BigInteger result = 1;
            List<BigInteger> degrees = new List<BigInteger>();
            GetDegreesTwo(DecomposeModule(degree), degrees);

            for (int i = 0; i < degrees.Count; i++)
            {
                result *= BigInteger.Pow(number, (int)BigInteger.Pow(2, (int)degrees[i])) % module;
            }

            result %= module;
            return result;
        }

        public List<BigInteger> FindGCDEvklid(BigInteger numberA, BigInteger numberB)
        {
            _criptoHelper.TrySwap(ref numberA, ref numberB);
            List<BigInteger> U = new List<BigInteger> { numberA, 1, 0 };
            List<BigInteger> V = new List<BigInteger> { numberB, 0, 1 };
            List<BigInteger> T = new List<BigInteger>();
            BigInteger q;

            while (V[0] > 0)
            {
                q = U[0] / V[0];
                T = new List<BigInteger> { U[0] % V[0], U[1] - q * V[1], U[2] - q * V[2] };
                U = V;
                V = T;
            }

            return U;
        }

        public void BuildSharedKeyDiffieHellman()
        {
            Random random = new Random();
            BigInteger q = random.Next(2, (int)BigInteger.Pow(10, 5));
            BigInteger p = 2 * q + 1;

            
            while (_criptoHelper.CheckForSimplicity(p) == false)
            {
                q = random.Next(2, (int)Math.Pow(10, 5));

                if (_criptoHelper.CheckForSimplicity(q))
                {
                    p = 2 * q + 1;
                }

            }

            Anew:
            BigInteger g = random.Next(1, (int)p - 1);

            if (BigInteger.Pow(g, (int)q) % p == 1)
            {
                goto Anew;
            }

            BigInteger xA = random.Next(2, (int)p - 1);
            BigInteger xB = random.Next(2, (int)p - 1);

            BigInteger yA = BigInteger.Pow(g, (int)xA) % p;
            BigInteger yB = BigInteger.Pow(g, (int)xB) % p;

            BigInteger zAB = BigInteger.Pow(yB, (int)xA) % p; 
            BigInteger zBA = BigInteger.Pow(yA, (int)xB) % p; 

            Console.WriteLine("Ключ Алисы " + zAB + "\nКлюч Боба " + zBA);
        }

        public void FindDiscreteLogarithm()
        {
            Random random = new Random();
            BigInteger p = random.Next(1, (int)Math.Pow(10, 2));
            BigInteger y = random.Next(0, (int)p);
            BigInteger a = random.Next(1, (int)Math.Pow(10, 2));

            BigInteger m = (BigInteger)Math.Sqrt((int)p) + 1;
            BigInteger k = m;
            List<BigInteger> babySteps = new List<BigInteger>();
            List<BigInteger> gaintSteps = new List<BigInteger>();
            List<BigInteger> results = new List<BigInteger>();

            for (BigInteger j = 0; j < m; j++)
            {
                babySteps.Add(y * BigInteger.Pow(a, (int)j) % p);
            }

            for (BigInteger i = 1; i <= k; i++)
            {
                gaintSteps.Add(BigInteger.Pow(a, (int)(i * m)) % p);
            }

            for (BigInteger j = 0; j < m; j++)
            {
                for (BigInteger i = 0; i < k; i++)
                {
                    if (babySteps[(int)j] == gaintSteps[(int)i])
                    {
                        results.Add((i + 1) * m - j);
                    }
                }
            }

            results.Sort();

            for (int i = 0; i < results.Count; i++)
            {
                Console.Write($"{a}^{results[i]} % {p} = {y} \n");
            }
        }

        private void GetDegreesTwo(List<BigInteger> arrayBinaryNumbers, List<BigInteger> degrees)
        {
            for (int i = 0; i < arrayBinaryNumbers.Count; i++)
            {
                if (arrayBinaryNumbers[i] > 0)
                {
                    degrees.Add(i);
                }
            }
        }

        private List<BigInteger> DecomposeModule(BigInteger degree)
        {
            List<BigInteger> arrayBinaryNumbers = new List<BigInteger>();

            for(int i = 0; degree != 1; i++)
            {
                arrayBinaryNumbers.Add(degree % 2);
                degree = (int)degree / 2;
            }

            arrayBinaryNumbers.Add(degree);
            return arrayBinaryNumbers;
        }
    }
}
