using System;
using System.Collections.Generic;

namespace Calculator
{
    public class ConstantCollection
    {
        Dictionary<string, double> consts =
            new Dictionary<string, double>();
        HashSet<string> constsOriginal = new HashSet<string>();
        HashSet<string> constsLowered = new HashSet<string>();

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
            if (ignore)
                return constsLowered.Contains(key.ToLower());
            return constsOriginal.Contains(key);
        }

        public void Set(string key, double value)
        {
            consts[key] = value;
            constsOriginal.Add(key);
            constsLowered.Add(key.ToLower());
        }

        public void Unset(string key)
        {
            consts.Remove(key);
            constsOriginal.Remove(key);
            constsLowered.Remove(key.ToLower());
        }

        public double Get(string key, bool ignore = false)
        {
            if (!ignore)
                return consts[key];
            key = key.ToLower();
            foreach (string k in consts.Keys)
                if (k.ToLower() == key)
                    return consts[k];
            throw new Exception();
        }

        public void Clear()
        {
            consts.Clear();
            constsOriginal.Clear();
            constsLowered.Clear();
        }
    }
}
