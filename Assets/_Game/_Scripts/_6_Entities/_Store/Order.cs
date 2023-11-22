using Assets._Game._Scripts._6_Entities._Customers;
using Assets._Game._Scripts._6_Entities._Sellers;
using Unity.VisualScripting;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Order {

        public Seller Seller { get; private set; }
        public Customer Customer { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public Order( Product product, int quantity) {
            // Seller = seller;
            // Customer = customer;
            Product = product;
            Quantity = quantity;
        }
    }
}