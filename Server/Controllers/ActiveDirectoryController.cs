using FeatureDBPortal.Server.ActiveDirectory;
using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly IUserProvider _provider;

        readonly IAsyncLoginService _loginService;

        public ActiveDirectoryController(IAsyncLoginService loginService, IUserProvider provider)
        {
            _loginService = loginService;
            _provider = provider;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<List<AdUser>> GetDomainUsers() => await _provider.GetDomainUsers();

        [HttpGet("[action]/{search}")]
        [Authorize]
        public async Task<List<AdUser>> FindDomainUser([FromRoute] string search) => await _provider.FindDomainUser(search);

        [HttpGet("[action]")]
        [Authorize]
        public AdUser GetCurrentUser() => _provider.CurrentUser;

        [HttpPost]
        async public Task<LoginResultDTO> Post([FromBody] CredentialsDTO credentials)
        {
            if (!ModelState.IsValid)
            {
                return new LoginResultDTO
                {
                    Successful = false,
                    Error = "Invalid credentials"
                };
            }

            var result = await _loginService.LoginAsync(credentials);

            return result;
        }

        [HttpDelete]
        [Authorize]
        async public Task Delete()
        {
            await _loginService.LogoutAsync();
        }
    }
}
