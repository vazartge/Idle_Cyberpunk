using Assets._Game._Scripts._2_Game;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Assets._Game._Scripts._5_Managers
{
    public class IAPManager : MonoBehaviour, IStoreListener {
        public static IAPManager Instance;
        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;

        public static string PRODUCT_DISABLE_AD = "com.disablead";
        public static string PRODUCT_INCREASE_PROFIT = "com.increaseprofit";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        void Start() {
            if (m_StoreController == null) {
                InitializePurchasing();
            }

          //  RestorePurchases(); // Вызов метода восстановления покупок при запуске

        }

        public void InitializePurchasing() {
            if (IsInitialized()) {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(PRODUCT_DISABLE_AD, ProductType.NonConsumable);
            builder.AddProduct(PRODUCT_INCREASE_PROFIT, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized() {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuyDisableADS() {
            BuyProductID(PRODUCT_DISABLE_AD);
        }

        public void BuyIncreaseProfit() {
            BuyProductID(PRODUCT_INCREASE_PROFIT);
        }

        void BuyProductID(string productId) {
            if (IsInitialized()) {
                Product product = m_StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase) {
                    m_StoreController.InitiatePurchase(product);
                } else {
                    Debug.Log("BuyProductID: FAIL. Not purchasing productStore, either is not found or is not available for purchase");
                }
            } else {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
            Game.Instance.OnIAPInitialized(); // Уведомление Game, что IAP инициализирован
        }

        public void OnInitializeFailed(InitializationFailureReason error) {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
            if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_DISABLE_AD, StringComparison.Ordinal)) {
                Debug.Log("Purchase Successful: " + args.purchasedProduct.definition.id);
                Game.Instance.StoreStats.PurchasedDisabledAds = true;
                // Сохранение состояния после восстановления покупок
                Game.Instance.OnSaveGameButton();
                // Любая дополнительная логика, связанная с отключением рекламы
            } else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_INCREASE_PROFIT, StringComparison.Ordinal)) {
                Debug.Log("Purchase Successful: " + args.purchasedProduct.definition.id);
                Game.Instance.StoreStats.PurchasedIncreaseProfit = true;
                // Сохранение состояния после восстановления покупок
                Game.Instance.OnSaveGameButton();
                // Любая дополнительная логика, связанная с увеличением прибыли
            } else {
                Debug.Log("Purchase Failed: " + args.purchasedProduct.definition.id);
            }

           

            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
        public void RestorePurchases() {
            if (!IsInitialized()) {
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.Android) {
                var googlePlayStoreExtensions = m_StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>();
                googlePlayStoreExtensions.RestoreTransactions((result) => {
                    // Этот колбэк будет вызван после попытки восстановления транзакций.
                    if (result) {
                        Debug.Log("RestorePurchases: Transactions restored.");
                        // Здесь можно добавить дополнительную логику после успешного восстановления покупок.
                    } else {
                        Debug.Log("RestorePurchases: Restore failed or no purchases to restore.");
                        // Здесь можно обработать ошибку восстановления.
                    }
                    // Также может быть полезно уведомить объект Game о результате восстановления.
                    Game.Instance.OnPurchasesRestored(result);
                });
            } else {
                Game.Instance.OnPurchasesRestored(false);
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


    }
}
