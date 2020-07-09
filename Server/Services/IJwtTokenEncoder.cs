using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IJwtTokenEncoder<T>
    {
        Task<string> EncodeAsync(T input);
    }
}
