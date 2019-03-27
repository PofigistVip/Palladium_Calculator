using System;
using System.Collections.Generic;

using static System.Console;

namespace Calculator
{
    class Program
    {
        static List<string> userConsts = new List<string>();
        static Calculator calculator = new Calculator();

        static void Main(string[] args)
        {
            foreach (string arg in args)
                if(arg == "-t" || arg == "test")
                    Test.Speed();
                else if (arg == "-c" || arg == "case")
                    calculator.IgnoreCase = true;
                else if (arg == "-m" || arg == "math")
                    calculator.StandartMath = true;
                else if (arg == "-z" || arg == "zero")
                    calculator.AllowDivisionByZero = true;
                else if (arg == "-h" || arg == "help")
                    Help();
                else if (ProcessString(arg) == 1)
                    return ;
            string str;
            while (true)
            {
                str = ReadLine();
                if (ProcessString(str) == 1)
                    break ;
            }
        }

        private static int ProcessString(string str)
        {
            str = str.Trim();
            string lowered = str.ToLower();
            if (lowered == "")
                return 0;
            if (lowered == "q" || lowered == "quit")
                return 1;
            if (lowered == "h" || lowered == "help")
                Help();
            else if (lowered == "t" || lowered == "test")
                Test.Speed();
            else if (lowered == "case")
                SwitchCaseIgnore();
            else if (lowered == "math")
                SwitchStandartMath();
            else if (lowered == "zero")
                SwitchDivisionByZero();
            else if (lowered == "clear")
                ConstClear();
            else if (lowered[0] == '=')
                SafeCalculation(str);
            else
                ParseConst(str);
            return 0;
        }

        private static void SwitchCaseIgnore()
        {
            calculator.IgnoreCase = !calculator.IgnoreCase;
            if (calculator.IgnoreCase)
                WriteLine("IgnoreCase mode: ON");
            else
                WriteLine("IgnoreCase mode: OFF");
        }

        private static void SwitchStandartMath()
        {
            calculator.StandartMath = !calculator.StandartMath;
            if (calculator.StandartMath)
                WriteLine("Standart mathematical functions " +
                    "and constants have been added");
            else
                WriteLine("Standart mathematical functions " +
                    "and constants have been removed");
        }

        private static void SwitchDivisionByZero()
        {
            calculator.AllowDivisionByZero =
                        !calculator.AllowDivisionByZero;
            if (calculator.AllowDivisionByZero)
                WriteLine("Division by zero have been allowed");
            else
                WriteLine("Division by zero have been prohibit");
        }

        private static void ConstClear()
        {
            WriteLine($"{userConsts.Count} user constants removed");
            foreach (string s in userConsts)
                calculator.Consts.Unset(s);
            userConsts.Clear();
        }

        private static void SafeCalculation(string str)
        {
            try
            {
                double result = calculator.Calculate(str);
                WriteLine(result);
            }
            catch (Exception e)
            {
                WriteLine("Opps: " + e.Message);
            }
        }

        private static void ParseConst(string str)
        {
            string[] splited = str.Split('=');
            if (splited.Length != 2 || splited[0].Length == 0 ||
                splited[1].Length == 0)
            {
                WriteLine("Unknown command");
                return ;
            }
            splited[0] = splited[0].Trim();
            splited[1] = splited[1].Trim();
            string constName = "";
            bool err = false;
            foreach (char c in splited[0])
                if (char.IsLetter(c) || c == '_')
                    constName += char.ToLower(c);
                else
                {
                    WriteLine("Only letters and '_' allowed in constant name");
                    err = true;
                    break;
                }
            if (!err)
            {
                double val;
                if (double.TryParse(splited[1].Replace('.', ','), out val))
                    calculator.Consts.Set(constName, val);
                else
                    WriteLine("Value is not a number");
            }
        }

        static void Help()
        {
            WriteLine("Palladium Calculator 1.1");
            WriteLine("Write 'constName = value' to add const");
            WriteLine("Write '=expression' to calculate it");
            WriteLine("Write 'math' to switch 'defaultMath' mode");
            WriteLine("Write 'dzero' to enable/disable DivisionByZero");
            WriteLine("Write clear to erase your constans");
            WriteLine("Write 'q' or 'quit' to exit");
            WriteLine("Write 'h' or 'help' to show this");
        }
    }
}
