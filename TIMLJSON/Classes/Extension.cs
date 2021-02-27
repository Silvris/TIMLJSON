using System;
using System.Collections.Generic;
using System.Text;

namespace TIMLJSON.Classes
{
    public static class Extension
    {
        public static TValue GetValueOrDefault<TKey, TValue>
    (this IDictionary<TKey, TValue> dictionary,
     TKey key,
     TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
