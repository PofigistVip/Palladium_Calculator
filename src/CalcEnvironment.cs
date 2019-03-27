using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalcEnvironment
    {
        
        public int i;
        public int brc;

        public Stack<char> exprStack;
        public LinkedList<Node> nodes;
        public List<LinkedListNode<Node>> opQueue;


        public CalcEnvironment(int capacity)
        {
            i = 0;
            brc = 0;
            exprStack = new Stack<char>(capacity);
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

        public char Char => exprStack.Peek();
        public int Length => exprStack.Count;
        public void NextChar() => exprStack.Pop();
    }
}
