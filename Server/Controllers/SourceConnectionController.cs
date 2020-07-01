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
        async public Task<Tuple<int?, IEnumerable<CellDTO>>> Get()
        {
            var tuple = new Tuple<int?, IEnumerable<CellDTO>>(5, new List<CellDTO>
            {
                new CellDTO() { Name = "Test 1" },
                new CellDTO() { Name = "Test 2" },
                new CellDTO() { Name = "Test 3" }
            });

            return await Task.FromResult(tuple);
        }
    }
}
