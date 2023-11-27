using Assets._Game._Scripts._6_Entities._Units._Customers;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    public class CustomerSlot : BaseSlot {
        public _Units._Customers.Customer Customer {
            get => Unit as _Units._Customers.Customer;
            set => Unit = value;
        }
    }
}