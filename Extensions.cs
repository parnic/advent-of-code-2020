using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2020
{
   public static class Extensions
   {
      public static T PopFront<T>(this ICollection<T> list)
      {
         T val = list.First();
         list.Remove(val);
         return val;
      }
   }
}
