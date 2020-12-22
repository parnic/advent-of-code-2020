using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _2020
{
   class Q14
   {
      [DebuggerDisplay("mask = [...], {writeInstructions.Count} writes")]
      class Instruction
      {
         [DebuggerDisplay("mem[{idx}] = {value}")]
         public class WriteInstruction
         {
            public ulong idx;
            public ulong value;
         }

         public List<byte> mask = new List<byte>();
         public List<WriteInstruction> writeInstructions = new List<WriteInstruction>();
      }
      static readonly List<Instruction> instructions = new List<Instruction>();

      public static void Go()
      {
         var start = DateTime.Now;
         MakeList();
         Util.Log($"Q14 MakeList took {(DateTime.Now - start).TotalMilliseconds}ms");
         var p1start = DateTime.Now;
         Part1();
         Util.Log($"Q14 part1 took {(DateTime.Now - p1start).TotalMilliseconds}ms");
         var p2start = DateTime.Now;
         Part2();
         Util.Log($"Q14 part2 took {(DateTime.Now - p2start).TotalMilliseconds}ms");

         Util.Log($"Q14 took {(DateTime.Now - start).TotalMilliseconds}ms");
      }

      static void MakeList()
      {
         foreach (var line in File.ReadAllLines("14input.txt"))
         {
            if (line.StartsWith("mask = "))
            {
               var inst = new Instruction();
               var bits = line["mask = ".Length..];
               foreach (var num in bits.Reverse())
               {
                  if (num == 'X')
                  {
                     inst.mask.Add(byte.MaxValue);
                  }
                  else
                  {
                     inst.mask.Add(num == '0' ? 0 : 1);
                  }
               }

               instructions.Add(inst);
            }
            else if (line.StartsWith("mem["))
            {
               var inst = instructions[^1];

               var writeInst = new Instruction.WriteInstruction
               {
                  idx = Convert.ToUInt64(line["mem[".Length..line.IndexOf("]")]),
                  value = Convert.ToUInt64(line[(line.IndexOf('=') + 1)..])
               };

               inst.writeInstructions.Add(writeInst);
            }
         }
      }

      static ulong ReplaceBit(ulong val, byte bitVal, int bit)
      {
         if (bitVal == 0)
         {
            val &= (ulong)~(1L << bit);
         }
         else if (bitVal == 1)
         {
            val |= 1UL << bit;
         }

         return val;
      }

      static void Part1()
      {
         var memory = new Dictionary<ulong, ulong>();

         foreach (var inst in instructions)
         {
            foreach (var write in inst.writeInstructions)
            {
               if (!memory.ContainsKey(write.idx))
               {
                  memory.Add(write.idx, 0);
               }

               var val = write.value;

               for (int i = 0; i < inst.mask.Count; i++)
               {
                  val = ReplaceBit(val, inst.mask[i], i);
               }

               memory[write.idx] = val;
            }
         }

         long total = memory.Sum(pair => (long)pair.Value);

         Util.Log($"Q14Part1: sum={total}");
      }

      static void ComputeAddresses(ulong baseAddr, ref List<ulong> addrs, List<byte> mask, int startIdx)
      {
         for (int i = startIdx; i < mask.Count; i++)
         {
            if (mask[i] == byte.MaxValue)
            {
               // these Add()s can add duplicates, but it's faster to re-set an existing key in a dictionary than to ensure uniqueness in this list
               var replacedAddr = ReplaceBit(baseAddr, 1, i);
               addrs.Add(replacedAddr);
               ComputeAddresses(replacedAddr, ref addrs, mask, i + 1);
               addrs.Add(ReplaceBit(baseAddr, 0, i));
            }
         }
      }

      static void Part2()
      {
         var memory = new Dictionary<ulong, ulong>();

         foreach (var inst in instructions)
         {
            foreach (var write in inst.writeInstructions)
            {
               var addrs = new List<ulong>();

               var baseAddr = write.idx;
               for (int i = 0; i < inst.mask.Count; i++)
               {
                  if (inst.mask[i] == 1)
                  {
                     baseAddr = ReplaceBit(baseAddr, 1, i);
                  }
                  else if (inst.mask[i] == byte.MaxValue)
                  {
                     baseAddr = ReplaceBit(baseAddr, 0, i);
                  }
               }

               addrs.Add(baseAddr);
               ComputeAddresses(baseAddr, ref addrs, inst.mask, 0);

               foreach (var addr in addrs)
               {
                  if (!memory.ContainsKey(addr))
                  {
                     memory.Add(addr, 0);
                  }

                  memory[addr] = write.value;
               }
            }
         }

         long total = memory.Sum(pair => (long)pair.Value);
         Util.Log($"Q14Part2: sum={total}");
      }
   }
}
