using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _2020
{
   class Q01
   {
      public static void Go()
      {
         var list = GetList();
         Part1(list);
         Part2(list);
      }

      static List<int> GetList()
      {
         var numList = new List<int>();

         using var fs = new FileStream("01input.txt", FileMode.Open);
         using var sr = new StreamReader(fs);
         string line;
         while ((line = sr.ReadLine()) != null)
         {
            if (int.TryParse(line, out int num))
            {
               numList.Add(num);
            }
         }

         return numList;
      }

      static void Part1(List<int> numList)
      {
         for (int i = 0; i < numList.Count; i++)
         {
            for (int j = i + 1; j < numList.Count; j++)
            {
               if (numList[i] + numList[j] == 2020)
               {
                  Debug.WriteLine($"Q01Part1: idx {i} + idx {j} = 2020. mult = {numList[i] * numList[j]}");
                  return;
               }
            }
         }
      }

      static void Part2(List<int> numList)
      {
         for (int i = 0; i < numList.Count; i++)
         {
            for (int j = i + 1; j < numList.Count; j++)
            {
               for (int k = j + 1; k < numList.Count; k++)
               {
                  if (numList[i] + numList[j] + numList[k] == 2020)
                  {
                     Debug.WriteLine($"Q01Part2: idx {i} + idx {j} + idx {k} = 2020. mult = {numList[i] * numList[j] * numList[k]}");
                     return;
                  }
               }
            }
         }
      }
   }
}
