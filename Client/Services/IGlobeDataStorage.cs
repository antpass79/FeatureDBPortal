using FeatureDBPortal.Client.Models;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IGlobeDataStorage
    {
        Task StoreAsync(GlobeLocalStorageData data);
        Task<GlobeLocalStorageData> GetAsync();
        Task RemoveAsync();
    }
}
