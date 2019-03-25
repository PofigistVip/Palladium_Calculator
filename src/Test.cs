using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    static class Test
    {
        const int itCount = 1000000;
        static string[] speedExpressions =
        {
            "5",
            "PI",
            "abs(-5)",
            "5^5%5*5/5+5-5",
            "((((((0))))))",
            "-(-(-(-(-5))))"
        };
        public static void Speed()
        {
            var watch = new System.Diagnostics.Stopwatch();
            var calc = new Calculator();
            calc.StandartMath = true;
            var i = 0;
            Console.WriteLine("Test speed started...");
            foreach(string expr in speedExpressions)
            {
                watch.Reset();
                watch.Start();
                for (i = 0; i < itCount; ++i)
                {
                    calc.Calculate(expr);
                }
                watch.Stop();
                Console.WriteLine(expr + " ticks: " + ((decimal)(watch.ElapsedTicks))/itCount);
            }
            Console.WriteLine("Finished!");
        }
    }
}
