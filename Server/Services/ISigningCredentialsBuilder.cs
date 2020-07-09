using Microsoft.IdentityModel.Tokens;

namespace FeatureDBPortal.Server.Services
{
    public interface ISigningCredentialsBuilder
    {
        SymmetricSecurityKey SigningKey { get; }
        ISigningCredentialsBuilder AddSecretKey(string secretKey);
        ISigningCredentialsBuilder AddAlgorithm(string algorithm);
        SigningCredentials Build();
    }
}
