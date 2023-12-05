using Assets._Game._Scripts._5_Managers;
using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._4_Services {
    public class WeightsProductMap {
        public int Counts;
        public float[] Weights;
    }
    public class ProductRandomizerService {
        // Список весов для всех продуктов
        private List<ProductWeight> _productWeights;
        private WeightsProductMap[] _weightsMap;

        public ProductRandomizerService() {
            InitializeWeights();
        }

        private void InitializeWeights() {
            // Заполняем веса для каждого продукта и каждого уровня
            _weightsMap = new WeightsProductMap[] {
                new WeightsProductMap
                {
                    Counts = 1,
                    Weights = new float[] {1f}
                },
                new WeightsProductMap {
                    Counts = 2,
                    Weights = new float[] {0.4f,0.6f}
                },
                new WeightsProductMap {
                    Counts = 3,
                    Weights = new float[] {0.21f, 0.32f, 0.47f}
                },
                new WeightsProductMap {
                    Counts = 4,
                    Weights = new float[] {0.12f,0.19f,0.28f,0.41f}
                },
            };

        }


        // Метод для выбора продукта на основе весов типов продуктов
        public ProductType GetRandomProductType(List<ProductType> availableTypes) {
            // Проверяем, есть ли доступные типы товаров
            if (!availableTypes.Any()) {
                throw new InvalidOperationException("No product types available.");
            }

            // Получаем веса для доступных типов товаров из _weightsMap
            var weightsForAvailableTypes = _weightsMap.FirstOrDefault(row => row.Counts == availableTypes.Count)?.Weights;
            if (weightsForAvailableTypes == null || !weightsForAvailableTypes.Any()) {
                throw new InvalidOperationException("No weights available for the number of product types.");
            }

            // Создаём список весов для доступных типов товаров
            _productWeights = availableTypes.Select((t, i) => new ProductWeight(t, weightsForAvailableTypes[i])).ToList();

            // Выбираем случайный тип товара на основе весов
            var randomProductType = _productWeights.OrderBy(pw => Random.value * pw.Weight).First().ProductType;

            return randomProductType;
        }

    }
}
