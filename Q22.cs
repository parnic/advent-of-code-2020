using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q22
   {
      static Dictionary<int, List<int>> list = new Dictionary<int, List<int>>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q22 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q22 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q22 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q22 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         var currPlayer = 0;
         foreach (var line in File.ReadAllLines("22input.txt"))
         {
            if (line.StartsWith("Player "))
            {
               currPlayer = Convert.ToInt32(line["Player ".Length..^1]);
               list.Add(currPlayer, new List<int>());
            }
            else if (string.IsNullOrWhiteSpace(line))
            {
               continue;
            }
            else
            {
               list[currPlayer].Add(Convert.ToInt32(line));
            }
         }
      }

      static string MakeHandsGUID(IEnumerable<int> p1deck, IEnumerable<int> p2deck)
      {
         return string.Join(",", string.Join("-", p1deck), string.Join("-", p2deck));
      }

      static void HandleRoundWinner(bool p1Wins, ICollection<int> p1deck, ICollection<int> p2deck, int p1card, int p2card)
      {
         var deck = p1Wins ? p1deck : p2deck;
         var firstCard = p1Wins ? p1card : p2card;
         var secondCard = p1Wins ? p2card : p1card;

         deck.Add(firstCard);
         deck.Add(secondCard);
      }

      static int gameNum = 0;
      static int deepestLevel = 0;
      static int currLevel = 0;
      static int PlayGame(ICollection<int> p1deck, ICollection<int> p2deck, bool recursiveGame = false)
      {
         gameNum++;
         currLevel++;
         if (currLevel > deepestLevel)
         {
            deepestLevel = currLevel;
         }

         Dictionary<string, bool> prevHands = null;
         while (p1deck.Any() && p2deck.Any())
         {
            if (recursiveGame)
            {
               if (prevHands == null)
               {
                  prevHands = new Dictionary<string, bool>();
               }

               var guid = MakeHandsGUID(p1deck, p2deck);
               if (prevHands.ContainsKey(guid))
               {
                  currLevel--;
                  return 1;
               }

               prevHands.Add(guid, true);
            }

            var p1card = p1deck.PopFront();
            var p2card = p2deck.PopFront();
            if (recursiveGame)
            {
               if (p1deck.Count >= p1card && p2deck.Count >= p2card)
               {
                  var winner = PlayGame(new List<int>(p1deck.Take(p1card)), new List<int>(p2deck.Take(p2card)), recursiveGame);
                  HandleRoundWinner(winner == 1, p1deck, p2deck, p1card, p2card);
                  continue;
               }
            }

            HandleRoundWinner(p1card > p2card, p1deck, p2deck, p1card, p2card);
         }

         currLevel--;

         return p1deck.Any() ? 1 : 2;
      }

      static int GetScore(IEnumerable<int> winnerDeck)
      {
         var score = 0;
         var deckSize = winnerDeck.Count();
         var idx = 0;
         foreach (var card in winnerDeck)
         {
            score += (deckSize - idx) * card;
            idx++;
         }

         return score;
      }

      static void Part1()
      {
         var p1deck = new List<int>(list[1]);
         var p2deck = new List<int>(list[2]);
         var winner = PlayGame(p1deck, p2deck);
         var score = GetScore(winner == 1 ? p1deck : p2deck);

         Util.Log($"Q22Part1: winner=p{winner}, score={score}");
      }

      static void Part2()
      {
         var p1deck = new List<int>(list[1]);
         var p2deck = new List<int>(list[2]);
         var winner = PlayGame(p1deck, p2deck, recursiveGame: true);
         var score = GetScore(winner == 1 ? p1deck : p2deck);

         Util.Log($"Q22Part2: games played={gameNum}, deepest level={deepestLevel}, winner=p{winner}, score={score}");
      }
   }
}
