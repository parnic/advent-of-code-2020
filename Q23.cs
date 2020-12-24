using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q23
   {
      static readonly List<int> list = new List<int>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q23 MakeList took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q23 part1 took {(DateTime.Now - p1start).TotalMilliseconds:N0}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q23 part2 took {(DateTime.Now - p2start).TotalMilliseconds:N0}ms");

         Util.Log($"Q23 took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
      }

      static void MakeList()
      {
         var text = File.ReadAllText("23input.txt");
         foreach (var ch in text)
         {
            list.Add(Convert.ToInt32(ch.ToString()));
         }
      }

      static int GetNextCup(int[] cupList, int currCup)
      {
         var next = cupList[currCup];
         if (next == 0)
         {
            return GetNextCup(cupList, next);
         }

         return next;
      }

      static void Solve(int[] cupList, int iterations)
      {
         var currentCup = cupList[0];
         var pickedUpCups = new int[3];
         for (int move = 0; move < iterations; move++)
         {
            var pickedUp = GetNextCup(cupList, currentCup);
            var nextCup = pickedUp;
            for (int i = 0; i < pickedUpCups.Length; i++)
            {
               pickedUpCups[i] = nextCup;
               nextCup = GetNextCup(cupList, nextCup);
            }

            cupList[currentCup] = nextCup;

            var destinationCup = currentCup - 1;
            while (pickedUpCups.Contains(destinationCup) || destinationCup == 0)
            {
               destinationCup--;
               if (destinationCup <= 0)
               {
                  destinationCup = cupList.Length - 1;
               }
            }

            var oldDestinationNext = GetNextCup(cupList, destinationCup);
            cupList[destinationCup] = pickedUp;
            cupList[pickedUpCups[^1]] = oldDestinationNext;
            currentCup = GetNextCup(cupList, currentCup);
         }
      }

      static void Part1()
      {
         var cupList = new int[list.Count + 1];
         for (int i = 0, currIdx = 0; i < list.Count; i++)
         {
            cupList[currIdx] = list[i];
            currIdx = list[i];
         }

         Solve(cupList, 100);

         var cup = cupList[1];
         var scoreStr = "";
         while (cup != 1)
         {
            scoreStr += cup.ToString();
            cup = GetNextCup(cupList, cup);
         }

         Util.Log($"Q23Part1: labels={scoreStr}");
      }

      static void Part2()
      {
         var cupList = new int[1_000_001];
         var currIdx = 0;
         foreach (var cup in list)
         {
            cupList[currIdx] = cup;
            currIdx = cup;
         }
         var highest = cupList.First(x => cupList[x] == 0);
         cupList[highest] = list.Count + 1;
         for (int i = cupList[highest]; i < cupList.Length; i++)
         {
            cupList[i] = i == cupList.Length - 1 ? 0 : i + 1;
         }

         Solve(cupList, 10_000_000);

         var first = cupList[1];
         var second = cupList[first];
         Util.Log($"Q23Part2: first={first}, second={second}, mult={1L * first * second}");
      }
   }
}
