using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q08
   {
      static readonly List<Tuple<string, int>> instructions = new List<Tuple<string, int>>();

      public static void Go()
      {
         MakeList();
         Part1();
         Part2();
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("08input.txt"))
         {
            var parts = line.Split(' ');
            if (parts.Length != 2)
            {
               throw new Exception("malformed input");
            }

            instructions.Add(new Tuple<string, int>(parts[0], Convert.ToInt32(parts[1])));
         }
      }

      static void ProcessInstruction(List<Tuple<string, int>> inst, ref int i, ref int accum)
      {
         switch (inst[i].Item1)
         {
            case "acc":
               accum += inst[i].Item2;
               i++;
               break;

            case "jmp":
               i += inst[i].Item2;
               break;

            case "nop":
               i++;
               break;
         }
      }

      static Tuple<int, bool> RunProgram(List<Tuple<string, int>> program)
      {
         var accum = 0;
         var executed = new Dictionary<int, int>();
         for (var i = 0; ;)
         {
            if (program.Count <= i)
            {
               return new Tuple<int, bool>(accum, true);
            }

            if (executed.ContainsKey(i))
            {
               break;
            }

            executed.Add(i, 1);

            ProcessInstruction(program, ref i, ref accum);
         }

         return new Tuple<int, bool>(accum, false);
      }

      static void Part1()
      {
         var result = RunProgram(instructions);
         if (result.Item2)
         {
            throw new Exception("Exited normally - shouldn't have");
         }

         Util.Log($"Q08Part1: accum value={result.Item1}");
      }

      static void Part2()
      {
         // should probably have used RunProgram here, but ended up replacing the first-encountered instruction instead of
         // moving through the program and changing the first unchanged instruction i came across. this seemed possibly
         // more efficient? i don't know.

         bool bExited = false;
         var instructionToChange = "jmp";
         var instructionToReplace = "nop";
         var replacedInstructions = new List<int>();
         var accum = 0;
         while (!bExited)
         {
            accum = 0;
            var copy = new List<Tuple<string, int>>(instructions);
            var executed = new Dictionary<int, int>();
            bool bChanged = false;
            for (var i = 0; ;)
            {
               if (copy.Count <= i)
               {
                  bExited = true;
                  break;
               }

               if (executed.ContainsKey(i))
               {
                  if (!bChanged)
                  {
                     // this whole section is hacky
                     if (instructionToChange != "jmp")
                     {
                        throw new Exception("nothing worked");
                     }

                     Util.Log($"changing {instructionToChange} to {instructionToReplace} didn't work - flipping");
                     var tmp = instructionToChange;
                     instructionToChange = instructionToReplace;
                     instructionToReplace = tmp;

                     replacedInstructions = new List<int>();
                  }

                  break;
               }

               executed.Add(i, 1);

               if (copy[i].Item1 == instructionToChange && !bChanged && !replacedInstructions.Contains(i))
               {
                  replacedInstructions.Add(i);
                  bChanged = true;
                  copy[i] = new Tuple<string, int>(instructionToReplace, copy[i].Item2);
               }

               ProcessInstruction(copy, ref i, ref accum);
            }
         }

         Util.Log($"Q08Part2: accum value={accum}");
      }
   }
}
