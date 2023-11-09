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
        public static bool dealerStanding = false;

        public static int cash;
        public static bool turn = true; //true for player  false for dealer

        public static int[] drawnCards = new int[13]; // 0 Ace 11 Jack 12 Queen 13 King

        public static int[] probabilities = {30, 65, 85, 90, 100};

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
                Console.Write("   The value of your hand is: " + Convert.ToString(playerVal) + "\n");
                Console.WriteLine();
                Console.Write("The dealers hand: " + dealersHand[0]);
                for (int i = 1; i < numDealerTurns; i++) {
                    Console.Write("*");
                    if (i == numDealerTurns-1) {
                        Console.Write("\n");
                    } else {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("What would you like to do? Hit/Stand");
                string? response = Console.ReadLine();
                if (response != null) {
                    if (response.ToUpper() == "HIT") {
                        playersHand[numPlayerTurns] = RandomCardDraw(true);
                        numPlayerTurns++;
                        
                        if (playerVal > 21) {
                            if (playerAces > 0) {
                                playerVal -= 10 * playerAces;
                                playerAces = 0;
                                if (playerVal > 21) {
                                    Console.WriteLine();
                                    Console.WriteLine("You drew an ace but you were already on 21, thus you lose.");
                                }
                            } else {
                                Console.WriteLine();
                                Console.WriteLine("You drew a " + playersHand[numPlayerTurns - 1] + " which set you over the value of 21. You lose");
                            }
                        } else {
                            turn = false;
                            game();
                        }
                    } else if (response.ToUpper() == "STAND") {
                        Console.WriteLine();
                        Console.WriteLine("You have decided to stand.");
                        Thread.Sleep(2000);
                        turn = false;
                        game();
                    }
                } else {
                    Console.WriteLine("You cannot input a null value, try again.");
                    Thread.Sleep(3000);
                    game();
                }
            } else {
                if (!dealerStanding) {
                    if (dealerVal < 17) {
                        dealersHand[numDealerTurns] = RandomCardDraw(false);
                        numDealerTurns++;
                    }

                    if (dealerVal >= 17) {
                        Random rand = new Random();
                        int randProb = rand.Next(101);
                        bool willHit = false;
                        switch(dealerVal) {
                            case 17:
                                if (randProb >= probabilities[0]) {
                                    willHit = true;
                                }
                                break;
                            case 18:
                                if (randProb >= probabilities[1]) {
                                    willHit = true;
                                }
                                break;
                            case 19:
                                if (randProb >= probabilities[2]) {
                                    willHit = true;
                                }
                                break;
                            case 20:
                                if (randProb >= probabilities[3]) {
                                    willHit = true;
                                }
                                break;
                            case 21:
                                if (randProb >= probabilities[4]) {
                                    willHit = true;
                                }
                                break;
                            default:
                                Console.WriteLine("Something went wrong with the ai.");
                                Thread.Sleep(10000);
                                break;
                        }

                        if (willHit) {
                            dealersHand[numDealerTurns] = RandomCardDraw(false);
                            numDealerTurns++;
                        }
                    }

                    if (dealerVal > 21) {
                        Console.WriteLine("The dealer has gone over the value of 21, you win!");
                    } else {
                        turn = true;
                        game();
                    }
                } else {
                    Console.WriteLine();
                    Console.WriteLine("The dealer has decided to stand.");
                    Thread.Sleep(3000);
                    turn = true;
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
            game();
        }
    }
}

/* TODO: If both players stand, end game. Try catch for line 94 to let the code run if it is in vsCode debug
Add a betting system using the save.dat file*/
