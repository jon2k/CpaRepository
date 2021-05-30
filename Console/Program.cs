using System;
using System.IO;

namespace Console
{
    public class Test
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"d:\Project VS\CpaRepository\CpaRepository\wwwroot\Root\Letters\Emicon\28.05.2021\123\уу\уу\ц\ечею.exe";

            string pathFolder= Path.GetDirectoryName(path);
            var directoryInfo = new DirectoryInfo(pathFolder);        
            Delete(directoryInfo);         
           
            void Delete(DirectoryInfo di)
            {
                var ee = di.GetFiles();
                var rr = di.GetDirectories();
                if (di.GetFiles().Length == 0 && di.GetDirectories().Length==0)
                {                  
                    di.Delete();
                    Delete(di.Parent);
                }
                
            }
        }
    }
}
