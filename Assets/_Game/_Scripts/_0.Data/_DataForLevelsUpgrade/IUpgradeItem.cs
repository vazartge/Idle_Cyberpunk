namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade {
    public interface IUpgradeItem {
        // Используется для создания и обновления кнопок Апгрейда магазина(количество продавцов, покупателей, буст, скорость)
        public string Name { get; }
        public int Price { get; }
        public bool IsPurchased { get; }
    }
}
