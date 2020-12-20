using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _2020
{
   class Q19
   {
      static Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
      static List<string> messages = new List<string>();

      [DebuggerDisplay("ch={ch} | num rules={ruleIdxs?.Count}")]
      class Rule
      {
         public char? ch;
         public List<List<int>> ruleIdxs = new List<List<int>>();
      }

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q19 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q19 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q19 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q19 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         int mode = 0;
         foreach (var line in File.ReadAllLines("19input.txt"))
         {
            if (mode == 0)
            {
               if (string.IsNullOrWhiteSpace(line))
               {
                  mode++;
                  continue;
               }

               var chunks = line.Split(":");
               var ruleIdx = Convert.ToInt32(chunks[0]);

               if (chunks[1].StartsWith(" \""))
               {
                  rules.Add(ruleIdx, new Rule() { ch = chunks[1][2] });
               }
               else
               {
                  var refs = chunks[1].Split("|");
                  var ruleIdxs = new List<List<int>>();
                  foreach (var ruleRef in refs)
                  {
                     var refGroup = new List<int>();

                     var refIdxs = ruleRef.Split(" ");
                     foreach (var refIdx in refIdxs)
                     {
                        if (!string.IsNullOrWhiteSpace(refIdx))
                        {
                           refGroup.Add(Convert.ToInt32(refIdx));
                        }
                     }

                     if (refGroup.Count > 0)
                     {
                        ruleIdxs.Add(refGroup);
                     }
                  }

                  rules.Add(ruleIdx, new Rule() { ruleIdxs = ruleIdxs });
               }
            }
            else
            {
               messages.Add(line);
            }
         }
      }

      static Tuple<bool, int> PassesRule(Dictionary<int, Rule> ruleList, int ruleNum, string str, int startIdx = 0)
      {
         if (ruleList[ruleNum].ch.HasValue)
         {
            return new Tuple<bool, int>(str.Length > startIdx && ruleList[ruleNum].ch == str[startIdx], 1);
         }
         else
         {
            var matchedGroups = false;
            var matchLen = 0;
            foreach (var grp in ruleList[ruleNum].ruleIdxs)
            {
               var matchedGroup = true;
               matchLen = 0;
               for (int i = 0; i < grp.Count; i++)
               {
                  var matchedRule = PassesRule(ruleList, grp[i], str, startIdx + matchLen);
                  if (!matchedRule.Item1)
                  {
                     matchedGroup = false;
                     break;
                  }
                  else
                  {
                     matchLen += matchedRule.Item2;
                  }
               }

               if (matchedGroup)
               {
                  matchedGroups = true;
                  break;
               }
            }

            return new Tuple<bool, int>(matchedGroups, matchLen);
         }
      }

      static void Part1()
      {
         int numMatched = 0;
         foreach (var msg in messages)
         {
            var result = PassesRule(rules, 0, msg);
            if (result.Item1 && result.Item2 == msg.Length)
            {
               numMatched++;
            }
         }

         Util.Log($"Q19Part1: total={messages.Count}, numMatched={numMatched}");
      }

      static void Part2()
      {
         var rulesUpdated = new Dictionary<int, Rule>(rules);
         rulesUpdated[8] = new Rule()
         {
            ruleIdxs = new List<List<int>>()
            {
               new List<int>()
               {
                  42,
               },
               new List<int>()
               {
                  42,
                  8,
               },
            },
         };
         rulesUpdated[11] = new Rule()
         {
            ruleIdxs = new List<List<int>>()
            {
               new List<int>()
               {
                  42,
                  31,
               },
               new List<int>()
               {
                  42,
                  11,
                  31,
               },
            },
         };

         int numMatched = 0;
         foreach (var msg in messages)
         {
            var firstRuleMatches = new List<Tuple<bool, int>>();
            Tuple<bool, int> match = null;
            var startIdx = 0;
            while ((match = PassesRule(rulesUpdated, rulesUpdated[0].ruleIdxs[0][0], msg, startIdx)).Item1)
            {
               firstRuleMatches.Add(new Tuple<bool, int>(match.Item1, match.Item2 + startIdx));
               startIdx += match.Item2;
            }
            var matchCombos = 0;
            foreach (var firstRuleMatch in firstRuleMatches)
            {
               startIdx = 0;
               while ((match = PassesRule(rulesUpdated, rulesUpdated[0].ruleIdxs[0][1], msg, firstRuleMatch.Item2 + startIdx)).Item1)
               {
                  if (firstRuleMatch.Item2 + match.Item2 == msg.Length)
                  {
                     matchCombos++;
                     break;
                  }
                  startIdx += match.Item2;
               }

               if (matchCombos > 0)
               {
                  break;
               }
            }

            if (matchCombos > 0)
            {
               numMatched++;
            }
         }

         Util.Log($"Q19Part2: total={messages.Count}, numMatched={numMatched}");
      }
   }
}
