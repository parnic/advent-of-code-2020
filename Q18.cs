using System;
using System.Collections.Generic;
using System.IO;

namespace _2020
{
   class Q18
   {
      static List<string> list = new List<string>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q18 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q18 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q18 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q18 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         list = new List<string>(File.ReadAllLines("18input.txt"));
      }

      static long DoOp(long left, long right, char op)
      {
         return op switch
         {
            '+' => left + right,
            '*' => left * right,
            _ => throw new Exception("unrecognized op"),
         };
      }

      static void Part1()
      {
         long sum = 0;
         for (int i = 0; i < list.Count; i++)
         {
            var evals = new Stack<long>();
            var ops = new Stack<char>();
            long currVal = 0;
            char currOp = '+';

            foreach (var ch in list[i])
            {
               if (ch == ' ')
               {
                  continue;
               }
               else if (ch == '+' || ch == '*')
               {
                  currOp = ch;
               }
               else if (ch == '(')
               {
                  ops.Push(currOp);
                  evals.Push(currVal);
                  currVal = 0;
                  currOp = '+';
               }
               else if (ch == ')')
               {
                  var lastOp = ops.Pop();
                  var lastVal = evals.Pop();
                  currVal = DoOp(currVal, lastVal, lastOp);
               }
               else
               {
                  var val = ch - '0';
                  currVal = DoOp(currVal, val, currOp);
               }
            }

            sum += currVal;
         }

         Util.Log($"Q18Part1: sum={sum}");
      }

      static long SolveAddPrecedence(string exp)
      {
         var evals = new Stack<long>();
         var ops = new Stack<char>();
         long currVal = 0;
         char currOp = '+';
         var numStr = "";

         foreach (var ch in exp)
         {
            if (ch == ' ')
            {
               if (!string.IsNullOrEmpty(numStr))
               {
                  currVal = DoOp(currVal, Convert.ToInt32(numStr), currOp);
                  numStr = "";
               }
               continue;
            }
            else if (ch == '+' || ch == '*')
            {
               currOp = ch;
               if (ch == '*')
               {
                  ops.Push(currOp);
                  evals.Push(currVal);
                  currVal = 0;
                  currOp = '+';
               }
            }
            else
            {
               numStr += ch;
            }
         }

         if (!string.IsNullOrEmpty(numStr))
         {
            currVal = DoOp(currVal, Convert.ToInt32(numStr), currOp);
         }

         return currVal;
      }

      static void Part2()
      {
         long sum = 0;
         for (int i = 0; i < list.Count; i++)
         {
            long currVal = 0;

            var expressions = new Stack<string>();
            string currExp = "";

            foreach (var ch in list[i])
            {
               if (ch == '(')
               {
                  expressions.Push(currExp);
                  currExp = "";
               }
               else if (ch == ')')
               {
                  var subVal = SolveAddPrecedence(currExp);
                  currExp = expressions.Pop();
                  currExp += subVal;
               }
               else
               {
                  currExp += ch;
               }
            }

            if (!string.IsNullOrEmpty(currExp))
            {
               currVal = SolveAddPrecedence(currExp);
            }

            sum += currVal;
         }

         Util.Log($"Q18Part2: sum={sum}");
      }
   }
}
