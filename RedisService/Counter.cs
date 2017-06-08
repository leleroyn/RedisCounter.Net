using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;

namespace RedisService
{
    /// <summary>
    /// Redis 计数器
    /// </summary>
    public class Counter
    {
        static IDatabase _redisDb = SessionFactory.RedisDb;

        public static void SetExpiry(string counterName, TimeSpan expiry)
        {
            SessionFactory.Verify();
            _redisDb.KeyExpire(counterName, expiry);
        }
        public static long Increment(string counterName, string field, long value = 1)
        {
            SessionFactory.Verify();
            long count = _redisDb.HashIncrement(counterName, field, value);
            return count;
        }

        public static long Decrement(string counterName, string field, long value = 1)
        {
            SessionFactory.Verify();
            long count = _redisDb.HashDecrement(counterName, field, value);
            return count;
        }

        public static long Get(string counterName, string field)
        {
            SessionFactory.Verify();
            return (long)_redisDb.HashGet(counterName, field);
        }

        public static bool Delete(string counterName, string field)
        {
            SessionFactory.Verify();
            return _redisDb.HashDelete(counterName, field);
        }

    }
}
