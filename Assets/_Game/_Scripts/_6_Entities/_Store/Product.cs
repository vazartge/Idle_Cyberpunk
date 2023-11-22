namespace Assets._Game._Scripts._6_Entities._Store
{
    public enum ProductType {
        MechanicalEye,
        RoboticArm,
        IronHeart,
        Neurochip
    }
    public class Product {
        public ProductType Type { get; private set; }
        //public string Name { get; private set; }
        public float Price { get; private set; }

        public Product(ProductType type, float price) {
            Type = type;
            // Name = name;
            Price = price;
        }
    }
}