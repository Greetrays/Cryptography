using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isWork = true;
            CriptoHelper criptoHelper = new CriptoHelper();

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Выберите лабораторную:");
                Console.WriteLine("1. Криптографическая библиотека с 4 функциями");
                Console.WriteLine("2. Библиотека с алгоритмами шифрования данных");
                Console.WriteLine("3. Библиотека с алгоритмами электронной подписи файлов");
                Console.WriteLine("4. Ментальный покер");
                Console.WriteLine("5. \"Протокол слепой\" подписи");
                Console.WriteLine("6. Протокол Фаита-Шамира");
                Console.WriteLine("E. Выход из программы");

                string userInput = Console.ReadLine().ToUpper();
                switch (userInput)
                {
                    case "1":
                        Lab1 lab1 = new Lab1();
                        lab1.StartProgram();
                        break;

                    case "2":
                        Lab2 lab2 = new Lab2();
                        lab2.StartProgram();
                        break;

                    case "3":
                        Lab3 lab3 = new Lab3();
                        lab3.StartProgram();
                        break;

                    case "4":
                        Console.WriteLine("Отдельный репозиторий MentalPoker");
                        break;

                    case "5":
                        Client client = new Client();
                        client.StartVote();
                        break;

                    case "6":
                        Client client = new Client();
                        client.StartVote();
                        break;

                    case "E":
                        isWork = false;
                        break;

                    default:
                        criptoHelper.PrintError();
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    class Lab1
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();

        public void StartProgram()
        {
            Criptographic _criptographic = new Criptographic();
            Random _rand = new Random();
            bool _isWork = true;

            while (_isWork)
            {
                Console.Clear();
                Console.WriteLine("Выберите из возможных вариантов");
                Console.WriteLine("1. Быстрое возведение числа в степень по модулю");
                Console.WriteLine("2. Функция, реализующая обобщенный алгоритм Евклида");
                Console.WriteLine("3. Функция построения общего ключа для двух абонентов по схеме Диффи - Хеллмана");
                Console.WriteLine("4. Функция, которая решает задачу нахождения дискретного логарифма при помощи алгоритма «Шаг младенца, шаг великана».");
                Console.WriteLine("E. Выход из программы");

                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        BigInteger number = _rand.Next(1, (int)Math.Pow(10, 7));
                        BigInteger degree = _rand.Next(1, (int)Math.Pow(10, 6));
                        BigInteger module = _rand.Next(1, (int)Math.Pow(10, 7));

                        Console.WriteLine($"{number}^{degree} mod {module} = ");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{number}^{degree} mod {module} = {_criptographic.RaiseDegreeModulo(number, degree, module)}");
                        break;

                    case "2":
                        BigInteger numberA = _rand.Next(1, (int)Math.Pow(10, 9));
                        BigInteger numberB = _rand.Next(1, (int)Math.Pow(10, 9));

                        Console.WriteLine($"Исходные данные \nA = {numberA}\nB = {numberB}");
                        List<BigInteger> results = _criptographic.FindGCDEvklid(numberA, numberB);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"GCD(a, b) = {results[0]}");
                        Console.WriteLine($"X = {results[1]}");
                        Console.WriteLine($"Y = {results[2]}");
                        break;

                    case "3":
                        BigInteger xA;
                        BigInteger xB;
                        BigInteger yA;
                        BigInteger yB;
                        BigInteger p;
                        BigInteger g;
                        _criptographic.BuildSharedKeyDiffieHellman();
                        break;

                    case "4":
                        _criptographic.FindDiscreteLogarithm();
                        break;


                    case "E":
                        _isWork = false;
                        break;

                    default:
                        _criptoHelper.PrintError();
                        break;
                }

                Console.WriteLine("Нажмите, чтобы продолжить . . .");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    class Lab2
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();

        public void StartProgram()
        {
            Encryption _encryption = new Encryption();
            Random _rand = new Random();
            bool _isWork = true;

            while (_isWork)
            {
                Console.Clear();
                Console.WriteLine("Выберите из возможных вариантов");
                Console.WriteLine("1. Шифр Шамира");
                Console.WriteLine("2. Шифр Эль-Гамаля");
                Console.WriteLine("3. Шифр Вернама");
                Console.WriteLine("4. Шифр RSA");
                Console.WriteLine("E. Выход из программы");

                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        _encryption.EncryptShammirCipher(_criptoHelper.ReadFile(@"Lab2\Shamir.jpg"), @"Lab2\Shamir\Result.jpg");
                        break;

                    case "2":
                        _encryption.EncryptElGamalCipher(_criptoHelper.ReadFile(@"Lab2\ElGamal.jpg"), @"Lab2\ElGamal\Result.jpg");
                        break;

                    case "3":
                        _encryption.EncryptVernamCipher(_criptoHelper.ReadFile(@"Lab2\Vernam.jpg"), @"Lab2\Vernam\Result.jpg");
                        break;

                    case "4":
                        _encryption.EncryptRSACipher(_criptoHelper.ReadFile(@"Lab2\RSA.jpg"), @"Lab2\RSA\Result.jpg");
                        break;

                    case "E":
                        _isWork = false;
                        break;

                    default:
                        _criptoHelper.PrintError();
                        break;
                }

                Console.WriteLine("Нажмите, чтобы продолжить . . .");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    class Lab3
    {
        private CriptoHelper _criptoHelper = new CriptoHelper();

        public void StartProgram()
        {
            DigitalSignature _digitalSignature = new DigitalSignature();
            Random _rand = new Random();
            bool _isWork = true;

            while (_isWork)
            {
                Console.Clear();
                Console.WriteLine("Выберите из возможных вариантов");
                Console.WriteLine("1. Подпись Эль-Гамаля");
                Console.WriteLine("2. Подпись RSA");
                Console.WriteLine("3. Подпись по ГОСТ");
                Console.WriteLine("E. Выход из программы");

                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        if (_digitalSignature.SignElGamal(_criptoHelper.ReadFile(@"Lab3\ElGamal.jpg"), @"Lab3\ElGamalResult.jpg"))
                        {
                            Console.WriteLine("Подпись верна!");
                        }
                        else
                        {
                            Console.WriteLine("Подпись не верна!");
                        }
                        break;

                    case "2":
                        if (_digitalSignature.SignRSA(_criptoHelper.ReadFile(@"Lab3\RSA.jpg"), @"Lab3\RSAResult.jpg"))
                        {
                            Console.WriteLine("Подпись верна!");
                        }
                        else
                        {
                            Console.WriteLine("Подпись не верна!");
                        }
                        break;

                    case "3":
                        if (_digitalSignature.SignGOST(_criptoHelper.ReadFile(@"Lab3\GOST.jpg"), @"Lab3\GOSTResult.jpg"))
                        {
                            Console.WriteLine("Подпись верна!");
                        }
                        else
                        {
                            Console.WriteLine("Подпись не верна!");
                        }
                        break;

                    case "E":
                        _isWork = false;
                        break;

                    default:
                        _criptoHelper.PrintError();
                        break;
                }

                Console.WriteLine("Нажмите, чтобы продолжить . . .");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
