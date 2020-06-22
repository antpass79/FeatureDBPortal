using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SourceConnectionController : ControllerBase
    {
        public SourceConnectionController()
        {
        }

        [HttpGet]
        async public Task<Tuple<int?, IEnumerable<CombinationCellDTO>>> Get()
        {
            var tuple = new Tuple<int?, IEnumerable<CombinationCellDTO>>(5, new List<CombinationCellDTO>
            {
                new CombinationCellDTO() { Name = "Test 1" },
                new CombinationCellDTO() { Name = "Test 2" },
                new CombinationCellDTO() { Name = "Test 3" }
            });

            return await Task.FromResult(tuple);
        }
    }
}
