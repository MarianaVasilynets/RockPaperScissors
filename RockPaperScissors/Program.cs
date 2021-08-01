using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RockPaperScissors
{
    class Program
    {
        private static string GetHMAC(string text, string key)
        {
            key = key ?? "";

            using (var hmacsha384 = new HMACSHA384(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmacsha384.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(hash);
            }

        }

        private static string GetCryproKey(int byteLength)
        {
            byte[] random = new byte[byteLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            return BitConverter.ToString(random).Replace("-", "");
        }

        public static int[] GetWinCombinations(string[] figures, int playerMove)
        {
            int[] winnners = new int[figures.Length / 2];
            for (int i = 0; i < winnners.Length; i++)
            {
                winnners[i] = (playerMove + i + 1) % figures.Length;
            }
            return winnners;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length % 2 == 0 || args.Length == 1)
            {
                Console.WriteLine("An odd number of parameters must be specified (greater than or equal to 3)");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (args.Length != args.Distinct().Count())
            {
                Console.WriteLine("Parameters' array has repeated values inside");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Random random = new Random();
            string key = GetCryproKey(16);
            int computerMove = random.Next(0, args.Length);
            string HMAC = GetHMAC(computerMove.ToString(), key);
            Console.WriteLine("HMAC: {0}", HMAC);
            Console.WriteLine("Available moves:");

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(i + 1);
            }

            Console.WriteLine("Press 0 to exit");
            Console.WriteLine("Enter your move: ");
            int move = Convert.ToInt32(Console.ReadLine());

            if (move == 0) Environment.Exit(0);


            Console.WriteLine("Your move - {0}", move);
            Console.WriteLine("Computer move - {0}", computerMove + 1);
            var winList = new List<int>(GetWinCombinations(args, move - 1));

            if (winList.Contains(computerMove)) Console.WriteLine("You win!");
            else if (computerMove == move - 1) Console.WriteLine("Draw!");
            else Console.WriteLine("Computer win!");

            Console.WriteLine("HMAC key: {0}", key);

            Console.ReadLine();
        }
    }
}
