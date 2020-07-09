using FeatureDBPortal.Server.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public class AdUserProvider : IUserProvider
    {
        private readonly IOptions<ActiveDirectoryOptions> _options;

        public AdUser CurrentUser { get; set; }
        public bool Initialized { get; set; }

        public AdUserProvider(IOptions<ActiveDirectoryOptions> options)
        {
            _options = options;
        }

        public async Task Create(HttpContext context, IConfiguration config)
        {
            CurrentUser = await GetAdUser(context.User.Identity);
            Initialized = true;
        }

        public Task<AdUser> GetAdUser(string userName, string password)
        {
            return Task.Run(() =>
            {
                try
                {
                    //PrincipalContext context = new PrincipalContext(ContextType.Domain, "it.esaote.priv", @"", "");
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, _options.Value.Domain);
                    if (!context.ValidateCredentials(userName, password))
                    {
                        return null;
                    }

                    UserPrincipal principal = new UserPrincipal(context);
                    //if (context != null)
                    //{
                    //    principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, identity.Name);
                    //}

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<AdUser> GetAdUser(IIdentity identity)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, "it.esaote.priv", @"esagroup\passalacqua", "ciriCODAK0620");
                    UserPrincipal principal = new UserPrincipal(context);

                    //if (context != null)
                    //{
                    //    principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, identity.Name);
                    //}

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<AdUser> GetAdUser(string samAccountName)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
                    }

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<AdUser> GetAdUser(Guid guid)
        {
            return Task.Run(() =>
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal principal = new UserPrincipal(context);

                    if (context != null)
                    {
                        principal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guid.ToString());
                    }

                    return AdUser.CastToAdUser(principal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving AD User", ex);
                }
            });
        }

        public Task<List<AdUser>> GetDomainUsers()
        {
            return Task.Run(() =>
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                principal.UserPrincipalName = "*@*";
                principal.Enabled = true;
                PrincipalSearcher searcher = new PrincipalSearcher(principal);

                var users = searcher
                    .FindAll()
                    .AsQueryable()
                    .Cast<UserPrincipal>()
                    .FilterUsers()
                    .SelectAdUsers()
                    .OrderBy(x => x.Surname)
                    .ToList();

                return users;
            });
        }

        public Task<List<AdUser>> FindDomainUser(string search)
        {
            return Task.Run(() =>
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain);
                UserPrincipal principal = new UserPrincipal(context);
                principal.SamAccountName = $"*{search}*";
                principal.Enabled = true;
                PrincipalSearcher searcher = new PrincipalSearcher(principal);

                var users = searcher
                    .FindAll()
                    .AsQueryable()
                    .Cast<UserPrincipal>()
                    .FilterUsers()
                    .SelectAdUsers()
                    .OrderBy(x => x.Surname)
                    .ToList();

                return users;
            });
        }

        // By Lapo
        //internal bool Authenticate(string domain, string userName, string password, ref string error_reason)
        //{
        //    bool authentic = false;
        //    error_reason = "";
        //    DirectoryEntry entry = null;
        //    try
        //    {
        //        entry = new DirectoryEntry("LDAP://" + domain,
        //                                   userName, password);
        //        object nativeObject = entry.NativeObject;
        //        authentic = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        error_reason = ex.ToString();
        //    }
        //    finally
        //    {
        //        if (entry != null)
        //        {
        //            entry.Dispose();
        //        }
        //    }
        //    return authentic;
        //}
    }
}
