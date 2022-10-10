using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace lab1
{
    class Client
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();
        private Criptographic _criptographic = new Criptographic();
        private Random _random = new Random();
        private Server _server = new Server();

        public void StartVote()
        {
            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Анонимное голосование за поправки");
                Console.WriteLine("1 - Голосовать");
                Console.WriteLine("2 - Закрыть голосование и показать результат");
                Console.Write("Введите номер действия: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Vote();
                        break;

                    case "2":
                        Console.Clear();
                        _server.ShowResult();
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка. Попробуйте снова!");
                        Console.WriteLine("Нажмите любую  кнопку");
                        Console.ReadKey();
                        break;
                }
            }

            Console.ReadKey();
            Console.WriteLine("Нажмите любую клаившу . . .");
        }

        private void Vote()
        {
            bool isWork = true;
            BigInteger rnd = _random.Next(0, 1000);

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Введите имя:");
                string userName = Console.ReadLine();

                Console.WriteLine("1 - Я за поправки");
                Console.WriteLine("2 - Я против поправок");
                Console.Write("Введите номер вашего выбора: ");
                string v = Console.ReadLine();

                if (v == "1" || v == "2")
                {
                    GenerateBulletin(v, rnd, userName);
                    isWork = false;
                }
                else
                {
                    Console.WriteLine("Ошибка. Попробуйте снова!");
                    Console.WriteLine("Нажмите любую  кнопку");
                    Console.ReadKey();
                }
            }
        }

        private void GenerateBulletin(string v, BigInteger rnd, string userName)
        {
            rnd = _random.Next(257, 10000);
            string n = rnd + ";" + v + ";" + DateTime.Now.ToShortTimeString();
            byte[] nBytes = Encoding.Unicode.GetBytes(n);

            MD5 md5 = MD5.Create();
            byte[] h = md5.ComputeHash(nBytes);

            BigInteger r = _random.Next(257, 10000);

            while (_criptoHelper.CheckForMutualSimplicity(r, _server.N) == false)
            {
                r = _random.Next(257, 10000);
            }

            List<BigInteger> H = new List<BigInteger>();

            for (int i = 0; i < h.Length; i++)
            {
                H.Add(h[i] % _server.N * _criptographic.RaiseDegreeModulo(r, _server.D, _server.N) % _server.N);
            }

            if (_server.SignBulletin(H, userName, out List<BigInteger> signBulletin))
            {
                List<BigInteger> mySignBulletin = new List<BigInteger>();

                BigInteger reverseR = _criptographic.FindGCDEvklid(r, _server.N)[2];

                if (reverseR < 0)
                {
                    reverseR = (reverseR + _server.N) % _server.N;
                }

                foreach (var item in signBulletin)
                {
                    mySignBulletin.Add((item * reverseR) % _server.N);
                }

                _server.RecvBulletin(n, mySignBulletin);
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Вы уже голосовали!");
                Console.ReadKey();
            }
        }
    }

    class Server
    {
        private List<string> _userNames = new List<string>();
        private Random _random = new Random();
        private CriptoHelper _criptoHelper = new CriptoHelper();
        private Criptographic _criptographic = new Criptographic();
        private BigInteger _p;
        private BigInteger _q;
        private BigInteger _n;
        private BigInteger _fi;
        private BigInteger _d;
        private BigInteger _c;

        public BigInteger D => _d;
        public BigInteger N => _n;

        private int _agreeCount;
        private int _doNotAgreeCount;

        public Server()
        {
            _p = _criptoHelper.GetPrimeRandomNumber(257, 3);
            _q = _criptoHelper.GetPrimeRandomNumber(257, 3);
            _n = _p * _q;
            _fi = (_p - 1) * (_q - 1);
            _d = _random.Next(1, (int)_fi);

            while (_criptoHelper.CheckForMutualSimplicity(_d, _fi) == false)
            {
                _d = _random.Next(1, (int)_fi);
            }

            _c = _criptographic.FindGCDEvklid(_d, _fi)[2];

            if (_c < 0)
            {
                _c = (_c + _fi) % _fi;
            }
        }

        public bool SignBulletin(List<BigInteger> bulletin, string userName, out List<BigInteger> signBulletin)
        {
            foreach (var item in _userNames)
            {
                if (userName == item)
                {
                    signBulletin = null;
                    return false;
                }
            }

            _userNames.Add(userName);

            signBulletin = new List<BigInteger>();

            for (int i = 0; i < bulletin.Count; i++)
            {
                signBulletin.Add(_criptographic.RaiseDegreeModulo(bulletin[i], _c, _n));
            }

            return true;
        }

        public void RecvBulletin(string n, List<BigInteger> s)
        {
            byte[] nBytes = Encoding.Unicode.GetBytes(n);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(nBytes);

            for (int i = 0; i < hash.Length; i++)
            {
                Console.Write(hash[i]);
            }

            Console.WriteLine();

            for (int i = 0; i < hash.Length; i++)
            {
                Console.Write(_criptographic.RaiseDegreeModulo(s[i], _d, _n));
            }

            CountVotes(n);
        }

        public void ShowResult()
        {
            Console.WriteLine($"За поправки проголосовало {_agreeCount} человек");
            Console.WriteLine($"Против поправок проголосовало {_doNotAgreeCount} человек");
        }

        private void CountVotes(string n)
        {
            string[] strs = n.Split(new char[] { ';' });

            if (strs[1] == "1")
            {
                _agreeCount++;
            }
            else if (strs[1] == "2")
            {
                _doNotAgreeCount++;
            }
        }
    }
}
