using System;
using System.Collections.Generic;

namespace Calculator
{
    public class ConstantCollection
    {
        Dictionary<string, double> consts =
            new Dictionary<string, double>();

        public double this[string key]
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
                return consts.ContainsKey(key);
            key = key.ToLower();
            foreach (string k in consts.Keys)
                if (k.ToLower() == key)
                    return true;
            return false;
        }

        public void Set(string key, double value)
        {
            consts[key] = value;
        }

        public void Unset(string key)
        {
            consts.Remove(key);
        }

        public double Get(string key, bool ignore = false)
        {
            if (Contains(key, ignore))
            {
                if (!ignore)
                    return consts[key];
                key = key.ToLower();
                foreach (string k in consts.Keys)
                    if (k.ToLower() == key)
                        return consts[k];
            }
                
            throw new Exception();
        }

        public void Clear()
        {
            consts.Clear();
        }
    }
}
