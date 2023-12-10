namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    public class SpeedBoost
    {
        public float SpeedMultiplier { get; set; }
        public int Price { get; set; }

        public SpeedBoost(float speedMultiplier, int price)
        {
            SpeedMultiplier = speedMultiplier;
            Price = price;
        }
    }
}