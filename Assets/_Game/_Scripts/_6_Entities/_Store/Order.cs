using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Customers;

namespace Assets._Game._Scripts._6_Entities._Store
{
    public class Order
    {
        public int ID { get; }

        public Customer Customer { get; private set; }

        //public IProduct ProductOrder { get; private set; }
        public bool IsOrderInStore { get; set; }
        public bool IsOrderInCollecting { get; set; }
        public ProductType ProductType { get; set; }


        public Order(Customer customer, ProductType productType, int id)
        {
            ID = id;
            Customer = customer;

            ProductType = productType;
            //Debug.Log($"{Customer.IDSprites}.OrderID: {IDSprites}");

        }

        public ProductType GetProductType()
        {
            return ProductType;
        }

    }
}