using Gentings.ConsoleApp;
using System.Threading.Tasks;

namespace GCApp
{
    class Program
    {
        static Task Main(string[] args)
        {
           return Consoles.StartAsync(args);
        }
    }
}
