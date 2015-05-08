using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            //SingleThreadServer server = new SingleThreadServer();
            //ThreadServer server = new ThreadServer();
            TaskServer server = new TaskServer();
            server.Start();
        }
    }
}
