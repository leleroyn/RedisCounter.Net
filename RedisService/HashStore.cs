using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisService
{
    public class HashStore
    {
        static IDatabase _redisDb = RedisService.RedisDb;
        public static void SetExpiry(string table, TimeSpan expiry)
        {
            RedisService.Verify();
            _redisDb.KeyExpire(table, expiry);
        }

        public static string GetValue(string table, string field)
        {
            RedisService.Verify();
            return _redisDb.HashGet(table, field);
        }

        public static string[] GetValue(string table, string[] fields)
        {
            RedisService.Verify();
            var parms =  fields.Select(o => (RedisValue)o).ToArray();
            return _redisDb.HashGet(table, parms).Select(o=>o.ToString()).ToArray();
        }

        public static bool SetValue(string table, string field, string value)
        {
            RedisService.Verify();
            return _redisDb.HashSet(table, field, value);
        }

        public static void SetValue(string table, Dictionary<string, string> hashEntry)
        {
            RedisService.Verify();
            var entries  = hashEntry.Select(o => new HashEntry(o.Key, o.Value)).ToArray();
            _redisDb.HashSet(table, entries);
        }
    }
}
