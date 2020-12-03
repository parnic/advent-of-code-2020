using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace _2020
{
   class Q02
   {
      public static void Go()
      {
         var list = GetList();
         Part1(list);
         Part2(list);
      }

      struct Q2Entry
      {
         public int first;
         public int second;
         public char ch;
         public string pw;
      }

      static List<Q2Entry> GetList()
      {
         var list = new List<Q2Entry>();

         using var fs = new FileStream("02input.txt", FileMode.Open);
         using var sr = new StreamReader(fs);
         string line;
         var rule02 = new Regex("(?<min>[0-9]+)-(?<max>[0-9]+) (?<ch>[a-z]): (?<pw>.+)", RegexOptions.Compiled);
         while ((line = sr.ReadLine()) != null)
         {
            var match = rule02.Match(line);
            if (match.Success)
            {
               list.Add(new Q2Entry()
               {
                  first = Convert.ToInt32(match.Groups["min"].Value),
                  second = Convert.ToInt32(match.Groups["max"].Value),
                  ch = match.Groups["ch"].Value[0],
                  pw = match.Groups["pw"].Value,
               });
            }
         }

         return list;
      }

      static void Part1(List<Q2Entry> list)
      {
         int numValid = 0;

         foreach (var entry in list)
         {
            var numFound = 0;
            foreach (var ch in entry.pw)
            {
               if (ch == entry.ch)
               {
                  numFound++;
               }
            }

            if (numFound >= entry.first && numFound <= entry.second)
            {
               numValid++;
            }
         }

         Debug.WriteLine($"Q02Part1: numValid={numValid}");
      }

      static void Part2(List<Q2Entry> list)
      {
         int numValid = 0;

         foreach (var entry in list)
         {
            var validConditions = 0;
            if (entry.pw.Length >= entry.first && entry.pw[entry.first - 1] == entry.ch)
            {
               validConditions++;
            }
            if (entry.pw.Length >= entry.second && entry.pw[entry.second - 1] == entry.ch)
            {
               validConditions++;
            }

            if (validConditions == 1)
            {
               numValid++;
            }
         }

         Debug.WriteLine($"Q02Part2: numValid={numValid}");
      }
   }
}
