﻿using Newtonsoft.Json;

namespace Assets._Game._Scripts._2_Game
{
    
    // Сериализатор на Newtonsoft
    public class StoreStatsService {
        public string SaveToJson(StoreStats storeStats) {
            var settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Игнорирование циклических ссылок
            };

            return JsonConvert.SerializeObject(storeStats, settings);
        }

        public StoreStats LoadFromJson(string json) {
            return JsonConvert.DeserializeObject<StoreStats>(json);
        }
    }

}