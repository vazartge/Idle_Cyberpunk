using Assets._Game._Scripts._6_Entities._Store;
using Newtonsoft.Json;

namespace Assets._Game._Scripts._2_Game
{
    public class StoreStatsService
    {
        public string SaveToJson(StoreStats storeStats)
        {
            return JsonConvert.SerializeObject(storeStats);
        }


        public StoreStats LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<StoreStats>(json);
        }
    }
}