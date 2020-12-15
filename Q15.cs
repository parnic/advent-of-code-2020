using System;
using System.Collections.Generic;
using System.IO;

namespace _2020
{
   class Q15
   {
      static List<int> list = new List<int>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q15 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q15 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q15 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q15 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         var input = File.ReadAllText("15input.txt");
         foreach (var num in input.Split(','))
         {
            list.Add(Convert.ToInt32(num));
         }
      }

      static int GetNumAt(int turn)
      {
         var numsSaidDict = new Dictionary<int, List<int>>();

         int lastNumSaid = 0;
         List<int> entry = null;
         for (int i = 0; i < turn; i++)
         {
            if (i < list.Count)
            {
               numsSaidDict.Add(list[i], new List<int>() { i });
               lastNumSaid = list[i];
            }
            else
            {
               if (i == list.Count)
               {
                  lastNumSaid = 0;
               }
               else
               {
                  if (entry != null)
                  {
                     lastNumSaid = entry.Count == 1 ? 0 : entry[^1] - entry[^2];
                  }
                  else
                  {
                     lastNumSaid = 0;
                  }
               }

               numsSaidDict.TryGetValue(lastNumSaid, out entry);
               if (entry == null)
               {
                  entry = new List<int>();
                  numsSaidDict.Add(lastNumSaid, entry);
               }

               entry.Add(i);
            }
         }

         return lastNumSaid;
      }

      static void Part1()
      {
         Util.Log($"Q15Part1: last num said={GetNumAt(2020)}");
      }

      static void Part2()
      {
         Util.Log($"Q15Part2: last num said={GetNumAt(30000000)}");
      }
   }
}
