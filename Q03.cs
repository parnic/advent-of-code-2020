using System.Collections.Generic;
using System.IO;

namespace _2020
{
   class Q03
   {
      static bool[,] List = null;
      public static void Go()
      {
         List = GetList();
         Part1();
         Part2();
      }

      static bool[,] GetList()
      {
         var list = new List<List<bool>>();

         using var fs = new FileStream("03input.txt", FileMode.Open);
         using var sr = new StreamReader(fs);
         string line;
         while ((line = sr.ReadLine()) != null)
         {
            list.Add(new List<bool>());
            for (int i = 0; i < line.Length; i++)
            {
               list[^1].Add(line[i] != '.');
            }
         }

         var retval = new bool[list.Count, list[0].Count];
         for (int i = 0; i < list.Count; i++)
         {
            for (int j = 0; j < list[i].Count; j++)
            {
               retval[i, j] = list[i][j];
            }
         }
         return retval;
      }

      static int GetNumTrees(int AddRow, int AddCol)
      {
         var numRows = List.GetLength(0);
         var numCols = List.GetLength(1);

         var numTrees = 0;
         for (int rowPos = 0, colPos = 0; rowPos < numRows; rowPos += AddRow, colPos += AddCol)
         {
            if (List[rowPos, colPos % numCols])
            {
               numTrees++;
            }
         }

         return numTrees;
      }

      static void Part1()
      {
         Util.Log($"Q03Part1: num trees hit = {GetNumTrees(1, 3)}");
      }

      static void Part2()
      {
         var treesPerSlope = new List<int>()
         {
            GetNumTrees(1, 1),
            GetNumTrees(1, 3),
            GetNumTrees(1, 5),
            GetNumTrees(1, 7),
            GetNumTrees(2, 1),
         };

         long multSum = 1;
         foreach (var trees in treesPerSlope)
         {
            multSum *= trees;
         }
         Util.Log($"Q03Part2: num trees hit, multiplied = {multSum}");
      }
   }
}
