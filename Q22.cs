using System;
using System.Collections.Generic;
using System.Diagnostics;
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

      static void Part1()
      {
         var p1deck = new List<int>(list[1]);
         var p2deck = new List<int>(list[2]);
         while (p1deck.Any() && p2deck.Any())
         {
            var p1card = p1deck.First();
            p1deck.RemoveAt(0);
            var p2card = p2deck.First();
            p2deck.RemoveAt(0);
            if (p1card > p2card)
            {
               p1deck.Add(p1card);
               p1deck.Add(p2card);
            }
            else
            {
               p2deck.Add(p2card);
               p2deck.Add(p1card);
            }
         }

         var winner = p1deck.Count == 0 ? p2deck : p1deck;
         var score = 0;
         for (int i = winner.Count - 1; i >= 0; i--)
         {
            score += (i + 1) * winner[winner.Count - i - 1];
         }

         Util.Log($"Q22Part1: score={score}");
      }

      static int gameNum = 0;
      static int deepestLevel = 0;
      static int currLevel = 0;
      static int PlayRecursiveGame(List<int> p1deck, List<int> p2deck)
      {
         gameNum++;
         currLevel++;
         if (currLevel > deepestLevel)
         {
            deepestLevel = currLevel;
         }

         var p1prev = new List<List<int>>();
         var p2prev = new List<List<int>>();
         while (p1deck.Any() && p2deck.Any())
         {
            if (p1prev.Any(x => Enumerable.SequenceEqual(p1deck, x)) && p2prev.Any(x => Enumerable.SequenceEqual(p2deck, x)))
            {
               currLevel--;
               return 1;
            }

            p1prev.Add(new List<int>(p1deck));
            p2prev.Add(new List<int>(p2deck));

            var p1card = p1deck.First();
            p1deck.RemoveAt(0);
            var p2card = p2deck.First();
            p2deck.RemoveAt(0);
            if (p1deck.Count >= p1card && p2deck.Count >= p2card)
            {
               var winner = PlayRecursiveGame(new List<int>(p1deck.Take(p1card)), new List<int>(p2deck.Take(p2card)));
               if (winner == 1)
               {
                  p1deck.Add(p1card);
                  p1deck.Add(p2card);
               }
               else
               {
                  p2deck.Add(p2card);
                  p2deck.Add(p1card);
               }
            }
            else
            {
               if (p1card > p2card)
               {
                  p1deck.Add(p1card);
                  p1deck.Add(p2card);
               }
               else
               {
                  p2deck.Add(p2card);
                  p2deck.Add(p1card);
               }
            }
         }

         currLevel--;

         return p1deck.Any() ? 1 : 2;
      }

      static void Part2()
      {
         var winner = PlayRecursiveGame(list[1], list[2]);

         var score = 0;
         var winnerDeck = list[winner];
         for (int i = winnerDeck.Count - 1; i >= 0; i--)
         {
            score += (i + 1) * winnerDeck[winnerDeck.Count - i - 1];
         }

         Util.Log($"Q22Part2: games played={gameNum}, deepest level={deepestLevel}, score={score}");
      }
   }
}
