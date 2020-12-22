using System;
using System.Collections.Generic;
using System.IO;

namespace _2020
{
   class Q09
   {
      static readonly List<long> list = new List<long>();

      const int chunkLength = 25;

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q09 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q09 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q09 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q09 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("09input.txt"))
         {
            list.Add(Convert.ToInt64(line));
         }
      }

      static int GetIndexOfInvalidNumber()
      {
         int retval = -1;
         for (int i = chunkLength; i < list.Count; i++)
         {
            var bFound = false;
            for (int j = i - chunkLength; j < i && !bFound; j++)
            {
               for (int k = j + 1; k < i && !bFound; k++)
               {
                  if (list[j] + list[k] == list[i])
                  {
                     bFound = true;
                  }
               }
            }

            if (!bFound)
            {
               if (retval == -1)
               {
                  retval = i;
               }
               else
               {
                  throw new Exception("Found multiple invalid numbers");
               }
            }
         }

         return retval;
      }

      static void Part1()
      {
         var idx = GetIndexOfInvalidNumber();

         if (idx >= 0)
         {
            Util.Log($"Q09Part1: found invalid number={list[idx]}");
         }
         else
         {
            throw new Exception("Didn't find an invalid number");
         }
      }

      static void Part2()
      {
         var idx = GetIndexOfInvalidNumber();
         var targetNum = list[idx];

         int sumRangeStartIdx = -1;
         int sumRangeEndIdx = -1;
         for (int i = 0; i < list.Count; i++)
         {
            var val = list[i];
            for (int j = i + 1; j < list.Count; j++)
            {
               val += list[j];
               if (val == targetNum)
               {
                  if (sumRangeStartIdx != -1 || sumRangeEndIdx != -1)
                  {
                     throw new Exception("Found two invalid ranges");
                  }

                  sumRangeStartIdx = i;
                  sumRangeEndIdx = j;
               }
            }
         }

         if (sumRangeStartIdx < 0 || sumRangeEndIdx < 0)
         {
            throw new Exception("Didn't find valid range");
         }

         long smallestInRange = long.MaxValue;
         long largestInRange = long.MinValue;
         for (int i = sumRangeStartIdx; i <= sumRangeEndIdx; i++)
         {
            if (list[i] < smallestInRange)
            {
               smallestInRange = list[i];
            }

            if (list[i] > largestInRange)
            {
               largestInRange = list[i];
            }
         }

         Util.Log($"Q09Part2: encryption weakness={smallestInRange + largestInRange}");
      }
   }
}
