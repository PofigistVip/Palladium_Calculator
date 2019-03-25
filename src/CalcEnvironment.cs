using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalcEnvironment
    {
        public string expr;
        public int i;
        public int brc;
        public LinkedList<Node> nodes;
        public List<LinkedListNode<Node>> opQueue;

        public CalcEnvironment(string expr)
        {
            this.expr = expr;
            i = 0;
            brc = 0;
            nodes = new LinkedList<Node>();
            opQueue = new List<LinkedListNode<Node>>();
        }

        public void AddOpInQueue(LinkedListNode<Node> node)
        {
            int priority = ((OperationNode)node.Value).priority;
            int i = 0;
            for (; i < opQueue.Count; ++i)
                if (priority > ((OperationNode)opQueue[i].Value).priority)
                    break;
            opQueue.Insert(i, node);
        }

        public char Char => expr[i];
        public int Length => expr.Length;
        public void NextChar() => ++i;
    }
}
