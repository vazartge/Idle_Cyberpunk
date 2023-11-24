using System;
using System.Collections;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots {
    public class DesktopSlot : BaseSlot {
       
        private float assemblyTime = 5.0f; // Время сбора заказа
        public virtual Type AllowedProductType { get; }
        public Seller Seller {
            get { return Unit as Seller; }
            set { Unit = value; }
        }

        // Метод для начала обработки заказа
        public IEnumerator ProcessOrder(Order order) {

            // Проверяем, соответствует ли продукт в заказе разрешенному типу продукта для этого стола
            if (order.Product.GetType() != AllowedProductType) {
                // Если нет, прерываем обработку
                yield break;
            }

            // Имитация времени на сбор заказа
            yield return new WaitForSeconds(assemblyTime);



        }
    }
}