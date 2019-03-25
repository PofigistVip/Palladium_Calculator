using System;
using System.Linq;

namespace Calculator
{
    public sealed partial class Calculator
    {
        private bool standartMath = false;

        public bool StandartMath
        {
            get { return standartMath; }
            set
            {
                if (standartMath != value)
                {
                    if (value == true) SetStandartMath();
                    else UnsetStandartMath();
                    standartMath = value;
                }
            }
        }

        private void SetStandartMath()
        {
            Consts.Set("PI", Math.PI);
            Consts.Set("E", Math.E);
            Functions.Set("abs", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("abs", args.Length);
                return Math.Abs(args[0]);
            });
            Functions.Set("sqrt", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("sqrt", args.Length);
                return Math.Sqrt(args[0]);
            });
            Functions.Set("exp", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("exp", args.Length);
                return Math.Exp(args[0]);
            });
            Functions.Set("log", (args) =>
            {
                if (args.Length == 1)
                    return Math.Log(args[0]);
                else if (args.Length == 2)
                    return Math.Log(args[0], args[1]);
                throw new FunctionArgCountException("log", args.Length);
            });
            Functions.Set("sum", (args) =>
            {
                if (args.Length == 0)
                    throw new FunctionArgCountException("sum", args.Length);
                return args.Sum();
            });
            Functions.Set("min", (args) =>
            {
                if (args.Length == 0)
                    throw new FunctionArgCountException("min", args.Length);
                return args.Min();
            });
            Functions.Set("max", (args) =>
            {
                if (args.Length == 0)
                    throw new FunctionArgCountException("max", args.Length);
                return args.Max();
            });
            Functions.Set("avg", (args) =>
            {
                if (args.Length == 0)
                    throw new FunctionArgCountException("avg", args.Length);
                return args.Average();
            });
            Functions.Set("sin", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("sin", args.Length);
                return Math.Sin(args[0]);
            });
            Functions.Set("cos", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("cos", args.Length);
                return Math.Cos(args[0]);
            });
            Functions.Set("tan", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("tan", args.Length);
                return Math.Tan(args[0]);
            });
            Functions.Set("round", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("round", args.Length);
                return Math.Round(args[0]);
            });
            Functions.Set("ceiling", (args) =>
            {
                if (args.Length != 1)
                    throw new FunctionArgCountException("ceiling", args.Length);
                return Math.Ceiling(args[0]);
            });
        }

        private void UnsetStandartMath()
        {
            string[] consts = { "PI", "E"};
            foreach (string s in consts)
                Consts.Unset(s);

            string[] funcs = 
                { "abs", "sqrt", "exp", "log", "sum", "min", "max", "avg",
                "sin", "cos", "tan", "round", "ceiling"};
            foreach (string s in funcs)
                Functions.Unset(s);
        }
    }
}
