﻿using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public static class IdentityExtensions
    {
        public static IQueryable<UserPrincipal> FilterUsers(this IQueryable<UserPrincipal> principals) =>
            principals.Where(x => x.Guid.HasValue);

        public static IQueryable<AdUser> SelectAdUsers(this IQueryable<UserPrincipal> principals) =>
            principals.Select(x => AdUser.CastToAdUser(x));
    }
}
