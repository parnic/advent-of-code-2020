using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q24
   {
      static readonly List<List<string>> list = new List<List<string>>();
      static Dictionary<Tuple<int, int>, int> tiles = new Dictionary<Tuple<int, int>, int>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q24 MakeList took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q24 part1 took {(DateTime.Now - p1start).TotalMilliseconds:N0}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q24 part2 took {(DateTime.Now - p2start).TotalMilliseconds:N0}ms");

         Util.Log($"Q24 took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("24input.txt"))
         {
            var sublist = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
               if (line[i] == 'e' || line[i] == 'w')
               {
                  sublist.Add(line[i].ToString());
               }
               else
               {
                  i++;
                  sublist.Add($"{line[i - 1]}{line[i]}");
               }
            }

            list.Add(sublist);
         }
      }

      static void Part1()
      {
         foreach (var inst in list)
         {
            var row = 0;
            var col = 0;
            foreach (var step in inst)
            {
               if (step.StartsWith('n'))
               {
                  row--;
               }
               else if (step.StartsWith('s'))
               {
                  row++;
               }
               
               if (step.EndsWith('e'))
               {
                  col++;
                  if (step.Length == 1)
                  {
                     col++;
                  }
               }
               else if (step.EndsWith('w'))
               {
                  col--;
                  if (step.Length == 1)
                  {
                     col--;
                  }
               }
            }

            var tilePos = Tuple.Create(row, col);
            if (!tiles.ContainsKey(tilePos))
            {
               tiles.Add(tilePos, 0);
            }
            tiles[tilePos]++;
         }

         var flipped = tiles.Values.Aggregate(0, (sum, curr) => IsFlipped(curr) ? (sum + 1) : sum);

         Util.Log($"Q24Part1: flipped={flipped}");
      }
      
      static List<Tuple<int, int>> GetNeighbors(Tuple<int, int> coords)
      {
         var result = new List<Tuple<int, int>>();
         result.Add(Tuple.Create(coords.Item1 - 1, coords.Item2 - 1));
         result.Add(Tuple.Create(coords.Item1    , coords.Item2 - 2));
         result.Add(Tuple.Create(coords.Item1 + 1, coords.Item2 - 1));
         result.Add(Tuple.Create(coords.Item1 - 1, coords.Item2 + 1));
         result.Add(Tuple.Create(coords.Item1    , coords.Item2 + 2));
         result.Add(Tuple.Create(coords.Item1 + 1, coords.Item2 + 1));
         return result;
      }

      static bool IsFlipped(int val)
      {
         return (val & 1) == 1;
      }

      static int NumNeighborsFlipped(Tuple<int, int> coords)
      {
         var result = 0;
         foreach (var neighbor in GetNeighbors(coords))
         {
            if (tiles.ContainsKey(neighbor) && IsFlipped(tiles[neighbor]))
            {
               result++;
            }
         }
         return result;
      }

      static Dictionary<Tuple<int, int>, int> PopulateNeighbors(Dictionary<Tuple<int, int>, int> inList)
      {
         var tempList = new Dictionary<Tuple<int, int>, int>(inList);
         foreach (var pair in inList)
         {
            if (IsFlipped(pair.Value))
            {
               foreach (var neighbor in GetNeighbors(pair.Key))
               {
                  if (!tempList.ContainsKey(neighbor))
                  {
                     tempList.Add(neighbor, 0);
                  }
               }
            }
         }

         return tempList;
      }

      static void Part2()
      {
         for (int day = 0; day < 100; day++)
         {
            var tileState = PopulateNeighbors(tiles);
            foreach (var pair in tileState)
            {
               var numNeighborsFlipped = NumNeighborsFlipped(pair.Key);
               var isFlipped = IsFlipped(pair.Value);
               if (isFlipped && (numNeighborsFlipped == 0 || numNeighborsFlipped > 2))
               {
                  tileState[pair.Key]++;
               }
               else if (!isFlipped && numNeighborsFlipped == 2)
               {
                  tileState[pair.Key]++;
               }
            }

            tiles = new Dictionary<Tuple<int, int>, int>(tileState);
         }

         var flipped = tiles.Values.Aggregate(0, (sum, curr) => IsFlipped(curr) ? (sum + 1) : sum);
         Util.Log($"Q24Part2: flipped={flipped}");
      }
   }
}
