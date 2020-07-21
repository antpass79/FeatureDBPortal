using AutoMapper;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Repositories;
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

        protected BaseInputFilterController(
            ILogger<TController> logger,
            IMapper mapper,
            IGenericRepository<TEntity> repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        async public Task<IEnumerable<TDTO>> Get()
        {
            var query = PreManipulation(_repository.Query());

            var items = await Task.FromResult(
                _mapper
                    .Map<IEnumerable<TDTO>>(query.ToList()));

            return PostManipulation(items);
        }

        abstract protected IQueryable<TDTO> PreManipulation(IQueryable<TEntity> query);

        virtual protected IEnumerable<TDTO> PostManipulation(IEnumerable<TDTO> items)
        {
            return items;
        }
    }
}
