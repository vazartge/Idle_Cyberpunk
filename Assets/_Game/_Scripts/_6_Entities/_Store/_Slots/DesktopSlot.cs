using System;
using System.Collections;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    public class DesktopSlot : BaseSlot {
        private float assemblyTime = 5.0f; // Время сбора заказа
        public virtual Type AllowedProductType { get; }

        public Seller Seller {
            get => Unit as Seller;
            set => Unit = value;
        }

        public IEnumerator ProcessOrder(Order order) {
            if (order.ProductOrder.GetType() != AllowedProductType) {
                yield break;
            }

            yield return new WaitForSeconds(assemblyTime);
            // Обработка заказа...
        }
    }
}