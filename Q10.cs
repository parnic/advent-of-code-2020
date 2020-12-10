using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _2020
{
   class Q10
   {
      static List<long> list = new List<long>();
      static List<long> sortedList = null;

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Debug.WriteLine($"Q10 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Debug.WriteLine($"Q10 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Debug.WriteLine($"Q10 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Debug.WriteLine($"Q10 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         list.Add(0);

         foreach (var line in File.ReadAllLines("10input.txt"))
         {
            list.Add(Convert.ToInt64(line));
         }

         sortedList = new List<long>(list);
         sortedList.Sort();
         sortedList.Add(sortedList[^1] + 3);
      }

      static void Part1()
      {
         var diffs = new int[3];
         for (int i = 1; i < sortedList.Count; i++)
         {
            diffs[sortedList[i] - sortedList[i - 1] - 1]++;
         }

         Debug.WriteLine($"Q10Part1: device joltage={sortedList[^1]}, diffs by 1={diffs[0]}, diffs by 3={diffs[2]}, multiplied={diffs[0] * diffs[2]}");
      }

      static void Part2()
      {
         var pathsToIndices = new List<long>();
         pathsToIndices.Add(1);
         for (int i = 1; i < sortedList.Count; i++)
         {
            long pathLen = 0;
            for (int j = i - 1; j >= 0; j--)
            {
               if (sortedList[j] + 3 >= sortedList[i])
               {
                  pathLen += pathsToIndices[j];
               }
               else
               {
                  break;
               }
            }

            pathsToIndices.Add(pathLen);
         }

         Debug.WriteLine($"Q10Part2: combinations={pathsToIndices[^1]}");
      }
   }
}
