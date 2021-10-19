
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadooAPI.Utills
{
    public static class ExtMethods
    {

        public static string DictionaryToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return string.Join(";", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray());
        }

        public static string KeyValuePairToString<TKey, TValue>(this IDictionary<TKey, TValue> pair)
        {
            return string.Join(";", pair.Select(kv => kv.Key + "=" + kv.Value).ToArray());
        }

        public static string DictionaryToJson<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var entries = dictionary.Select(d =>
         string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }

    }
}
