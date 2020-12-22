using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _2020
{
   class Q04
   {
      static readonly List<Dictionary<string, string>> Passports = new List<Dictionary<string, string>>();

      static readonly Regex PassportRegex = new Regex(@"(?<key>[a-z]{3}):(?<val>\S+)", RegexOptions.Compiled);
      static readonly Regex HTMLColor = new Regex("^#[0-9a-f]{6}$", RegexOptions.Compiled);
      static readonly Regex PassportID = new Regex("^[0-9]{9}$", RegexOptions.Compiled);

      public static void Go()
      {
         MakeList();
         Part1();
         Part2();
      }

      static void MakeList()
      {
         using var fs = new FileStream("04input.txt", FileMode.Open);
         using var sr = new StreamReader(fs);
         string line;
         while ((line = sr.ReadLine()) != null)
         {
            if (line.Length == 0 || Passports.Count == 0)
            {
               Passports.Add(new Dictionary<string, string>());
            }

            var matches = PassportRegex.Matches(line);
            for (int i = 0; i < matches.Count; i++)
            {
               Passports[^1].Add(matches[i].Groups["key"].Value, matches[i].Groups["val"].Value);
            }
         }
      }

      static bool IsValid_Basic(Dictionary<string, string> passport)
      {
         if (passport.Count >= 8)
         {
            return true;
         }
         else if (passport.Count == 7 && !passport.ContainsKey("cid"))
         {
            return true;
         }

         return false;
      }

      static void Part1()
      {
         int numValid = 0;
         foreach (var passport in Passports)
         {
            if (IsValid_Basic(passport))
            {
               numValid++;
            }
         }

         Util.Log($"Q04Part1: num valid={numValid}");
      }

      static void Part2()
      {
         int numValid = 0;
         foreach (var passport in Passports)
         {
            bool isValid = IsValid_Basic(passport);
            foreach (var set in passport)
            {
               switch (set.Key)
               {
                  case "byr":
                     {
                        var year = Convert.ToInt32(set.Value);
                        isValid = isValid && year >= 1920 && year <= 2002;
                     }
                     break;

                  case "iyr":
                     {
                        var year = Convert.ToInt32(set.Value);
                        isValid = isValid && year >= 2010 && year <= 2020;
                     }
                     break;

                  case "eyr":
                     {
                        var year = Convert.ToInt32(set.Value);
                        isValid = isValid && year >= 2020 && year <= 2030;
                     }
                     break;

                  case "hgt":
                     {
                        var val = 0;
                        var unit = "";
                        if (set.Value.Length > 2)
                        {
                           val = Convert.ToInt32(set.Value[..^2]);
                           unit = set.Value[^2..];
                        }
                        isValid = isValid && ((unit == "cm" && val >= 150 && val <= 193) || (unit == "in" && val >= 59 && val <= 76));
                     }
                     break;

                  case "hcl":
                     {
                        isValid = isValid && HTMLColor.IsMatch(set.Value);
                     }
                     break;

                  case "ecl":
                     {
                        isValid = isValid && (set.Value == "amb" || set.Value == "blu" || set.Value == "brn" || set.Value == "gry" || set.Value == "grn" || set.Value == "hzl" || set.Value == "oth");
                     }
                     break;

                  case "pid":
                     {
                        isValid = isValid && PassportID.IsMatch(set.Value);
                     }
                     break;
               }
            }

            if (isValid)
            {
               numValid++;
            }
         }

         Util.Log($"Q04Part2: num valid={numValid}");
      }
   }
}
