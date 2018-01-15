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

        private static volatile ConnectionMultiplexer _connection;
        private static readonly object lockObj = new object();

        private static void CreateConnectionMultiplexer(ConnectionMultiplexer connection)
        {
            if (_connection == null)
            {
                lock (lockObj)
                {
                    if (_connection == null)
                    {
                        _connection = connection;
                    }
                }
            }
        }

        public static void Initialize(ConnectionMultiplexer connection, int dataBase)
        {
            CreateConnectionMultiplexer(connection);
            RedisDb = connection.GetDatabase(dataBase);
            IsInitialize = true;
        }

        public static void Initialize(string connectionStr)
        {
            Initialize(connectionStr, 0);
        }

        public static void Initialize(string connectionStr, int dataBase)
        {
            Initialize(connectionStr, dataBase,null);
        }

        public static void Initialize(string connectionStr, int dataBase, EventHandler<ConnectionFailedEventArgs> connectionFailedEvent)
        {
            _connection = ConnectionMultiplexer.Connect(connectionStr);
            if (connectionFailedEvent != null)
            {
                _connection.ConnectionFailed += connectionFailedEvent;
            }
            Initialize(_connection, dataBase);
        }

        internal static void Verify()
        {
            if (!IsInitialize || RedisDb == null)
                throw new Exception("Redis服务未初始化，请先执行Initialize");
        }

    }
}
