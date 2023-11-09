using System;
using System.IO;
using System.Security.Principal;
using System.Threading;

namespace ConsoleBlackJack {
    class ConsoleBlackJack {
        public static string[] playersHand = new string[13];
        public static int numPlayerTurns = 0;
        public static int playerVal = 0;
        public static int playerAces = 0;

        public static string[] dealersHand = new string[13];
        public static int numDealerTurns = 0;
        public static int dealerVal = 0;
        public static int dealerAces = 0;

        public static int cash;
        public static bool turn = false; //true for player  false for dealer

        public static int[] drawnCards = new int[13]; // 0 Ace 11 Jack 12 Queen 13 King

        static string RandomCardDraw(bool whoDraw /*true, player draw. false, dealer draw*/) {
            Random rand = new Random();
            int drawn = rand.Next(13);
            if (drawnCards[drawn] == 4) {
                return RandomCardDraw(whoDraw);
            }
            if (whoDraw) {
                switch (drawn) {
                    case 0:
                        playerVal += 11;
                        playerAces += 1;
                        return "A";
                    case 10:
                        playerVal += 10;
                        return "J";
                    case 11:
                        playerVal += 10;
                        return "Q";
                    case 12:
                        playerVal += 10;
                        return "K";
                    default:
                        try {
                            playerVal += drawn + 1;
                            return Convert.ToString(drawn+1);
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                            return "null";
                        }
                }
            } else {
                switch (drawn) {
                    case 0:
                        dealerVal += 11;
                        dealerAces += 1;
                        return "A";
                    case 10:
                        dealerVal += 10;
                        return "J";
                    case 11:
                        dealerVal += 10;
                        return "Q";
                    case 12:
                        dealerVal += 10;
                        return "K";
                    default:
                        try {
                            dealerVal += drawn + 1;
                            return Convert.ToString(drawn+1);
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                            return "null";
                        }
                }
            }
        }

        static void writeArray(string[] array, int arrayLen) {
            for (int i = 0; i < arrayLen; i++) {
                Console.Write(array[i]);
                if (i != arrayLen-1) {
                    Console.Write(", ");
                }
            }
        }

        static void game() {
            if (turn) {
                Console.Clear();
                Console.Write("Your current cards: ");
                writeArray(playersHand, numPlayerTurns);
                Console.Write("   The value of you hand is: " + Convert.ToString(playerVal))
                Console.WriteLine();
                Console.WriteLine("What would you like to do? Hit/Stand");
                string? response = Console.ReadLine();
                if (response != null) {
                    if (response.ToUpper() == "HIT") {
                        playersHand[numPlayerTurns] = RandomCardDraw(true);
                        numPlayerTurns++;
                    }
                } else {
                    Console.WriteLine("You cannot input a null value, try again.");
                    Thread.Sleep(3000);
                    game();
                }
            }
        }

        public static void Main(string[] args) {
            using (StreamReader sr = File.OpenText("save.dat")) {
                cash = Convert.ToInt32(sr.ReadLine());
            }

            for (int i = 0; i < 13; i++) {
                drawnCards[i] = 0;
            }

            playersHand[0] = RandomCardDraw(true);
            playersHand[1] = RandomCardDraw(true);
            dealersHand[0] = RandomCardDraw(false);
            dealersHand[1] = RandomCardDraw(false);
            numPlayerTurns = 2;
            numDealerTurns = 2;
        }
    }
}
