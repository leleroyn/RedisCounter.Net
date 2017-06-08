using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisService
{
    public class SessionFactory
    {
        public static IDatabase RedisDb { get; private set; }
        public static bool IsInitialize { get; private set; }

        public static void Initialize(ConnectionMultiplexer connection)
        {
            RedisDb = connection.GetDatabase();
            IsInitialize = true;
        }

        public static void Initialize(string connectionStr)
        {
            RedisDb = ConnectionMultiplexer.Connect(connectionStr).GetDatabase();
            IsInitialize = true;
        }

        internal static void Verify()
        {
            if (!IsInitialize || RedisDb == null)
                throw new Exception("Redis计数器尚未初始化，请先执行Initialize");
        }

    }
}
