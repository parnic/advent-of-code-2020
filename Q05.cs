using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _2020
{
   class Q05
   {
      static List<string> list = new List<string>();

      const int numRows = 128;
      const int numCols = 8;

      public static void Go()
      {
         MakeList();
         Part1();
         Part2();
      }

      static void MakeList()
      {
         list = new List<string>(System.IO.File.ReadAllLines("05input.txt"));
      }

      static int BinaryPartition(int upperExcl, IEnumerable<char> input, char frontHalf, char backHalf)
      {
         int retval = -1;

         int lowerBoundIncl = 0;
         int upperBoundExcl = upperExcl;

         int i = 0;
         for (; retval < 0; i++)
         {
            if (input.Count() <= i)
            {
               throw new Exception("Fell off the end of the input");
            }

            int range = upperBoundExcl - lowerBoundIncl;
            int reduceBy = range / 2;

            if (input.ElementAt(i) == frontHalf)
            {
               upperBoundExcl -= reduceBy;
            }
            else if (input.ElementAt(i) == backHalf)
            {
               lowerBoundIncl += reduceBy;
            }
            else
            {
               throw new Exception("Unexpected input");
            }

            if (upperBoundExcl - lowerBoundIncl == 1)
            {
               retval = lowerBoundIncl;
            }
         }

         return retval;
      }

      static Tuple<int, int> GetRowCol(string ticket)
      {
         var fbs = ticket.TakeWhile((c) => c == 'F' || c == 'B');
         int row = BinaryPartition(numRows, fbs, 'F', 'B');
         var lrs = ticket.Skip(fbs.Count()).TakeWhile((c) => c == 'L' || c == 'R');
         int col = BinaryPartition(numCols, lrs, 'L', 'R');

         if (ticket.Length != fbs.Count() + lrs.Count())
         {
            throw new Exception("Invalid input");
         }

         return new Tuple<int, int>(row, col);
      }

      static int GetSeatId(int row, int col)
      {
         return (row * numCols) + col;
      }

      static void Part1()
      {
         int highestId = 0;
         foreach (var ticket in list)
         {
            var (row, col) = GetRowCol(ticket);

            int seatId = GetSeatId(row, col);
            if (seatId > highestId)
            {
               highestId = seatId;
            }
         }

         Debug.WriteLine($"Q05Part1: highest id={highestId}");
      }

      static void Part2()
      {
         var seatList = new bool[numRows, numCols];
         foreach (var ticket in list)
         {
            var (row, col) = GetRowCol(ticket);
            seatList[row, col] = true;
         }

         bool foundValid = false;
         bool foundMine = false;
         int myId = -1;
         for (int row = 0; row < seatList.GetLength(0); row++)
         {
            for (int col = 0; col < seatList.GetLength(1); col++)
            {
               if (seatList[row, col])
               {
                  if (!foundValid && myId >= 0)
                  {
                     if (foundMine)
                     {
                        throw new Exception("Found multiple seats");
                     }

                     Debug.WriteLine($"Q05Part2: your seat={myId}");
                     foundMine = true;
                  }

                  foundValid = true;
               }
               else if (foundValid)
               {
                  myId = GetSeatId(row, col);
                  foundValid = false;
               }
            }
         }
      }
   }
}
