using System;
using System.IO;

namespace ConsoleBlackJack {
    class ConsoleBlackJack {
        public static string[] playersHand = new string[13];
        public static string[] dealersHand = new string[13];
        public static int cash;

        public static int[] drawnCards = new int[14]; // 0 Ace 11 Jack 12 Queen 13 King

        static string RandomCardDraw() {
            Random rand = new Random();
            int drawn = rand.Next(14);
            if (drawnCards[drawn] == 4) {
                return RandomCardDraw();
            }

            switch (drawn) {
                case 0:
                    return "A";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                default:
                    try {
                        return Convert.ToString(drawn);
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                        return "null";
                    }
            }
        }

        public static void Main(string[] args) {
            using (StreamReader sr = File.OpenText("save.dat")) {
                cash = Convert.ToInt32(sr.ReadLine());
            }

            for (int i = 0; i < 14; i++) {
                drawnCards[i] = 0;
            }
            Console.WriteLine(RandomCardDraw());
        }
    }
}