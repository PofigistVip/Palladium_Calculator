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
            "5.5",
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
                Console.WriteLine(expr +
                    "\tms: " + ((decimal)(watch.ElapsedMilliseconds)) / itCount +
                    " (ticks: " + ((decimal)(watch.ElapsedTicks))/itCount + ")");
            }
            Console.WriteLine("Finished!");
        }
        static string[] correctExpressions =
        {
            "5.5-4.5",
            "(5^2 - 5 + 30 *(-10 + 5) / 50) % 5 - 1",
            "(x^2-x+30*(-10+x)/50)%x-1",
            "sum(1, 2, 3, 4, 3+2)+min(-5, 10, -10)-255^0-3"
        };
        public static void Correct()
        {
            var calc = new Calculator();
            calc.StandartMath = true;
            calc.Consts.Set("x", 5.0);
            var i = 0;
            Console.WriteLine("Correct test started...");
            foreach (string expr in correctExpressions)
            {
                if (calc.Calculate(expr) == 1)
                    Console.WriteLine(expr + " - OK");
                else
                    Console.WriteLine(expr + " - ERROR");
            }
            Console.WriteLine("Finished!");
        }
    }
}
