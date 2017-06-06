using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;

namespace RedisCounter
{
    /// <summary>
    /// Redis 计数器
    /// </summary>
    public class Counter
    {
        private static IDatabase _redisDb;
        private static bool _isInitialize = false;

        public static void Initialize(ConnectionMultiplexer connection)
        {
            _redisDb = connection.GetDatabase();

            _isInitialize = true;
        }

        public static void Initialize(string connectionStr)
        {
            _redisDb = ConnectionMultiplexer.Connect(connectionStr).GetDatabase();

            _isInitialize = true;
        }

        public static long Increment(string key, string field, TimeSpan? expiry = default(TimeSpan?), long value = 1)
        {
            Verify();

            long count = _redisDb.HashIncrement(key, field, value);

            if (expiry != null)
                _redisDb.KeyExpire(key, expiry);

            return count;
        }

        public static long Decrement(string key, string field, TimeSpan? expiry = default(TimeSpan?), long value = 1)
        {
            Verify();

            long count = _redisDb.HashDecrement(key, field, value);

            if (expiry != null)
                _redisDb.KeyExpire(key, expiry);

            return count;
        }

        public static long Get(string key, string field)
        {
            Verify();

            return (long)_redisDb.HashGet(key, field);
        }

        public static bool Delete(string key, string field)
        {
            Verify();

            return _redisDb.HashDelete(key, field);
        }

        private static void Verify()
        {
            if (!_isInitialize || _redisDb == null)
                throw new Exception("Redis计数器尚未初始化，请先执行Initialize");
        }
    }
}
