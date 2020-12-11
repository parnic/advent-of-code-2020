namespace _2020
{
   class Program
   {
      static void Main(string[] args)
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
         System.Diagnostics.Debug.WriteLine($"Total time={(System.DateTime.Now - start).TotalMilliseconds}ms");
      }
   }
}
