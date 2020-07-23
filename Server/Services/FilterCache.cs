using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Services
{
    public class FilterCache : IFilterCache
    {
        private Dictionary<string, IReadOnlyList<IQueryableItem>> _cache = new Dictionary<string, IReadOnlyList<IQueryableItem>>();
        static object _lock = new object();

        public FilterCache()
        {
        }

        public void Add(string key, IEnumerable<IQueryableItem> items)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
                _cache.Add(key, items.ToList());
            }
        }

        public IReadOnlyList<IQueryableItem> Get(string key)
        {
            lock (_lock)
            {
                return _cache[key];
            }
        }
        public IReadOnlyList<IQueryableItem> Get<TEntity>()
            where TEntity : IQueryableEntity
        {
            lock (_lock)
            {
                return _cache[typeof(TEntity).Name];
            }
        }
    }
}
