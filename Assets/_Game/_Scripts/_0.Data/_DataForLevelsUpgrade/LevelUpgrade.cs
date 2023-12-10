using System.Collections.Generic;

namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    public class LevelUpgrade
    {
        public List<UpgradeOption> Buyers { get; set; }
        public List<UpgradeOption> Sellers { get; set; }
        public ProductBoost ProductionBoost { get; set; }
        public SpeedBoost SellersSpeedIncrease { get; set; }

        public LevelUpgrade(List<UpgradeOption> buyers, List<UpgradeOption> sellers, ProductBoost productionBoost,
            SpeedBoost sellersSpeedIncrease)
        {
            Buyers = buyers;
            Sellers = sellers;
            ProductionBoost = productionBoost;
            SellersSpeedIncrease = sellersSpeedIncrease;
        }
    }
}