﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadooAPI.Utills
{
    public static class ExtMethods
    {
        private static ILogger _logger;
        public static void Configure(ILogger logger)
        {
            _logger = logger;
        }
        public static string DictionaryToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return string.Join(";", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray());
        }

        public static string DictionaryToJson<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var entries = dictionary.Select(d =>
         string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }

        public static void LogException(this Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogTrace(e.StackTrace);
        }
    }
}
