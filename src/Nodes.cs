using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Node
    {
        public NodeType type;
    }

    internal class FuncNode : NumberNode
    {
        public string name;
        public string[] args;
        public int sign;

        public Func<double[], double> func;
        public double[] args_vals;

        public FuncNode(string name, string[] args, int sign)
        {
            type = NodeType.Function;
            this.name = name;
            this.args = args;
            this.sign = sign;
            args_vals = new double[args.Length];
            func = null;
        }

        public override string ToString()
        {
            string text = $"Function '{name}', sign: {sign}. Args: ";
            foreach (string arg in args)
                text += arg + "; ";
            return text;
        }
    }

    internal class ConstNode : NumberNode
    {
        public string constName;
        public int sign;

        public ConstNode(string constName, int sign)
        {
            this.constName = constName;
            this.sign = sign;
        }

        public override string ToString()
        {
            return $"Const '{constName}', sign: {sign}";
        }
    }

    internal class OperationNode : Node
    {
        public char operation;
        public int priority;

        public OperationNode(char operation, int brc)
        {
            type = NodeType.Operation;
            this.operation = operation;
            priority = brc * 3;
            if (operation == '^' || operation == '%')
                priority += 2;
            else if (operation == '*' || operation == '/')
                priority += 1;
        }

        public override string ToString()
        {
            return $"Operation: '{operation}', priority: {priority}";
        }
    }

    internal class NumberNode : Node
    {
        public double number;

        public NumberNode()
        {

        }

        public NumberNode(double number)
        {
            type = NodeType.Number;
            this.number = number;
        }

        public override string ToString()
        {
            return $"Number: {number}";
        }
    }

    internal enum NodeType
    {
        Function,
        Const,
        Operation,
        Number
    }
}
