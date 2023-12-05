using System;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Order {
        public int ID { get; }
        public Customer Customer { get; private set; }
        //public IProduct ProductOrder { get; private set; }
        public bool IsOrderInStore { get; set; }
        public bool IsOrderInCollecting { get; set; }
        public ProductType ProductType { get; set; }
       

        public Order(Customer customer, ProductType productType,  int id)
        {
            ID = id;
            Customer = customer;
            
            ProductType = productType;
            //Debug.Log($"{Customer.ID}.OrderID: {ID}");
            
        }

        public ProductType GetProductType()
        {
            return ProductType;
        }



        // Метод для взятия товара из ордера в работу
        // public bool TryTakeProduct() {
        //     lock (this) {
        //         if (AmountTaken < TotalAmount) {
        //             AmountTaken++;
        //
        //             return true;
        //         }
        //         return false;
        //     }
        // }

        // // Метод для подтверждения доставки товара
        // public void ConfirmDelivery() {
        //     if (AmountDelivered < AmountTaken) {
        //         AmountDelivered++;
        //         // Обновить UI покупателя, уменьшить количество ожидаемого товара
        //     }
        //     Debug.Log($"Продавец {Customer.ID}.Ордер {this.ID}: получен товар");
        //     if (IsCompleted())
        //     {
        //         OrderCompleted();
        //     }
        // }
        //
        // // Проверка, завершен ли заказ
        // public bool IsCompleted() {
        //     Debug.Log($"Продавец {Customer.ID}.Ордер {this.ID}: AmountDelivered= {AmountDelivered } и TotalAmount= {TotalAmount}");
        //
        //     return AmountDelivered >= TotalAmount;
        // }
        // private void OrderCompleted()
        // {
        //     
        // }
    }
}