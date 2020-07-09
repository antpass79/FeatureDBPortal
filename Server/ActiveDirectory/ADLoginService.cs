using FeatureDBPortal.Server.Services;
using FeatureDBPortal.Shared;
using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public class ADLoginService : IAsyncLoginService
    {
        private readonly IUserProvider _userProvider;
        private readonly IJwtTokenEncoder<AdUser> _jwtTokenEncoder;

        public ADLoginService(IUserProvider userProvider, IJwtTokenEncoder<AdUser> jwtTokenEncoder)
        {
            _userProvider = userProvider;
            _jwtTokenEncoder = jwtTokenEncoder;
        }

        async public Task<LoginResultDTO> LoginAsync(CredentialsDTO credentials)
        {
            if (string.IsNullOrEmpty(credentials.UserName) || string.IsNullOrEmpty(credentials.Password))
                return await Task.FromResult<LoginResultDTO>(BuildInvalidLoginResult());

            var userToVerify = await _userProvider.GetAdUser(credentials.UserName, credentials.Password);
            if (userToVerify == null)
                return await Task.FromResult<LoginResultDTO>(BuildInvalidLoginResult());

            return await Task.FromResult<LoginResultDTO>(new LoginResultDTO
            {
                Successful = true,
                Token = await _jwtTokenEncoder.EncodeAsync(userToVerify)
            });
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }

        private LoginResultDTO BuildInvalidLoginResult()
        {
            return new LoginResultDTO
            {
                Successful = false,
                Error = "Invalid credentials"
            };
        }
    }
}
