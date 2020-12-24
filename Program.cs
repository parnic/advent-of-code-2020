namespace _2020
{
   class Program
   {
      static void Main()
      {
         var start = System.DateTime.Now;
         Q01.Go();
         Q02.Go();
         Q03.Go();
         Q04.Go();
         Q05.Go();
         Q06.Go();
         Q07.Go();
         Q08.Go();
         Q09.Go();
         Q10.Go();
         Q11.Go();
         Q12.Go();
         Q13.Go();
         Q14.Go();
         Q15.Go();
         Q18.Go();
         Q19.Go();
         Q20.Go();
         Q21.Go();
         Q22.Go();
         Q23.Go();
         Q24.Go();
         Util.Log($"Total time={(System.DateTime.Now - start).TotalMilliseconds}ms");
      }
   }

   class Util
   {
      internal static void Log(string msg)
      {
         System.Diagnostics.Debug.WriteLine(msg);
         System.Console.WriteLine(msg);
      }
   }
}
