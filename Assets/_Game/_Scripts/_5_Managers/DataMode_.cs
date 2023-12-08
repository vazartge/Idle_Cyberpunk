using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._0.Data._DataForUpgrade;
using Assets._Game._Scripts._2_Game;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public enum ProductType {
        MechanicalEyeProduct,
        RoboticArmProduct,
        IronHeartProduct,
        NeurochipProduct
    }

    public class DataMode_ : MonoBehaviour {
        [SerializeField] public ResourceData ResData;
        public Dictionary<ProductType, BaseUpgradeSO> dataForUpgradeDesktopsMap;
        [SerializeField] public IronHeartUpgradeSO ironHeartUpgradeSO;
        [SerializeField] public MechanicalEyeUpgradeSO mechanicalEyeUpgradeSo;
        [SerializeField] public NeurochipUpgradeSO neurochipUpgradeSo;
        [SerializeField] public RoboticArmUpgradeSO roboticArmUpgradeSO;

        [SerializeField] public GameObject desktopPrefab;

        [SerializeField] public GameObject prebuilderDesktopPrefab;
        [SerializeField] public GameObject starForTooltipPrafab;

        [SerializeField] private GameMode _gameMode;
        [SerializeField] private UIMode _uiMode;


        public Dictionary<ProductType, BaseUpgradeSO> DataForUpgradeDesktopsMap => dataForUpgradeDesktopsMap;
        public Dictionary<ProductType, int> MaxStarsOnLevelMap;
        public GameObject PrefabsForCreateDesktop => desktopPrefab;
        public GameObject PreafabsForCreatePrebuilderDesktop => prebuilderDesktopPrefab;
        public int GameLevel => Game.Instance.GameLevel;

        private void Awake() {
            //словарь данных для обновления столов 
            dataForUpgradeDesktopsMap = new Dictionary<ProductType, BaseUpgradeSO>()
            {
                { ProductType.IronHeartProduct , ironHeartUpgradeSO},
                { ProductType.MechanicalEyeProduct , mechanicalEyeUpgradeSo},
                { ProductType.NeurochipProduct , neurochipUpgradeSo},
                { ProductType.RoboticArmProduct , roboticArmUpgradeSO},
            };
            // Словарь для определения количества звезд в тултипе
            MaxStarsOnLevelMap =  new Dictionary<ProductType, int>();
            UpdateMaxStarsOnLevel();

        }
        public void Construct(GameMode gameMode, UIMode uiMode) {
            _gameMode = gameMode;
            _uiMode = uiMode;
            
        }
        public void UpdateMaxStarsOnLevel() {
            foreach (var kvp in dataForUpgradeDesktopsMap) {
                MaxStarsOnLevelMap[kvp.Key] = kvp.Value.GetMaxStarsForLevel(GameLevel);
            }
        }
        public int GetMaxStarsForProductType(ProductType productType) {
            if (MaxStarsOnLevelMap.TryGetValue(productType, out int maxStars)) {
                return maxStars;
            }
            return 0; // Возвращаем 0, если в словаре нет записи для данного типа продукта
        }

        public GameObject GetStarPrefab()
        {
            return starForTooltipPrafab;
        }

        public Sprite GetIconByProductType(ProductType productType) {
            var productInfo = ResData.ProductsInfo.FirstOrDefault(p => p.ProductType.ToString() == productType.ToString());
            return productInfo != null ? productInfo.ProductIcon : ResData.BaseIcon;
        }


        public BaseUpgradeSO GetProductUpgradeSO(ProductType productType) {
            return DataForUpgradeDesktopsMap[productType];
        }

        public GameObject GetPrefabForPrebuilderDesktop() {
            return PreafabsForCreatePrebuilderDesktop;
        }

        public GameObject GetPrefabForDesktop() {
            return PrefabsForCreateDesktop;
        }
    }
}