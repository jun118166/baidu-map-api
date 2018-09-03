using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }

        private static void Test2()
        {
            var waits = new List<EventWaitHandle>();
            for (int i = 0; i < 10; i++)
            {
                var handler = new ManualResetEvent(false);
                waits.Add(handler);
                new Thread(new ParameterizedThreadStart(Print))
                {
                    Name = "thread" + i.ToString()
                }.Start(new Tuple<int, EventWaitHandle>(i, handler));
            }
            WaitHandle.WaitAll(waits.ToArray());
            Console.WriteLine("All Completed!");
            Console.Read();
        }
        private static void Print(object param)
        {
            var p = (Tuple<int, EventWaitHandle>)param;
            Console.WriteLine(Thread.CurrentThread.Name + ": Begin!");

            if (p.Item1 == 2) Thread.Sleep(1200);
            else if (p.Item1 == 1) Thread.Sleep(2000);
            else Thread.Sleep(1000);
            Console.WriteLine(Thread.CurrentThread.Name + ": Print" + p.Item1);
            Console.WriteLine(Thread.CurrentThread.Name + ": End!");
            p.Item2.Set();
        }
    }
}
