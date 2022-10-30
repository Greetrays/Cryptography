using System;
using System.Numerics;

namespace lab1
{
    class RGRClient
    {
        private Random _random = new Random();
        private RGRServer _server = new RGRServer();
        private BigInteger _s;
        private BigInteger _v;
        private BigInteger _t;

        public RGRClient()
        {
            _s = _random.Next((int)Math.Sqrt((double)_server.N), (int)Math.Sqrt((double)_server.N) + (int)Math.Pow(10, 4));
            _v = BigInteger.Pow(_s, 2) % _server.N;
            _t = 1000;
        }

        public bool Login()
        {
            for (int i = 0; i < _t; i++)
            {
                BigInteger r = _random.Next(1, (int)_server.N - 1);
                BigInteger x = BigInteger.Pow(r, 2) % _server.N;
                BigInteger e = _server.GenerateE(x);
                BigInteger y = r * BigInteger.Pow(_s, (int)e);

                if (_server.Verify(y, _v) == false)
                {
                    return false;
                }
            }

            return true;          
        }
    }

    class RGRServer
    {
        private Random _random = new Random();
        private CriptoHelper _criptoHelper = new CriptoHelper();
        private BigInteger _p;
        private BigInteger _q;
        private BigInteger _n;
        private BigInteger _e;
        private BigInteger _clientX;

        public BigInteger N => _n;

        public RGRServer()
        {
            _p = _criptoHelper.GetPrimeRandomNumber(257, 3);
            _q = _criptoHelper.GetPrimeRandomNumber(257, 3);
            _n = _p * _q;
        }

        public BigInteger GenerateE(BigInteger x)
        {
            _clientX = x;
            return _e = _random.Next(0, 2);
        }

        public bool Verify(BigInteger y, BigInteger v)
        {
            if (BigInteger.Pow(y, 2) % N != _clientX * BigInteger.Pow(v, (int)_e) % N)
            {
                return false;
            }

            return true;
        }
    }
}
