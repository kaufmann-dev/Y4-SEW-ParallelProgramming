using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgrammingRevision
{
    class Program
    {
        static void Main(string[] args)
        {
            // RACE CONDITION
            Console.WriteLine("RACE CONDITION");
            var sum = 0;
            
            Thread t1 = new Thread(() => {
                for (int i = 0; i < 10000000; i++)
                {
                    sum++;
                }
            });

            Thread t2 = new Thread(() => {
                for (int i = 0; i < 10000000; i++)
                {
                    sum++;
                }
            });
            
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine($"should be sum: 20000000\nactual sum:    {sum}");
            
            // DEADLOCK
            Console.WriteLine("\nDEADLOCK");
            var money = new SemaphoreSlim(0);
            var product = new SemaphoreSlim(0);
            Thread p1 = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("Waitung on p2 to release money so we can release product");
                    money.Wait();
                    product.Release();
                }
            });
            Thread p2 = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("Waitung on p1 to release product so we can release money");
                    product.Wait();
                    money.Release();
                }
            });
            p1.Start();
            p2.Start();
        }
    }
}