using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalculatorException : Exception
    {
        public CalculatorException(string message) :
            base(message)
        {

        }
    }

    class MissingConstantException : CalculatorException
    {
        public MissingConstantException(string constName) : 
            base($"Constant '{constName}' missing")
        {
            
        }
    }

    class MissingFunctionException : CalculatorException
    {
        public MissingFunctionException(string funcName) :
            base($"Function '{funcName}' missing")
        {

        }
    }

    class ParseException : CalculatorException
    {
        public ParseException(string message) :
            base($"Parse error: {message}")
        {

        }

        public ParseException(int i,  string mess) :
            base($"Parse error at index {i}: {mess}")
        {

        }

        public ParseException(int i, char c, string mess) :
            base($"Parse error at index {i}, symbol '{c}': {mess}")
        {

        }
    }

    class FunctionArgCountException : CalculatorException
    {
        public FunctionArgCountException(string name, int argc) :
            base($"Function '{name} doesn't take {argc} argument(s)'")
        {

        }
    }
}
