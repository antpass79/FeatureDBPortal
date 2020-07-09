using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public interface IUserProvider
    {
        AdUser CurrentUser { get; set; }
        bool Initialized { get; set; }
        Task Create(HttpContext context, IConfiguration config);
        Task<AdUser> GetAdUser(string userName, string password);
        Task<AdUser> GetAdUser(IIdentity identity);
        Task<AdUser> GetAdUser(string samAccountName);
        Task<AdUser> GetAdUser(Guid guid);
        Task<List<AdUser>> GetDomainUsers();
        Task<List<AdUser>> FindDomainUser(string search);
    }
}
