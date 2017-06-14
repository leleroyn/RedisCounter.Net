using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisService
{
    public class HashStore
    {
        static IDatabase _redisDb = SessionFactory.RedisDb;

        /// <summary>
        /// 初始化哈希表（如果已存在，不修改原来哈希表的任何状态，包括过期时间）
        /// </summary>
        /// <param name="table"></param>
        /// <param name="expiry"></param>
        public static void Init(string table, TimeSpan? expiry)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(table))
            {
                if (expiry.HasValue)
                {
                    _redisDb.KeyExpire(table, expiry);
                }
            }
        }
        public static void SetExpiry(string table, TimeSpan expiry)
        {
            SessionFactory.Verify();
            _redisDb.KeyExpire(table, expiry);
        }

        public static string GetValue(string table, string field)
        {
            SessionFactory.Verify();
            return _redisDb.HashGet(table, field);
        }

        public static string[] GetValue(string table, string[] fields)
        {
            SessionFactory.Verify();
            var parms =  fields.Select(o => (RedisValue)o).ToArray();
            return _redisDb.HashGet(table, parms).Select(o=>o.ToString()).ToArray();
        }

        public static bool SetValue(string table, string field, string value)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(table))
            {
                throw new Exception(string.Format("哈希表:{0}不存在.", table));
            }            
            return _redisDb.HashSet(table, field, value);
        }

        public static void SetValue(string table, Dictionary<string, string> hashEntry)
        {
            SessionFactory.Verify();
            if (!_redisDb.KeyExists(table))
            {
                throw new Exception(string.Format("哈希表:{0}不存在.", table));
            } 
            var entries  = hashEntry.Select(o => new HashEntry(o.Key, o.Value)).ToArray();
            _redisDb.HashSet(table, entries);
        }
    }
}
