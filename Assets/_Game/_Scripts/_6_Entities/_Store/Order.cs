using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Customers;

namespace Assets._Game._Scripts._6_Entities._Store
{
    // Заказ товара Покупаетелем у Продавцов
    public class Order
    {
        public int ID { get; }

        public Customer Customer { get; private set; }

        //public IProduct ProductOrder { get; private set; }
        public bool IsOrderInStore { get; set; }
        public bool IsOrderInCollecting { get; set; }
        public ProductStoreType ProductStoreType { get; set; }


        public Order(Customer customer, ProductStoreType productStoreType, int id)
        {
            ID = id;
            Customer = customer;

            ProductStoreType = productStoreType;
            //Debug.Log($"{Customer.IDSprites}.OrderID: {IDSprites}");

        }

        public ProductStoreType GetProductType()
        {
            return ProductStoreType;
        }

    }
}