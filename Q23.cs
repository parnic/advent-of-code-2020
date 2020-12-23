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
         Util.Log($"Q23 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q23 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q23 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q23 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         var text = File.ReadAllText("23input.txt");
         foreach (var ch in text)
         {
            list.Add(Convert.ToInt32(ch.ToString()));
         }
      }

      static int mod(int x, int m)
      {
         int r = x;
         if (r >= x)
         {
            r = x % m;
         }
         return r < 0 ? r + m : r;
      }

      static void Part1()
      {
         var cupList = new List<int>(list);
         var currentCupVal = cupList[0];
         var currentCupIdx = 0;
         var pickedUp = new int[3];
         for (int move = 0; move < 100; move++)
         {
            for (int i = currentCupIdx + 1, pickedUpIdx = 0; pickedUpIdx < 3; pickedUpIdx++)
            {
               if (i >= cupList.Count)
               {
                  i = 0;
               }

               pickedUp[pickedUpIdx] = cupList[i];
               cupList.RemoveAt(i);
            }

            var destCupVal = currentCupVal - 1;
            var destCupIdx = cupList.IndexOf(destCupVal);
            while (destCupIdx < 0)
            {
               destCupVal = mod(destCupVal - 1, cupList.Count + pickedUp.Length + 1);
               destCupIdx = cupList.IndexOf(destCupVal);
            }

            cupList.InsertRange(destCupIdx + 1, pickedUp);
            currentCupVal = cupList[mod(cupList.IndexOf(currentCupVal) + 1, cupList.Count)];
            currentCupIdx = cupList.IndexOf(currentCupVal);
         }

         var startIdx = cupList.IndexOf(1);
         var scoreStr = "";
         for (var idx = startIdx + 1; scoreStr.Length < cupList.Count - 1; idx++)
         {
            scoreStr += cupList[idx % cupList.Count].ToString();
         }

         Util.Log($"Q23Part1: labels={scoreStr}");
      }

      static void Part2()
      {
         var cupList = new List<int>(list);
         var highestVal = cupList.Aggregate(0, (result, curr) => curr > result ? curr : result);
         for (int i = cupList.Count, next = highestVal + 1; i < 1000000; i++, next++)
         {
            cupList.Add(next);
         }

         var currentCupVal = cupList[0];
         var currentCupIdx = 0;
         var pickedUp = new int[3];
         var start = DateTime.Now;
         var lastMove = 0;
         for (int move = 0; move < 10000000; move++)
         {
            for (int i = currentCupIdx + 1, pickedUpIdx = 0; pickedUpIdx < 3; pickedUpIdx++)
            {
               if (i >= cupList.Count)
               {
                  i = 0;
               }

               pickedUp[pickedUpIdx] = cupList[i];
               cupList.RemoveAt(i);
            }

            var destCupVal = currentCupVal - 1;
            var destCupIdx = cupList.IndexOf(destCupVal);
            while (destCupIdx < 0)
            {
               destCupVal = mod(destCupVal - 1, cupList.Count + pickedUp.Length + 1);
               destCupIdx = cupList.IndexOf(destCupVal);
            }

            cupList.InsertRange(destCupIdx + 1, pickedUp);
            currentCupVal = cupList[mod(cupList.IndexOf(currentCupVal) + 1, cupList.Count)];
            currentCupIdx = cupList.IndexOf(currentCupVal);

            var dt = DateTime.Now - start;
            var dtSeconds = dt.TotalSeconds;
            if (dtSeconds >= 1.0)
            {
               start = DateTime.Now;
               var rate = (move - lastMove) / dtSeconds;
               lastMove = move;
               var remainingMoves = 10000000 - move;
               var endTime = DateTime.Now + TimeSpan.FromSeconds(remainingMoves / rate);
               Util.Log($"Current rate: {rate:N1} mps, {remainingMoves:N} moves remaining. Should be done by {endTime}");
            }
         }

         var oneIdx = cupList.IndexOf(1);
         Util.Log($"Q23Part2: next two={cupList[(oneIdx + 1) % cupList.Count]}, {cupList[(oneIdx + 2) % cupList.Count]}, multiplied={1L * cupList[(oneIdx + 1) % cupList.Count] * cupList[(oneIdx + 2) % cupList.Count]}");
      }
   }
}
