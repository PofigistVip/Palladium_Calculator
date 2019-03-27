using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Calculator
{
    public sealed partial class Calculator
    {
        public bool IgnoreCase { get; set; } = false;
        public bool AllowDivisionByZero { get; set; } = false;

        public ConstantCollection Consts =
            new ConstantCollection();

        public FunctionCollection Functions =
            new FunctionCollection();

        private static int sign;
        private static bool wasOpenBrc;

        private static NumberStyles numberStyles =
            NumberStyles.AllowDecimalPoint;
        private static IFormatProvider provider =
            new NumberFormatInfo { NumberDecimalSeparator = "." };


        public double Calculate(string expr)
        {
            CalcEnvironment env = new CalcEnvironment(expr.Length);
            for (int i = expr.Length - 1; i >= 0; --i)
                if (char.IsLetterOrDigit(expr[i]) || ".,+-*/^%()".IndexOf(expr[i]) != -1)
                    env.exprStack.Push(expr[i]);
            Calc_Parse(env);
            if (env.nodes.Count == 0)
                throw new CalculatorException("Not a valid string");
            FindConstValues(env);
            FindFuncValues(env);
            //PrintInfo(env);
            return Calc_Process(env);
        }

        private void FindConstValues(CalcEnvironment env)
        {
            ConstNode constNode;
            string cName;
            foreach (Node node in env.nodes)
            {
                if (node is ConstNode)
                {
                    constNode = node as ConstNode;
                    cName = constNode.constName;
                    if (Consts.Contains(cName, IgnoreCase))
                        constNode.number = Consts.Get(cName, IgnoreCase) * constNode.sign;
                    else
                        throw new MissingConstantException(constNode.constName);
                }
            }
        }

        private void FindFuncValues(CalcEnvironment env)
        {
            FuncNode funcNode;
            string fName;
            Func<double[], double> func;
            foreach (Node node in env.nodes)
            {
                if (node is FuncNode)
                {
                    funcNode = node as FuncNode;
                    fName = funcNode.name;
                    for (int i = 0; i < funcNode.args.Length; ++i)
                        funcNode.args_vals[i] = Calculate(funcNode.args[i]);
                    if (Functions.Contains(fName, IgnoreCase))
                    {
                        func = Functions.Get(fName, IgnoreCase);
                        funcNode.number = func(funcNode.args_vals) * funcNode.sign;
                    }
                    else
                        throw new MissingFunctionException(funcNode.name);
                }
            }
        }

        private static void PrintInfo(CalcEnvironment env)
        {
            LinkedListNode<Node> node = env.nodes.First;
            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Next;
            }
            Console.WriteLine("Count:" + env.nodes.Count);
            Console.WriteLine("=====================");
            Console.WriteLine("Operation Queue");
            for (int i = 0; i < env.opQueue.Count; ++i)
                Console.WriteLine($"{i + 1}\t{env.opQueue[i].Value}");
            Console.WriteLine("Count: " + env.opQueue.Count);
        }

        private static void Calc_Parse(CalcEnvironment env)
        {
            var parseStage = 0;
            var firstValue = true;
            while (env.Length != 0)
            {
                switch (parseStage)
                {
                    case 0: ParseOpenBrackets(env, ref firstValue); break;
                    case 1: ParseValue(env); break;
                    case 2: ParseCloseBrackets(env); break;
                    case 3: ParseOperation(env); break;
                }
                if (++parseStage == 4)
                    parseStage = 0;
            }
            if (env.brc != 0)
                throw new ParseException(env.i, "Missing closing bracket");
        }

        private static void ParseOperation(CalcEnvironment env)
        {
            LinkedListNode<Node> op;
            if ("^%*/+-".Contains(env.Char))
            {
                op = env.nodes.AddLast(new OperationNode(env.Char, env.brc));
                env.AddOpInQueue(op);
            }
            else
                throw new ParseException(env.i, env.Char, "Operation expected");
            env.NextChar();
        }

        private static void ParseOpenBrackets(CalcEnvironment env, ref bool firstValue)
        {
            char c;
            wasOpenBrc = false;
            sign = 1;
            do
            {
                c = env.Char;
                if (c == '(')
                {
                    wasOpenBrc = true;
                    ++env.brc;
                }
                else if (c == '-')
                {
                    if (wasOpenBrc || firstValue)
                    {
                        sign *= -1;
                        wasOpenBrc = false;
                        firstValue = false;
                    }
                    else
                        throw new ParseException(env.i, "Bad minus");
                }
                else
                    break;
                env.NextChar();
            } while (env.Length != 0);
        }

        private static void ParseValue(CalcEnvironment env)
        {
            if (char.IsDigit(env.Char))
                ParseNumber(env, sign);
            else if (char.IsLetter(env.Char) || env.Char == '_')
                ParseConstOrFunc(env, sign);
        }

        private static void ParseCloseBrackets(CalcEnvironment env)
        {
            char c;
            while (env.Length != 0)
            {
                c = env.Char;
                if (c == ')')
                {
                    --env.brc;
                    env.NextChar();
                }
                else
                    break;
            }
        }

        private static void ParseNumber(CalcEnvironment env, int sign)
        {
            var wasDigit = false;
            var wasDot = false;
            string str = "";
            double numb = 0.0;
            char c;
            do
            {
                c = env.Char;
                if (c == '.')
                {
                    if (wasDigit && !wasDot)
                    {
                        str += '.';
                        wasDot = true;
                    }
                    else
                        throw new ParseException(env.i, "Bad dot");
                }
                else if (char.IsDigit(c))
                {
                    str += c;
                    wasDigit = true;
                }
                else
                    break;
                env.NextChar();
            } while (env.Length != 0);
            
            if (!double.TryParse(str, numberStyles, provider, out numb))
                throw new ParseException(env.i, $"Bad number '{str}'");
            env.nodes.AddLast(new NumberNode(numb * sign));
        }

        private static void ParseConstOrFunc(CalcEnvironment env, int sign)
        {
            bool isFunc = false;
            List<string> args = null;
            string str = "";
            char c;
            do
            {
                c = env.Char;
                if (char.IsLetter(c) || c == '_')
                    str += c;
                else if (c == '(')
                {
                    isFunc = true;
                    args = ParseFuncArgs(env);
                    break;
                }
                else
                    break;
                env.NextChar();
            } while (env.Length != 0);
            if (isFunc)
                env.nodes.AddLast(new FuncNode(str, args.ToArray(), sign));
            else
                env.nodes.AddLast(new ConstNode(str, sign));
        }

        private static List<string> ParseFuncArgs(CalcEnvironment env)
        {
            List<string> args = new List<string>();
            int brc = 0;
            string arg = "";
            char c;
            while (env.Length != 0)
            {
                env.NextChar();
                c = env.Char;
                if (c == ')')
                {
                    --brc;
                    if (brc == -1)
                    {
                        env.NextChar();
                        break ;
                    }
                }
                else if (c == '(')
                {
                    ++brc;
                }
                else if (c == ',')
                {
                    if (arg != "")
                    {
                        args.Add(arg);
                        arg = "";
                    }
                    else
                        throw new ParseException(env.i, "Empty argument");
                }
                if (c != ',')
                    arg += c;
            }
            if (brc != -1)
                throw new ParseException(env.i, env.Char, "Missing function closing bracket");
            if (arg != "")
                args.Add(arg);
            else if (args.Count > 0)
                throw new ParseException(env.i, "Empty argument");
            return (args);
        }

        private double Calc_Process(CalcEnvironment env)
        {
            OperationNode operation;
            LinkedListNode<Node> a, b;
            NumberNode aNumb, bNumb;
            for (int i = 0; i < env.opQueue.Count; ++i)
            {
                operation = env.opQueue[i].Value as OperationNode;
                a = env.opQueue[i].Previous;
                b = env.opQueue[i].Next;
                aNumb = a.Value as NumberNode;
                bNumb = b.Value as NumberNode;
                switch (operation.operation)
                {
                    case '^': aNumb.number = Math.Pow(aNumb.number, bNumb.number); break;
                    case '%': aNumb.number %= bNumb.number; break;
                    case '*': aNumb.number *= bNumb.number; break;
                    case '/':
                        if (bNumb.number != 0 || AllowDivisionByZero)
                            aNumb.number /= bNumb.number;
                        else
                            throw new DivideByZeroException();
                        break;
                    case '+': aNumb.number += bNumb.number; break;
                    case '-': aNumb.number -= bNumb.number; break;
                }
                env.nodes.Remove(b);
                env.nodes.Remove(env.opQueue[i]);
            }
            return ((NumberNode)env.nodes.First.Value).number;
        }
    }
}
