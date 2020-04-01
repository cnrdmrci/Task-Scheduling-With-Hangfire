using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace TaskSchedulingWithHangfire
{
    public class Database
    {
        [AutomaticRetry(Attempts = 3)]
        public void BackUp()
        {
            Console.WriteLine("Database BackUp Run");
        }
    }
}
