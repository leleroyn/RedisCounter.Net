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
            if (!string.IsNullOrWhiteSpace(value))
            {
                _redisDb.ListRightPush(queueName, value);
            }
        }
    }
}
