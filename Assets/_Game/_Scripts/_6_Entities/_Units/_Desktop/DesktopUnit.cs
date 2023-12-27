using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {

    public class DesktopUnit : DesktopBaseUnitBase {
        [SerializeField] public Vector3 Position;
        [SerializeField] public float RotationAngleZ;
        [SerializeField] public DesktopSlot DesktopSlot1;
        [SerializeField] public DesktopSlot DesktopSlot2;
        [SerializeField] public GameObject ContainerForRotateGO;
        [SerializeField] public GameObject AvailabilityIndicatorMainGO;
        [SerializeField] public GameObject AvailabilityIndicatorAdditionalGO;
        [SerializeField] public GameObject AdditionalDesktopGO;
        [SerializeField] public GameObject RewardedButtonGO;

        [SerializeField] public Transform AdditionalDesktopPointTransform;
        [SerializeField] public Transform UIPointTransformMain;
        [SerializeField] public Transform UIPointTransformAdditional;
        [SerializeField] public Transform UICanvasTransform;

        [SerializeField] public bool IsAdditionalDesktop;
        [SerializeField] public int Level = 1;
        [SerializeField] public ProductStoreType ProductStoreType;
        [SerializeField] public SpriteRenderer[] SpriteIconProductTypes;
        [SerializeField] public EconomyAndUpgradeService EconomyAndUpgrade;
        [SerializeField] public GameMode GameMode;
        // public Order Order;
        [SerializeField] public DesktopViewModel ViewModel;
        [SerializeField] public UIDesktopView View;
        [SerializeField] public long Cost;
        [SerializeField] public bool IsUpgradedForLevel;
        private readonly string DesktopTooltip5LevelsAdd = "DesktopTooltip5LevelsAdd";


        public void ConstructMain(GameMode gameMode,
            Vector3 position,
            float rotationAngleZ,
            ProductStoreType productStoreType,
            int upgradeLevel,
            bool isAdditionalDesktop,
            bool isUpgradedForLevel) {

            GameMode = gameMode;
            View = GetComponentInChildren<UIDesktopView>();
            ViewModel = new DesktopViewModel(this, View);
            EconomyAndUpgrade = GameMode.EconomyAndUpgrade;

            Position = position;
            RotationAngleZ = rotationAngleZ;
            ProductStoreType = productStoreType;
            Level = upgradeLevel;
            IsAdditionalDesktop = isAdditionalDesktop;
            IsUpgradedForLevel = isUpgradedForLevel;
            gameObject.transform.position = Position;
            ContainerForRotateGO.transform.rotation =  Quaternion.Euler(0f, 0f, RotationAngleZ);
            ProductStoreType = productStoreType;
            SetupDesktopInStore();
            SetupSlotInStore(DesktopSlot1);
            foreach (var icon in SpriteIconProductTypes) {
                if (icon != null && icon.gameObject.activeSelf) {
                    icon.sprite = GameMode.DataMode.GetIconByProductType(ProductStoreType);
                }
            }
            if (IsAdditionalDesktop) {
                SetupAdditionalDesktop();

            }
            GameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;
            UpdateViewAvailabilityIndicator();
        }

        private void SetupDesktopInStore() {
            GameMode.Store.AddDesktop(this);
        }
        private void SetupSlotInStore(DesktopSlot slot) {
            slot.gameObject.SetActive(true);
            slot.ProductStoreType = ProductStoreType;
            GameMode.Store.AddDesktopSlots(slot);



            // Корутина для активации покупателей с задержкой
        }
        public void SetupAdditionalDesktop() {
            AdditionalDesktopGO.transform.position = AdditionalDesktopPointTransform.position;
            AdditionalDesktopGO.SetActive(true);
            SetupSlotInStore(DesktopSlot2);

        }
        private void UpdateViewAvailabilityIndicator() {

            bool res = GameMode.Coins >= GameMode.DataMode
                .GetProductUpgradeSO(ProductStoreType).Upgrades[Level].Cost && !IsUpgradedForLevel;
            AvailabilityIndicatorMainGO.SetActive(res);
            if (IsAdditionalDesktop) {
                AvailabilityIndicatorAdditionalGO.SetActive(res);
            }
        }

        public void UpdateOnChangeStatsOrMoney() {

            SetCost();
            UpdateViewAvailabilityIndicator();
            ViewModel.UpdateOnChangeMoney();

        }


        public void OnButtonUpgradeDesktop() {

            var isSuccess = GameMode.OnButtonUpgradeDesktop(this);
            //UpdateOnChangeStatsOrMoney();
        }

        public void UpgradeLevelUp(int levelsToUpgrade) {
            for (int i = 0; i < levelsToUpgrade; i++) {
                Level++;
                //Level++;
                // Находим данные об уровне прокачки стола, соответствующего его текущему уровню
                var upgradeData = GameMode.DataMode.DataForUpgradeDesktopsMap[ProductStoreType];

                // Находим данные об уровне прокачки стола, соответствующего его текущему уровню
                var currentUpgradeData = upgradeData.Upgrades[Level];
                GameMode.EconomyAndUpgrade.CheckDesktopAfterUpgrade(this);
                // Проверяем, существуют ли данные для данного уровня и не превышает ли уровень игры OpeningAtLevel
                if (currentUpgradeData != null && GameMode.GameLevel < currentUpgradeData.OpeningAtLevel) {
                    IsUpgradedForLevel = true;
                    RewardedButtonGO.SetActive(false);
                    break;
                }


                Debug.Log($"IsUpgradedForLevel == {IsUpgradedForLevel}");
            }

           
            UpdateOnChangeStatsOrMoney();
        }



        private void SetCost() {
            Cost = EconomyAndUpgrade.SetCostBuyProductAndLevel(Level, ProductStoreType);

        }

        public void OnClickDesktop1() {
            if (IsAdditionalDesktop) {
                UICanvasTransform.transform.position = UIPointTransformAdditional.position;
            } else {
                UICanvasTransform.transform.position = UIPointTransformMain.position;
            }

            ViewModel.ShowWindow();
        }

        public void OnClickDesktop2() {
            UICanvasTransform.transform.position = UIPointTransformAdditional.position;
            ViewModel.ShowWindow();
        }

        public void AddAdditionalDesktop() {
            IsAdditionalDesktop = true;
        }

        public void OnRewardedClickButton() {
            Game.Instance.OnRewardedButtonFor5LevelsUpgrade(this);
        }


    }
}