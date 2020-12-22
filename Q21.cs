using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q21
   {
      static readonly List<Tuple<List<string>, List<string>>> list = new List<Tuple<List<string>, List<string>>>();
      static readonly Dictionary<string, int> ingredientCounts = new Dictionary<string, int>();
      static IEnumerable<string> allAllergens = new List<string>();
      static IEnumerable<string> allIngredients = new List<string>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q21 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q21 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q21 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q21 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("21input.txt"))
         {
            var chunks = line.Split(" (contains ");
            var ingredients = new List<string>(chunks[0].Split(' '));
            var allergens = new List<string>(chunks[1][..^1].Split(", "));
            list.Add(new Tuple<List<string>, List<string>>(ingredients, allergens));
         }

         list.ForEach(entry =>
         {
            allIngredients = allIngredients.Union(entry.Item1);
            allAllergens = allAllergens.Union(entry.Item2);

            foreach (var item in entry.Item1)
            {
               if (!ingredientCounts.ContainsKey(item))
               {
                  ingredientCounts.Add(item, 0);
               }

               ingredientCounts[item]++;
            }
         });
      }

      static void Part1()
      {
         IEnumerable<string> possibleAllergenIngredients = new List<string>();
         foreach (var a in allAllergens)
         {
            var possibleIngredients = new List<List<string>>();
            foreach (var item in list)
            {
               if (item.Item2.Contains(a))
               {
                  possibleIngredients.Add(item.Item1);
               }
            }

            var intersection = possibleIngredients.Aggregate(possibleIngredients[0], (commonIngredients, item) => commonIngredients = new List<string>(commonIngredients.Intersect(item)));
            possibleAllergenIngredients = possibleAllergenIngredients.Union(intersection);
         }

         var nonAllergenWords = allIngredients.Except(possibleAllergenIngredients);
         var count = 0;
         foreach (var word in nonAllergenWords)
         {
            count += ingredientCounts[word];
         }

         Util.Log($"Q21Part1: count={count}");
      }

      static void Part2()
      {
         IEnumerable<string> possibleAllergenIngredients = new List<string>();
         var allergenIngredientMap = new Dictionary<string, IEnumerable<string>>();
         foreach (var a in allAllergens)
         {
            var possibleIngredients = new List<List<string>>();
            foreach (var item in list)
            {
               if (item.Item2.Contains(a))
               {
                  possibleIngredients.Add(item.Item1);
               }
            }

            var intersection = possibleIngredients.Aggregate(possibleIngredients[0], (commonIngredients, item) => commonIngredients = new List<string>(commonIngredients.Intersect(item)));
            allergenIngredientMap.Add(a, intersection);
            possibleAllergenIngredients = possibleAllergenIngredients.Union(intersection);
         }

         bool foundAll = false;
         while (!foundAll)
         {
            foundAll = true;
            foreach (var i in allergenIngredientMap)
            {
               if (i.Value.Count() > 1)
               {
                  var possibleIngredients = i.Value;
                  var filtered = allergenIngredientMap.Where(x => x.Key != i.Key).Aggregate(possibleIngredients, (commonIngredients, item) =>
                  {
                     if (item.Value.Count() == 1)
                     {
                        possibleIngredients = possibleIngredients.Except(item.Value);
                     }

                     return possibleIngredients;
                  });

                  if (filtered.Count() == 1)
                  {
                     allergenIngredientMap[i.Key] = filtered;
                  }
                  else
                  {
                     foundAll = false;
                  }
               }
            }
         }

         var orderedAllergenMap = allergenIngredientMap.OrderBy(x => x.Key);
         var items = orderedAllergenMap.Aggregate(new List<string>(), (lst, itm) => { lst.Add(itm.Value.ElementAt(0)); return lst; });
         var joined = string.Join(',', items);

         Util.Log($"Q21Part2: avoid={joined}");
      }
   }
}
