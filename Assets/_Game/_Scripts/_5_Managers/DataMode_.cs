using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._0.Data._DataForUpgrade;
using UnityEngine;
public enum ProductType {
    MechanicalEyeProduct,
    RoboticArmProduct,
    IronHeartProduct,
    NeurochipProduct
}
namespace Assets._Game._Scripts._5_Managers {
    
    public class DataMode_ : MonoBehaviour {
        [SerializeField] public ResourceData ResData;
         public Dictionary<ProductType, BaseUpgradeSO> _dataForUpgradeDesktopsMap;
        [SerializeField] public IronHeartUpgradeSO _ironHeartUpgradeSO;
        [SerializeField] public MechanicalEyeUpgradeSO _mechanicalEyeUpgradeSo;
        [SerializeField] public NeurochipUpgradeSO _neurochipUpgradeSo;
        [SerializeField] public RoboticArmUpgradeSO _roboticArmUpgradeSO;
       
        [SerializeField] public GameObject _desktopPrefab;
       
        [SerializeField] public GameObject _prebuilderDesktopPrefab;

        [SerializeField] private GameMode _gameMode;
        [SerializeField] private UIMode _uiMode;

        
        public Dictionary<ProductType, BaseUpgradeSO> DataForUpgradeDesktopsMap => _dataForUpgradeDesktopsMap;
        public GameObject PrefabsForCreateDesktop => _desktopPrefab;
        public GameObject PreafabsForCreatePrebuilderDesktop => _prebuilderDesktopPrefab;

        private void Awake()
        {
            _dataForUpgradeDesktopsMap = new Dictionary<ProductType, BaseUpgradeSO>()
            {
                { ProductType.IronHeartProduct , _ironHeartUpgradeSO},
                { ProductType.MechanicalEyeProduct , _mechanicalEyeUpgradeSo},
                { ProductType.NeurochipProduct , _neurochipUpgradeSo},
                { ProductType.RoboticArmProduct , _roboticArmUpgradeSO},
            };
            
        }
        public void Construct(GameMode gameMode, UIMode uiMode) {
            _gameMode = gameMode;
            _uiMode = uiMode;
          
           

        }
        public Sprite GetIconByProductType(ProductType productType) {
            var productInfo = ResData.ProductsInfo.FirstOrDefault(p => p.ProductType.ToString() == productType.ToString());
            return productInfo != null ? productInfo.ProductIcon : ResData.BaseIcon;
        }

        // public MechanicalEyeUpgradeSO GetProductUpgradeSO(ProductType productType)
        // {
        //     
        //  
        //     return _mechanicalEyeUpgradeSo;
        // }

        public BaseUpgradeSO GetProductUpgradeSO(ProductType productType)
        {
            return DataForUpgradeDesktopsMap[productType];
        }

        public GameObject GetPrefabForPrebuilderDesktop()
        {
            return PreafabsForCreatePrebuilderDesktop;
        }

        public GameObject GetPrefabForDesktop()
        {
            return PrefabsForCreateDesktop;
        }
    }
}


