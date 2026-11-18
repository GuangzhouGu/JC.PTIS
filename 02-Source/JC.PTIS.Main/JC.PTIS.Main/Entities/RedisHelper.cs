using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JC.PTIS.Common;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace JC.PTIS.Main.Entities
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisHelper : Singleton<RedisHelper>
    {
        static ConnectionMultiplexer _conn;
        static IDatabase _db;
        public bool IsConnected { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="connectionString">连接字符串：127.0.0.1:6379,password=123456,defaultDatabase=0</param>
        public bool Init(string connectionString)
        {
            if (_conn == null || !_conn.IsConnected)
            {
                _conn = ConnectionMultiplexer.Connect(connectionString);
            }
            _db = _conn.GetDatabase();
            IsConnected = _db != null;
            return _db != null;
        }
        public void Close()
        {
            _conn.Close();
        }

        #region String 缓存
        /// <summary>
        /// 设置字符串缓存
        /// </summary>
        public bool StringSet(string key, string value, TimeSpan? expiry = null)
        {
            return _db.StringSet(key, value, expiry, When.Always, CommandFlags.None);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public string StringGet(string key)
        {
            return _db.StringGet(key);
        }

        /// <summary>
        /// 设置对象（Json序列化）
        /// </summary>
        public bool ObjectSet<T>(string key, T obj, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(obj);
            return StringSet(key, json, expiry);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public T ObjectGet<T>(string key)
        {
            var json = StringGet(key);
            if (string.IsNullOrEmpty(json)) return default;
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 数字自增
        /// </summary>
        public long StringIncr(string key, long value = 1)
        {
            return _db.StringIncrement(key, value);
        }

        /// <summary>
        /// 数字自减
        /// </summary>
        public long StringDecr(string key, long value = 1)
        {
            return _db.StringDecrement(key, value);
        }
        #endregion

        #region Hash
        public bool HashSet(string key, string field, string value)
        {
            return _db.HashSet(key, field, value);
        }

        public string HashGet(string key, string field)
        {
            return _db.HashGet(key, field);
        }

        public Dictionary<string, string> HashGetAll(string key)
        {
            var entries = _db.HashGetAll(key);
            return entries.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());
        }

        public long HashDel(string key, params string[] fields)
        {
            var redisFields = fields.Select(x => (RedisValue)x).ToArray();
            return _db.HashDelete(key, redisFields);
        }
        #endregion

        #region Key通用
        public bool KeyExists(string key)
        {
            return _db.KeyExists(key);
        }

        public bool KeyDelete(string key)
        {
            return _db.KeyDelete(key);
        }

        public bool KeyExpire(string key, TimeSpan expiry)
        {
            return _db.KeyExpire(key, expiry);
        }

        public TimeSpan? KeyTtl(string key)
        {
            return _db.KeyTimeToLive(key);
        }
        // 替换异步流实现，兼容 C# 7.3
        public List<string> GetAllKeys(string pattern = "*", int pageSize = 100)
        {
            try
            {
                var server = _conn.GetServer(_conn.GetEndPoints()[0]);
                var keys = new List<string>();
                foreach (var key in server.Keys(pattern: pattern, pageSize: pageSize))
                {
                    keys.Add(key);
                }
                return keys;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 分布式锁
        /// <summary>
        /// 获取分布式锁
        /// 使用 SET key value NX EX 原子命令，防止死锁
        /// </summary>
        /// <param name="lockKey">锁名称</param>
        /// <param name="lockValue">锁标识（必须唯一，释放锁要校验）</param>
        /// <param name="expireTime">锁过期时间</param>
        /// <returns>true=获取锁成功</returns>
        public bool LockTake(string lockKey, string lockValue, TimeSpan expireTime)
        {
            // NX:不存在才设置；EX:过期时间
            return _db.StringSet(lockKey, lockValue, expireTime, When.NotExists);
        }

        /// <summary>
        /// 释放分布式锁（Lua脚本原子释放，防止误删别人的锁）
        /// 只有锁的值和当前传入lockValue一致才删除
        /// </summary>
        public bool LockRelease(string lockKey, string lockValue)
        {
            // Lua脚本：if redis.call('GET',KEYS[1]) == ARGV[1] then return redis.call('DEL',KEYS[1]) else return 0 end
            const string script = @"
                if redis.call('GET',KEYS[1]) == ARGV[1] then
                    return redis.call('DEL',KEYS[1])
                else
                    return 0
                end";
            var res = _db.ScriptEvaluate(script, new RedisKey[] { lockKey }, new RedisValue[] { lockValue });
            return (int)res == 1;
        }

        /// <summary>
        /// 带锁执行委托（自动获取+释放锁）
        /// </summary>
        /// <param name="lockKey">锁key</param>
        /// <param name="expireTime">锁过期时间</param>
        /// <param name="action">业务代码</param>
        /// <param name="retryCount">获取锁重试次数</param>
        /// <param name="retryDelayMs">重试间隔ms</param>
        /// <returns>true：执行业务成功拿到锁；false：抢锁失败</returns>
        public bool LockRun(string lockKey, TimeSpan expireTime, Action action, int retryCount = 0, int retryDelayMs = 100)
        {
            string lockValue = Guid.NewGuid().ToString("N");
            int tryTimes = 0;
            while (tryTimes <= retryCount)
            {
                if (LockTake(lockKey, lockValue, expireTime))
                {
                    try
                    {
                        action.Invoke();
                    }
                    finally
                    {
                        LockRelease(lockKey, lockValue);
                    }
                    return true;
                }
                tryTimes++;
                if (tryTimes <= retryCount)
                {
                    Task.Delay(retryDelayMs).Wait();
                }
            }
            return false;
        }
        #endregion

        #region 异步版本
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _db.StringSetAsync(key, value, expiry, When.Always, CommandFlags.None);
        }

        public async Task<string> StringGetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> ObjectSetAsync<T>(string key, T obj, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(obj);
            return await StringSetAsync(key, json, expiry);
        }

        public async Task<T> ObjectGetAsync<T>(string key)
        {
            var json = await StringGetAsync(key);
            if (string.IsNullOrEmpty(json)) return default;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<bool> LockTakeAsync(string lockKey, string lockValue, TimeSpan expireTime)
        {
            return await _db.StringSetAsync(lockKey, lockValue, expireTime, When.NotExists);
        }

        public async Task<bool> LockReleaseAsync(string lockKey, string lockValue)
        {
            const string script = @"
                if redis.call('GET',KEYS[1]) == ARGV[1] then
                    return redis.call('DEL',KEYS[1])
                else
                    return 0
                end";
            var res = await _db.ScriptEvaluateAsync(script, new RedisKey[] { lockKey }, new RedisValue[] { lockValue });
            return (int)res == 1;
        }
        #endregion
    }
}
