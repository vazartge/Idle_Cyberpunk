using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Customers;

namespace Assets._Game._Scripts._6_Entities._Store
{
    public class Order {
        public int ID { get; set; }
        public Customer Customer { get; private set; }
        public IProduct Product { get; private set; }
        public int TotalAmount { get; private set; }
        public int AmountTaken { get; private set; }
        public int AmountDelivered { get; private set; }

        public Order(Customer customer, IProduct product, int totalAmount) {
            Customer = customer;
            Product = product;
            TotalAmount = totalAmount;
            AmountTaken = 0;
            AmountDelivered = 0;
        }

        // Метод для взятия товара из ордера в работу
        public bool TryTakeProduct() {
            if (AmountTaken < TotalAmount) {
                AmountTaken++;
                return true;
            }
            return false;
        }

        // Метод для подтверждения доставки товара
        public void ConfirmDelivery() {
            if (AmountDelivered < AmountTaken) {
                AmountDelivered++;
                // Обновить UI покупателя, уменьшить количество ожидаемого товара
            }
        }

        // Проверка, завершен ли заказ
        public bool IsCompleted() {
            return AmountDelivered == TotalAmount;
        }
    }
}