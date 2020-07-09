using FeatureDBPortal.Server.Options;
using FeatureDBPortal.Server.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public class ADJwtTokenEncoder : IJwtTokenEncoder<AdUser>
    {
        private readonly IOptions<JwtAuthenticationOptions> _options;

        public ADJwtTokenEncoder(IOptions<JwtAuthenticationOptions> options)
        {
            _options = options;
        }

        async public Task<string> EncodeAsync(AdUser input)
        {
            var claims = await BuildClaimsAsync(input);

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: _options.Value.SigningCredentials);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        async virtual protected Task<IEnumerable<Claim>> BuildClaimsAsync(AdUser input)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            return await Task.FromResult<IEnumerable<Claim>>(claims);
        }
    }
}
