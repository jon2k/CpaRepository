using System;
using System.IO;

namespace Console
{
    public class Test
    {
        public string Go(int x)
        {

            try
            {
                var res=5 / x;
                return res.ToString();
            }
            catch (Exception)
            {

                return "Catch";
            }
            finally
            {
                System.Console.WriteLine("Finaly");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            var test = new Test();
            System.Console.WriteLine(test.Go(0));
        }
    }
}
