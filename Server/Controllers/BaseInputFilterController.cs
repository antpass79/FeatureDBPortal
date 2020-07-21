using AutoMapper;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    public abstract class BaseInputFilterController<TEntity, TDTO, TController> : ControllerBase
        where TEntity: IQueryableCombination
        where TDTO: IOrderablePropertyName
        where TController: BaseInputFilterController<TEntity, TDTO, TController>
    {
        private readonly ILogger<TController> _logger;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IFilterCache _filterCache;

        protected BaseInputFilterController(
            ILogger<TController> logger,
            IMapper mapper,
            IGenericRepository<TEntity> repository,
            IFilterCache filterCache)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _filterCache = filterCache;
        }

        [HttpGet]
        async public Task<IEnumerable<TDTO>> Get()
        {
            var query = PreManipulation(_repository.Query());

            var items = await Task.FromResult(
                _mapper
                    .Map<IEnumerable<TDTO>>(query.ToList()));

            PutInCache(items);

            return PostManipulation(items);
        }

        abstract protected IQueryable<TDTO> PreManipulation(IQueryable<TEntity> query);

        virtual protected IEnumerable<TDTO> PostManipulation(IEnumerable<TDTO> items)
        {
            return items;
        }

        private void PutInCache(IEnumerable<TDTO> items)
        {
            _filterCache.Add(typeof(TEntity).Name, items.Cast<IQueryableItem>());
        }
    }
}
