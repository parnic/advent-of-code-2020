using System;
using System.IO;

namespace _2020
{
   class Q25
   {
      static int CardPubKey = 0;
      static int DoorPubKey = 0;
      const int ModBy = 20201227;

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q25 MakeList took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q25 part1 took {(DateTime.Now - p1start).TotalMilliseconds:N0}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q25 part2 took {(DateTime.Now - p2start).TotalMilliseconds:N0}ms");

         Util.Log($"Q25 took {(DateTime.Now - start).TotalMilliseconds:N0}ms");
      }

      static void MakeList()
      {
         var lines = File.ReadAllLines("25input.txt");
         if (lines.Length != 2)
         {
            throw new Exception("invalid input");
         }

         CardPubKey = Convert.ToInt32(lines[0]);
         DoorPubKey = Convert.ToInt32(lines[1]);
      }

      static int GetLoopsToTransform(int subjectNum, int targetVal)
      {
         var derivedNum = 1;
         int loopSize;
         for (loopSize = 0; derivedNum != targetVal; loopSize++)
         {
            derivedNum *= subjectNum;
            derivedNum = derivedNum % ModBy;
         }

         return loopSize;
      }

      static long GetTransformed(int subjectNum, int loopSize)
      {
         var result = 1L;
         for (int i = 0; i < loopSize; i++)
         {
            result *= subjectNum;
            result = result % ModBy;
         }

         return result;
      }

      static void Part1()
      {
         var cardLoopSize = GetLoopsToTransform(7, CardPubKey);
         var doorLoopSize = GetLoopsToTransform(7, DoorPubKey);

         var cardEncryptionKey = GetTransformed(DoorPubKey, cardLoopSize);
         var doorEncryptionKey = GetTransformed(CardPubKey, doorLoopSize);

         if (cardEncryptionKey != doorEncryptionKey)
         {
            throw new Exception("you did it wrong");
         }

         Util.Log($"Q25Part1: key={cardEncryptionKey}");
      }

      static void Part2()
      {
         Util.Log($"Q25Part2: there is no part 2");
      }
   }
}
