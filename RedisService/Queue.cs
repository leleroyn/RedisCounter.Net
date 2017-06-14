using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisService
{
    public class Queue
    {
        static IDatabase _redisDb = SessionFactory.RedisDb;
        /// <summary>
        /// 初始化队列（如果已存在，不修改原来队列的任何状态，包括过期时间）
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="expiry"></param>
        public static void Init(string queueName, TimeSpan? expiry)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(queueName))
            {
                if (expiry.HasValue)
                {
                    _redisDb.KeyExpire(queueName, expiry);
                }
            }
        }
        public static string Pop(string queueName)
        {
            SessionFactory.Verify();
            var value = _redisDb.ListLeftPop(queueName);
            return value;
        }
        /// <summary>
        /// 向队列中添加数据
        /// </summary>
        /// <param name="value"></param>
        public static void Push(string queueName,string value)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(queueName))
            {
                throw new Exception(string.Format("队列:{0}不存在.", queueName));
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                _redisDb.ListRightPush(queueName, value);
            }
        }
    }
}
