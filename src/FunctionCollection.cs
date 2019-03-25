using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class FunctionCollection
    {
        Dictionary<string, Func<double[], double>> funcs =
            new Dictionary<string, Func<double[], double>>();

        public Func<double[], double> this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public bool Contains(string key, bool ignore = false)
        {
            if (!ignore)
                return funcs.ContainsKey(key) && funcs[key] != null;
            key = key.ToLower();
            foreach (string k in funcs.Keys)
                if (k.ToLower() == key)
                    return funcs[k] != null;
            return false;
        }

        public void Set(string key, Func<double[], double> value)
        {
            funcs[key] = value ?? 
                throw new CalculatorException("Function reference can't be null");
        }

        public void Unset(string key)
        {
            funcs.Remove(key);
        }

        public Func<double[], double> Get(string key, bool ignore = false)
        {
            if (Contains(key, ignore))
            {
                if (!ignore)
                    return (funcs[key]);
                key = key.ToLower();
                foreach (string k in funcs.Keys)
                    if (k.ToLower() == key)
                        return funcs[k];
            }
            throw new Exception();
        }

        public void Clear()
        {
            funcs.Clear();
        }
    }
}
