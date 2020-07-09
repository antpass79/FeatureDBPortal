using FeatureDBPortal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResultDTO> Login(CredentialsDTO credentials);
        Task Logout();
    }
}
