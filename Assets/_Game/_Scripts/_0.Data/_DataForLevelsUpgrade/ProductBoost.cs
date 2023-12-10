namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    public class ProductBoost
    {
        public float ProductMultiplier { get; set; }
        public int Price { get; set; }

        public ProductBoost(float productMultiplier, int price)
        {
            ProductMultiplier = productMultiplier;
            Price = price;
        }
    }
}