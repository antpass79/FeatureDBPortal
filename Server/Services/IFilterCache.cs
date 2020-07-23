using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Services
{
    public interface IFilterCache
    {
        void Add(string key, IEnumerable<IQueryableItem> items);
        IReadOnlyList<IQueryableItem> Get(string key);
        IReadOnlyList<IQueryableItem> Get<TEntity>()
            where TEntity: IQueryableEntity;
    }
}
