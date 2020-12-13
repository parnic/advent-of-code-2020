using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _2020
{
   class Q12
   {
      [DebuggerDisplay("({x}, {y})")]
      struct Vec2
      {
         public int x;
         public int y;
      }

      static Vec2 shipPosition;
      static Vec2 waypointPosition = new Vec2() { x = 10, y = 1 };
      static int facingDir = 90;

      static List<Tuple<char, int>> instructions = new List<Tuple<char, int>>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q12 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q12 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q12 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q12 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("12input.txt"))
         {
            instructions.Add(new Tuple<char, int>(line[0], Convert.ToInt32(line[1..]) % 360));
         }
      }

      static void MoveDir(ref Vec2 vec, char dir, int amount)
      {
         switch (dir)
         {
            case 'N':
               vec.y += amount;
               break;

            case 'S':
               vec.y -= amount;
               break;

            case 'E':
               vec.x += amount;
               break;

            case 'W':
               vec.x -= amount;
               break;

            default:
               throw new Exception("Invalid MoveDir direction");
         }
      }

      static void MoveShipDir(char dir, int amount)
      {
         MoveDir(ref shipPosition, dir, amount);
      }

      static void MoveShipToWaypoint(int amount)
      {
         shipPosition.x += waypointPosition.x * amount;
         shipPosition.y += waypointPosition.y * amount;
      }

      static void MoveWaypointDir(char dir, int amount)
      {
         MoveDir(ref waypointPosition, dir, amount);
      }

      static char GetFacingDir()
      {
         if (facingDir == 0)
         {
            return 'N';
         }
         else if (facingDir == 90)
         {
            return 'E';
         }
         else if (facingDir == 180)
         {
            return 'S';
         }
         else if (facingDir == 270)
         {
            return 'W';
         }

         throw new Exception("Invalid facing dir");
      }

      static void RotateShipDir(char dir, int amount)
      {
         if (amount % 90 != 0)
         {
            throw new Exception("Turned toward a non-cardinal direction");
         }

         if (dir == 'R')
         {
            facingDir = (facingDir + amount) % 360;
         }
         else if (dir == 'L')
         {
            facingDir = (facingDir - amount);
            while (facingDir < 0)
            {
               facingDir = 360 + facingDir;
            }
         }
         else
         {
            throw new Exception("Invalid rotation dir");
         }
      }

      static void RotateWaypointDir(char dir, int amount)
      {
         if (amount % 90 != 0)
         {
            throw new Exception("Turned toward a non-cardinal direction");
         }

         switch (amount % 360)
         {
            case 180:
               waypointPosition.y = -waypointPosition.y;
               waypointPosition.x = -waypointPosition.x;
               break;

            case 90:
               if (dir == 'R')
               {
                  var tmp = waypointPosition.x;
                  waypointPosition.x = waypointPosition.y;
                  waypointPosition.y = -tmp;
               }
               else if (dir == 'L')
               {
                  var tmp = waypointPosition.y;
                  waypointPosition.y = waypointPosition.x;
                  waypointPosition.x = -tmp;
               }
               break;

            case 270:
               RotateWaypointDir(dir == 'R' ? 'L' : 'R', 90);
               break;

            default:
               throw new Exception("Unexpected waypoint rotation direction");
         }
      }

      static void Part1()
      {
         foreach (var inst in instructions)
         {
            switch (inst.Item1)
            {
               case 'N':
               case 'S':
               case 'E':
               case 'W':
                  MoveShipDir(inst.Item1, inst.Item2);
                  break;

               case 'F':
                  MoveShipDir(GetFacingDir(), inst.Item2);
                  break;

               case 'R':
               case 'L':
                  RotateShipDir(inst.Item1, inst.Item2);
                  break;
            }
         }

         Util.Log($"Q12Part1: finished facing {facingDir}, x={shipPosition.x}, y={shipPosition.y}, manhattan dist={Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y)}");
      }

      static void Part2()
      {
         shipPosition = new Vec2();
         foreach (var inst in instructions)
         {
            switch (inst.Item1)
            {
               case 'N':
               case 'S':
               case 'E':
               case 'W':
                  MoveWaypointDir(inst.Item1, inst.Item2);
                  break;

               case 'F':
                  MoveShipToWaypoint(inst.Item2);
                  break;

               case 'R':
               case 'L':
                  RotateWaypointDir(inst.Item1, inst.Item2);
                  break;
            }
         }

         Util.Log($"Q12Part2: finished facing {facingDir}, x={shipPosition.x}, y={shipPosition.y}, manhattan dist={Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y)}");
      }
   }
}
