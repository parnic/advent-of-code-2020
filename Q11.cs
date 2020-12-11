using System;
using System.Diagnostics;
using System.IO;

namespace _2020
{
   class Q11
   {
      static SeatState[,] list = null;
      static int numRows = 0;
      static int numCols = 0;

      enum SeatState
      {
         Floor,
         Empty,
         Occupied,
      }

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Debug.WriteLine($"Q11 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Debug.WriteLine($"Q11 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Debug.WriteLine($"Q11 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Debug.WriteLine($"Q11 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         var lines = File.ReadAllLines("11input.txt");
         numRows = lines.Length;
         numCols = lines[0].Length;

         list = new SeatState[numRows, numCols];
         for (int i = 0; i < numRows; i++)
         {
            for (int j = 0; j < numCols; j++)
            {
               list[i, j] = lines[i][j] == '.' ? SeatState.Floor : SeatState.Empty;
            }
         }
      }

      static int GetNumOccupiedAdjacent(SeatState[,] seatList, int row, int col)
      {
         int numOccupied = 0;
         if (row - 1 >= 0)
         {
            if (col - 1 >= 0 && seatList[row - 1, col - 1] == SeatState.Occupied)
            {
               numOccupied++;
            }
            if (seatList[row - 1, col] == SeatState.Occupied)
            {
               numOccupied++;
            }
            if (col + 1 < numCols && seatList[row - 1, col + 1] == SeatState.Occupied)
            {
               numOccupied++;
            }
         }

         if (col - 1 >= 0 && seatList[row, col - 1] == SeatState.Occupied)
         {
            numOccupied++;
         }
         if (col + 1 < numCols && seatList[row, col + 1] == SeatState.Occupied)
         {
            numOccupied++;
         }

         if (row + 1 < numRows)
         {
            if (col - 1 >= 0 && seatList[row + 1, col - 1] == SeatState.Occupied)
            {
               numOccupied++;
            }
            if (seatList[row + 1, col] == SeatState.Occupied)
            {
               numOccupied++;
            }
            if (col + 1 < numCols && seatList[row + 1, col + 1] == SeatState.Occupied)
            {
               numOccupied++;
            }
         }

         return numOccupied;
      }

      static int GetNumOccupiedLOS(SeatState[,] seatList, int row, int col)
      {
         int numOccupied = 0;

         Func<int, int, bool> checkSeat = (checkRow, checkCol) =>
         {
            if (seatList[checkRow, checkCol] == SeatState.Occupied)
            {
               numOccupied++;
               return true;
            }
            if (seatList[checkRow, checkCol] == SeatState.Empty)
            {
               return true;
            }

            return false;
         };

         // ul
         for (int checkRow = row - 1, checkCol = col - 1; checkRow >= 0 && checkCol >= 0; checkRow--, checkCol--)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // u
         for (int checkRow = row - 1, checkCol = col; checkRow >= 0; checkRow--)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // ur
         for (int checkRow = row - 1, checkCol = col + 1; checkRow >= 0 && checkCol < numCols; checkRow--, checkCol++)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // l
         for (int checkRow = row, checkCol = col - 1; checkCol >= 0; checkCol--)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // r
         for (int checkRow = row, checkCol = col + 1; checkCol < numCols; checkCol++)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // dl
         for (int checkRow = row + 1, checkCol = col - 1; checkRow < numRows && checkCol >= 0; checkRow++, checkCol--)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // d
         for (int checkRow = row + 1, checkCol = col; checkRow < numRows; checkRow++)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         // dr
         for (int checkRow = row + 1, checkCol = col + 1; checkRow < numRows && checkCol < numCols; checkRow++, checkCol++)
         {
            if (checkSeat(checkRow, checkCol))
            {
               break;
            }
         }

         return numOccupied;
      }

      static void Part1()
      {
         SeatState[,] currState = new SeatState[numRows, numCols];
         SeatState[,] nextState = new SeatState[numRows, numCols];
         Array.Copy(list, currState, numRows * numCols);

         int rounds = 0;
         int totalNumOccupied = 0;
         for (int numChanges = 0; rounds == 0 || numChanges != 0; rounds++)
         {
            Array.Copy(currState, nextState, numRows * numCols);

            totalNumOccupied = 0;
            numChanges = 0;

            for (int i = 0; i < numRows; i++)
            {
               for (int j = 0; j < numCols; j++)
               {
                  var numOccupied = GetNumOccupiedAdjacent(currState, i, j);
                  if (currState[i, j] == SeatState.Empty && numOccupied == 0)
                  {
                     nextState[i, j] = SeatState.Occupied;
                     numChanges++;
                  }
                  else if (currState[i, j] == SeatState.Occupied && numOccupied >= 4)
                  {
                     nextState[i, j] = SeatState.Empty;
                     numChanges++;
                  }

                  if (nextState[i, j] == SeatState.Occupied)
                  {
                     totalNumOccupied++;
                  }
               }
            }

            Array.Copy(nextState, currState, numRows * numCols);
         }

         Debug.WriteLine($"Q11Part1: stabilized after {rounds - 1} rounds, total occupied={totalNumOccupied}");
      }

      static void Part2()
      {
         SeatState[,] currState = new SeatState[numRows, numCols];
         SeatState[,] nextState = new SeatState[numRows, numCols];
         Array.Copy(list, currState, numRows * numCols);

         int rounds = 0;
         int totalNumOccupied = 0;
         for (int numChanges = 0; rounds == 0 || numChanges != 0; rounds++)
         {
            Array.Copy(currState, nextState, numRows * numCols);

            totalNumOccupied = 0;
            numChanges = 0;

            for (int i = 0; i < numRows; i++)
            {
               for (int j = 0; j < numCols; j++)
               {
                  var numOccupied = GetNumOccupiedLOS(currState, i, j);
                  if (currState[i, j] == SeatState.Empty && numOccupied == 0)
                  {
                     nextState[i, j] = SeatState.Occupied;
                     numChanges++;
                  }
                  else if (currState[i, j] == SeatState.Occupied && numOccupied >= 5)
                  {
                     nextState[i, j] = SeatState.Empty;
                     numChanges++;
                  }

                  if (nextState[i, j] == SeatState.Occupied)
                  {
                     totalNumOccupied++;
                  }
               }
            }

            Array.Copy(nextState, currState, numRows * numCols);
         }

         Debug.WriteLine($"Q11Part2: stabilized after {rounds - 1} rounds, total occupied={totalNumOccupied}");
      }
   }
}
