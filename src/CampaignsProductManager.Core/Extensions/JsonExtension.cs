using Newtonsoft.Json;

namespace CampaignsProductManager.Core.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
