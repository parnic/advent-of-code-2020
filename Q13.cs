using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _2020
{
   class Q13
   {
      static int leaveTime;
      static readonly List<int> busses = new List<int>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q13 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q13 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q13 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q13 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         var lines = File.ReadAllLines("13input.txt");
         if (lines.Length != 2)
         {
            throw new Exception("Invalid input");
         }

         leaveTime = Convert.ToInt32(lines[0]);
         foreach (var bus in lines[1].Split(','))
         {
            if (bus == "x")
            {
               busses.Add(-1);
               continue;
            }

            busses.Add(Convert.ToInt32(bus));
         }
      }

      static void Part1()
      {
         int smallestModulo = int.MaxValue;
         int smallestBus = -1;
         foreach (var bus in busses)
         {
            if (bus < 0)
            {
               continue;
            }

            var mod = bus - (leaveTime % bus);
            if (mod < smallestModulo)
            {
               smallestModulo = mod;
               smallestBus = bus;
            }
         }

         Util.Log($"Q13Part1: soonest bus={smallestBus}, departs={leaveTime + smallestModulo}, wait time={smallestModulo}m, answer={smallestBus * smallestModulo}");
      }

      // taken from https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23, modified for int64s. i don't know how this works. who am i, sun-tzu?
      static long GetChineseRemainderTheorem(IEnumerable<int> n, IEnumerable<int> a)
      {
         long prod = n.Aggregate(1L, (i, j) => i * j);
         long p;
         long sm = 0;
         for (int i = 0; i < n.Count(); i++)
         {
            p = prod / n.ElementAt(i);
            sm += a.ElementAt(i) * ModularMultiplicativeInverse(p, n.ElementAt(i)) * p;
         }
         return sm % prod;
      }

      static long ModularMultiplicativeInverse(long a, long mod)
      {
         long b = a % mod;
         for (long x = 1; x < mod; x++)
         {
            if ((b * x) % mod == 1)
            {
               return x;
            }
         }
         return 1;
      }

      static void Part2()
      {
         var a = new List<int>();
         var n = new List<int>();
         for (int i = 0; i < busses.Count; i++)
         {
            if (busses[i] <= 0)
            {
               continue;
            }

            a.Add(busses[i] - i % busses[i]);
            n.Add(busses[i]);
         }

         Util.Log($"Q13Part2: val={GetChineseRemainderTheorem(n, a)}");
      }

      // this is how i initially solved it. took ~30mins. i'm not proud of it, but the "real" answer apparently required specialized knowledge i did not have.
#pragma warning disable IDE0051 // Remove unused private members
      static void Part2BruteForce()
#pragma warning restore IDE0051 // Remove unused private members
      {
         int root = busses.First(x => x > 0);
         var sorted = busses.OrderBy(x => x);
         var largest = sorted.Last();

         long checkVal = 0;

         var idx = busses.IndexOf(largest);
         int answersFound = 0;
         int numThreads = 13;
         Parallel.For(0, numThreads, thread =>
         {
            var threadVal = checkVal + (thread * largest);
            long rootCheckVal = 0;
            while (answersFound == 0)
            {
               threadVal += numThreads * largest;
               rootCheckVal = threadVal - idx;
               if (rootCheckVal % root == 0)
               {
                  var gotit = false;
                  for (int i = 1; i < busses.Count; i++)
                  {
                     if (busses[i] <= 0)
                     {
                        continue;
                     }

                     if ((rootCheckVal + i) % busses[i] == 0)
                     {
                        gotit = true;
                     }
                     else
                     {
                        gotit = false;
                        break;
                     }
                  }

                  if (gotit)
                  {
                     Interlocked.Increment(ref answersFound);
                     break;
                  }
               }
            }

            Util.Log($"Q13Part2: val={rootCheckVal}");
         });
      }
   }
}
