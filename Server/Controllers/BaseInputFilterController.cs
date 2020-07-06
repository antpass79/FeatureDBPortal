using AutoMapper;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        protected IGenericRepository<TEntity> Repository { get; }

        protected BaseInputFilterController(
            ILogger<TController> logger,
            IMapper mapper,
            IGenericRepository<TEntity> repository)
        {
            _logger = logger;
            _mapper = mapper;
            Repository = repository;
        }

        [HttpGet]
        async public Task<IEnumerable<TDTO>> Get()
        {
            var items = GetItems();
            return await Task.FromResult(
                _mapper
                    .Map<IEnumerable<TDTO>>(items)
                    .OrderBy(item => item.OrderableProperty));
        }

        virtual protected IEnumerable<TEntity> GetItems()
        {
            return Repository.Get();
        }
    }
}
