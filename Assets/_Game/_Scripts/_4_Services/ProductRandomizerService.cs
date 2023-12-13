using Assets._Game._Scripts._6_Entities._Store._Products;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._4_Services {
    public class WeightsProductMap {
        public int Counts;
        public float[] Weights;
    }

    public class ProductRandomizerService {
        private Dictionary<int, WeightsProductMap> _weightsMapByCount;

        public ProductRandomizerService() {
            InitializeWeights();
        }

        private void InitializeWeights() {
            _weightsMapByCount = new Dictionary<int, WeightsProductMap> {
                { 1, new WeightsProductMap { Counts = 1, Weights = new[] {1f} } },
                { 2, new WeightsProductMap { Counts = 2, Weights = new[] {0.4f, 0.6f} } },
                { 3, new WeightsProductMap { Counts = 3, Weights = new[] {0.21f, 0.32f, 0.47f} } },
                { 4, new WeightsProductMap { Counts = 4, Weights = new[] {0.12f, 0.19f, 0.28f, 0.41f} } }
            };
        }

        public ProductType? GetRandomProductType(List<ProductType> availableProductTypes) {
            if (availableProductTypes == null || availableProductTypes.Count == 0) {
                Debug.Log("Нет доступных типов продуктов.");
                return null;
            }

            // Сортируем доступные типы по приоритету
            var sortedProductTypes = availableProductTypes.OrderBy(type => type.GetPriority()).ToList();

            // Получаем количество доступных типов продуктов
            int count = sortedProductTypes.Count;

            // Получаем веса из соответствующего объекта WeightsProductMap
            if (!_weightsMapByCount.TryGetValue(count, out WeightsProductMap weightsMap)) {
                Debug.Log("Веса для данного количества продуктов не найдены.");
                return null;
            }

            float[] weights = weightsMap.Weights;

            // Генерируем случайное число
            float totalWeight = weights.Sum();
            float randomValue = UnityEngine.Random.Range(0, totalWeight);

            // Выбираем продукт на основе случайного значения
            float weightSum = 0;
            for (int i = 0; i < weights.Length; i++) {
                weightSum += weights[i];
                if (randomValue <= weightSum) {
                    return sortedProductTypes[i];
                }
            }

            // В случае если что-то пошло не так, возвращаем null
            return null;
        }
    }
}
