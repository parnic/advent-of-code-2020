using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q06
   {
      class Group
      {
         public int numInGroup;
         public Dictionary<char, int> answers;
      }
      static readonly List<Group> list = new List<Group>();

      public static void Go()
      {
         MakeList();
         Part1();
         Part2();
      }

      static void MakeList()
      {
         using var fs = new FileStream("06input.txt", FileMode.Open);
         using var sr = new StreamReader(fs);
         string line;
         Group entry = null;
         while ((line = sr.ReadLine()) != null)
         {
            if (line.Length == 0 || entry == null)
            {
               entry = new Group()
               {
                  numInGroup = 0,
                  answers = new Dictionary<char, int>(),
               };
               list.Add(entry);
            }

            if (line.Length == 0)
            {
               continue;
            }

            entry.numInGroup++;
            foreach (var ch in line)
            {
               if (!entry.answers.ContainsKey(ch))
               {
                  entry.answers.Add(ch, 1);
               }
               else
               {
                  entry.answers[ch]++;
               }
            }
         }
      }

      static void Part1()
      {
         var sum = list.Sum(x => x.answers.Keys.Count);
         Util.Log($"Q06Part1: sum={sum}");
      }

      static void Part2()
      {
         var sum = list.Sum(x => x.answers.Sum(y => y.Value == x.numInGroup ? 1 : 0));
         Util.Log($"Q06Part2: sum={sum}");
      }
   }
}
