using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        async public static Task<T> GetValue<T>(this HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<T>(content);

            return value;
        }

        async public static Task<byte[]> GetByteArrayValue(this HttpResponseMessage httpResponseMessage)
        {
            return await httpResponseMessage.Content.ReadAsByteArrayAsync();
        }
    }
}
