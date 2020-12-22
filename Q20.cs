using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q20
   {
      static readonly List<Tile> list = new List<Tile>();
      const int TileSize = 10;

      [DebuggerDisplay("{ID}")]
      private class Tile
      {
         public enum SideName { Top, TopReverse, Left, LeftReverse, Bottom, BottomReverse, Right, RightReverse }
         public int ID { get; private set; }
         public char[,] Data = new char[TileSize, TileSize];
         public bool IsPlaced { get; set; }
         public IEnumerable<int> Sides => Enum.GetValues(typeof(SideName)).Cast<SideName>().Select(n => GetSide(n));

         public Tile(int id, string[] lines)
         {
            ID = id;
            for (int i = 0; i < lines.Length; i++)
            {
               for (int j = 0; j < lines[i].Length; j++)
               {
                  Data[i, j] = lines[i][j];
               }
            }
         }

         public int GetSide(SideName name)
         {
            return name switch
            {
               SideName.Top => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[0, r]).ToArray())),
               SideName.TopReverse => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[0, r]).Reverse().ToArray())),
               SideName.Left => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[r, 0]).ToArray())),
               SideName.LeftReverse => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[r, 0]).Reverse().ToArray())),
               SideName.Bottom => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[TileSize - 1, r]).ToArray())),
               SideName.BottomReverse => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[TileSize - 1, r]).Reverse().ToArray())),
               SideName.Right => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[r, TileSize - 1]).ToArray())),
               SideName.RightReverse => ParseNumber(new string(Enumerable.Range(0, TileSize).Select(r => Data[r, TileSize - 1]).Reverse().ToArray())),
               _ => -1,
            };
         }

         public void Rotate()
         {
            RotateSquareCW(ref Data);
         }

         public void FlipHorizontally()
         {
            Data = FlipH(Data);
         }

         public void FlipVertically()
         {
            Data = FlipV(Data);
         }

         public int Matches(IEnumerable<Tile> tiles)
         {
            return tiles.Count(t => t.ID != ID && t.Sides.Any(s => Sides.Contains(s)));
         }

         private static int ParseNumber(string text)
         {
            return Convert.ToInt32(text.Replace('#', '1').Replace('.', '0'), 2);
         }
      }

      static void RotateSquareCW(ref char[,] source)
      {
         var n = source.GetLength(0);
         for (int i = 0; i < n / 2; i++)
         {
            for (int j = i; j < n - i - 1; j++)
            {
               var temp = source[i, j];
               source[i, j] = source[n - 1 - j, i];
               source[n - 1 - j, i] = source[n - 1 - i, n - 1 - j];
               source[n - 1 - i, n - 1 - j] = source[j, n - 1 - i];
               source[j, n - 1 - i] = temp;
            }
         }
      }

      static char[,] FlipH(char[,] source)
      {
         var numRows = source.GetLength(0);
         var numCols = source.GetLength(1);
         char[,] temp = new char[numRows, numCols];
         for (int row = 0; row < numRows; row++)
         {
            for (int col = 0; col < numCols; col++)
            {
               temp[row, numCols - col - 1] = source[row, col];
            }
         }

         return temp;
      }

      static char[,] FlipV(char[,] source)
      {
         var numRows = source.GetLength(0);
         var numCols = source.GetLength(1);
         char[,] temp = new char[numRows, numCols];
         for (int row = 0; row < numRows; row++)
         {
            for (int col = 0; col < numCols; col++)
            {
               temp[numRows - row - 1, col] = source[row, col];
            }
         }

         return temp;
      }

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q20 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q20 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q20 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q20 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         string[] currTile = new string[TileSize];
         int tileNum = 0;
         int rowNum = 0;
         foreach (var line in File.ReadAllLines("20input.txt"))
         {
            if (string.IsNullOrWhiteSpace(line))
            {
               list.Add(new Tile(tileNum, currTile));
               currTile = new string[TileSize];
               rowNum = 0;
            }
            else if (line.StartsWith("Tile"))
            {
               tileNum = Convert.ToInt32(line["Tile ".Length..^1]);
            }
            else
            {
               currTile[rowNum] = line;
               rowNum++;
            }
         }

         list.Add(new Tile(tileNum, currTile));
      }

      static int CountMonsters(char[,] input)
      {
         var count = 0;
         for (int col = 0; col < input.GetLength(1) - monster.GetLength(1); col++)
         {
            for (int row = 0; row < input.GetLength(0) - monster.GetLength(0); row++)
            {
               if (HasMonster(input, row, col))
               {
                  count++;
               }
            }
         }

         return count;
      }

      static readonly char[,] monster = new char[3, 20]
      {
         { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.' },
         { '#', '.', '.', '.', '.', '#', '#', '.', '.', '.', '.', '#', '#', '.', '.', '.', '.', '#', '#', '#' },
         { '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '#', '.', '.', '.' },
      };

      static int MonsterSize
      {
         get
         {
            int size = 0;
            for (int i = 0; i < monster.GetLength(0); i++)
            {
               for (int j = 0; j < monster.GetLength(1); j++)
               {
                  if (monster[i, j] == '#')
                  {
                     size++;
                  }
               }
            }

            return size;
         }
      }

      static bool HasMonster(char[,] grid, int row, int col)
      {
         for (int c2 = 0; c2 < 20; c2++)
         {
            for (int r2 = 0; r2 < 3; r2++)
            {
               if (monster[r2, c2] == '#' && grid[col + c2, row + r2] != '#')
               {
                  return false;
               }
            }
         }

         return true;
      }

      static List<Tile> _corners;
      static List<Tile> Corners
      {
         get
         {
            if (_corners == null)
            {
               _corners = list.Where(t => t.Matches(list) == 2).ToList();
            }

            return _corners;
         }
      }

      static void Part1()
      {
         Util.Log($"Q20Part1: {Corners.Aggregate(1L, (i, v) => i * v.ID)}");
      }

      static void Part2()
      {
         var sides = new Dictionary<int, List<Tile>>(
             list.SelectMany(t => t.Sides.Select(s => new { s, t }))
             .GroupBy(s => s.s)
             .Where(s => s.Count() > 1)
             .Select(s => new KeyValuePair<int, List<Tile>>(s.Key, s.Select(i => i.t).ToList()))
         );

         var gridSize = (int)Math.Sqrt(list.Count);
         var assembled = new Tile[gridSize, gridSize];
         for (int col = 0; col < gridSize; col++)
         {
            for (int row = 0; row < gridSize; row++)
            {
               Tile nextTile;
               if (col == 0 && row == 0) // Find the corner and orient
               {
                  nextTile = Corners.First();
                  while (!sides.ContainsKey(nextTile.GetSide(Tile.SideName.Right)) || !sides.ContainsKey(nextTile.GetSide(Tile.SideName.Bottom)))
                  {
                     nextTile.Rotate();
                  }
               }
               else if (row == 0) // top row: find the tile that fits to the right of the last one
               {
                  var lastRight = assembled[row, col - 1].GetSide(Tile.SideName.Right);
                  nextTile = sides[lastRight].Single(e => !e.IsPlaced);
                  // Rotate until the right side is on the left
                  while (nextTile.GetSide(Tile.SideName.Left) != lastRight && nextTile.GetSide(Tile.SideName.LeftReverse) != lastRight)
                  {
                     nextTile.Rotate();
                  }
                  // Flip if necessary
                  if (nextTile.GetSide(Tile.SideName.Left) != lastRight)
                  {
                     nextTile.FlipVertically();
                  }
               }
               else // find the tile that fits the one above
               {
                  var lastBottom = assembled[row - 1, col].GetSide(Tile.SideName.Bottom);
                  nextTile = sides[lastBottom].Single(e => !e.IsPlaced);
                  while (nextTile.GetSide(Tile.SideName.Top) != lastBottom && nextTile.GetSide(Tile.SideName.TopReverse) != lastBottom)
                  {
                     nextTile.Rotate();
                  }
                  if (nextTile.GetSide(Tile.SideName.Top) != lastBottom)
                  {
                     nextTile.FlipHorizontally();
                  }
               }
               assembled[row, col] = nextTile;
               nextTile.IsPlaced = true;
            }
         }

         // Extract the final image
         var imgDimensions = (TileSize - 2) * gridSize;
         var grid = new char[imgDimensions, imgDimensions];
         var innerWidth = TileSize - 2;
         for (int col = 0; col < gridSize; col++)
         {
            for (int tileColIdx = 1; tileColIdx < innerWidth + 1; tileColIdx++)
            {
               for (int row = 0; row < gridSize; row++)
               {
                  for (int tileRowIdx = 1; tileRowIdx < innerWidth + 1; tileRowIdx++)
                  {
                     grid[(col * innerWidth) + tileColIdx - 1, (row * innerWidth) + tileRowIdx - 1] = assembled[row, col].Data[tileRowIdx, tileColIdx];
                  }
               }
            }
         }

         var waterCount = grid.Cast<char>().Count(c => c == '#');
         var roughness = waterCount;
         var numSides = 4;
         for (int i = 0; i < numSides * 2; i++)
         {
            var count = CountMonsters(grid);
            if (count > 0)
            {
               roughness = waterCount - count * MonsterSize;
            }

            if (i == numSides)
            {
               grid = FlipH(grid);
            }
            else
            {
               RotateSquareCW(ref grid);
            }
         }

         Util.Log($"Q20Part2: roughness={roughness}");
      }
   }
}
