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

        /// <summary>
        /// 初始化计数器（如果已存在，不修改原来计数器的任何状态，包括过期时间）
        /// </summary>
        /// <param name="table"></param>
        /// <param name="expiry"></param>
        public static void Init(string counterName, TimeSpan? expiry)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(counterName))
            {
                if (expiry.HasValue)
                {
                    _redisDb.KeyExpire(counterName, expiry);
                }
            }
        }
        public static void SetExpiry(string counterName, TimeSpan expiry )
        {
            SessionFactory.Verify();           
            _redisDb.KeyExpire(counterName, expiry);
          
        }
        public static long Increment(string counterName, string field, long value = 1)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(counterName))
            {
                throw new Exception(string.Format("计数器:{0}不存在.", counterName));
            }     
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
