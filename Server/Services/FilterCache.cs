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

        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }
    }
}
