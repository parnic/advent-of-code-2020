using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
   class Q07
   {
      class BagType
      {
         public string BagName;
         public List<Tuple<string, int>> ValidContents = new List<Tuple<string, int>>();
      }

      static Regex bagRegex = new Regex(@"(?: contain |, )?(?<numBags>[0-9]+ )?(?<bagType>.+?) bags?", RegexOptions.Compiled);

      static List<BagType> list = new List<BagType>();

      public static void Go()
      {
         MakeList();
         Part1();
         Part2();
      }

      static void MakeList()
      {
         var lines = File.ReadAllLines("07input.txt");
         foreach (var line in lines)
         {
            var matches = bagRegex.Matches(line);
            if (matches.Count <= 1)
            {
               throw new Exception("Unexpected input line");
            }

            var bag = new BagType()
            {
               BagName = matches[0].Groups["bagType"].Value,
            };

            for (int i = 1; i < matches.Count; i++)
            {
               if (matches[i].Groups["bagType"].Value == "no other")
               {
                  break;
               }

               bag.ValidContents.Add(new Tuple<string, int>(matches[i].Groups["bagType"].Value, Convert.ToInt32(matches[i].Groups["numBags"].Value)));
            }

            list.Add(bag);
         }
      }

      static List<BagType> GetBagsCanContain(BagType checkBag)
      {
         var retval = new List<BagType>();
         foreach (var bag in list)
         {
            if (bag.ValidContents.FirstOrDefault(x => x.Item1 == checkBag.BagName) != null)
            {
               retval.Add(bag);
            }
         }

         return retval;
      }

      static List<BagType> GetCanContainAny(List<BagType> others)
      {
         var retval = new List<BagType>();
         foreach (var other in others)
         {
            retval.AddRange(GetBagsCanContain(other));
         }

         return retval;
      }

      static void Part1()
      {
         Func<List<string>, BagType, List<string>> bagAgg = (accum, bagType) =>
         {
            accum.Add(bagType.BagName);
            return accum;
         };

         var directContain = GetBagsCanContain(new BagType() { BagName = "shiny gold" });
         var totalContain = directContain.Aggregate(new List<string>(), bagAgg);

         var recursiveContain = GetCanContainAny(directContain);
         while (recursiveContain.Count > 0)
         {
            totalContain.AddRange(recursiveContain.Aggregate(new List<string>(), bagAgg));
            recursiveContain = GetCanContainAny(recursiveContain);
         }

         var result = totalContain.Distinct();
         Util.Log($"Q07Part1: total contain={result.Count()}");
      }

      static int GetNumBagsContainedIn(BagType bag)
      {
         int bagsNeeded = 0;
         foreach (var contain in bag.ValidContents)
         {
            var containBagRule = list.First(x => x.BagName == contain.Item1);
            bagsNeeded += contain.Item2;
            bagsNeeded += contain.Item2 * GetNumBagsContainedIn(containBagRule);
         }

         return bagsNeeded;
      }

      static void Part2()
      {
         var shinyGoldBag = list.First(x => x.BagName == "shiny gold");

         int bagsNeeded = GetNumBagsContainedIn(shinyGoldBag);

         Util.Log($"Q07Part2: bags needed={bagsNeeded}");
      }
   }
}
