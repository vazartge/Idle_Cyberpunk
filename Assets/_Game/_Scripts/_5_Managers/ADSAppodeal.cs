using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using Assets._Game._Scripts._2_Game;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public class ADSAppodeal : MonoBehaviour
    {
        public static ADSAppodeal Instance;
        private bool initComplete;

        private void Awake()
        {
            if (Instance != null)
            {
                if (Instance != this)
                {
                    Destroy(this.gameObject);
                }

                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
           
        }

        public void Start()
        {
            InitAppodeal();
        }
        public void InitAppodeal() {
            int adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner | AppodealAdType.RewardedVideo |
                          AppodealAdType.Mrec;
            string appKey = "e71ccb4211e751afdddac7bf22d1a3108c9c21d9b6d075ba";
            AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
            Appodeal.Initialize(appKey, adTypes);
            
        
        
        }
        
        #region OnInitializedFinished
        
        public void OnInitializationFinished(object sender, SdkInitializedEventArgs e) {
            initComplete = true;
           // Game.Instance.AppodealInitialized(this);
            Appodeal.Cache(AppodealAdType.Interstitial); //кэширование межстраничной рекламы -надо ли?
            Appodeal.Cache(AppodealAdType.RewardedVideo); //кэширование ревард рекламы -надо ли?
            Appodeal.Cache(AppodealAdType.Banner);
        
            AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
            AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
            AppodealCallbacks.Interstitial.OnShown += OnInterstitialShown;
            AppodealCallbacks.Interstitial.OnShowFailed += OnInterstitialShowFailed;
            AppodealCallbacks.Interstitial.OnClosed += OnInterstitialClosed;
            AppodealCallbacks.Interstitial.OnClicked += OnInterstitialClicked;
            AppodealCallbacks.Interstitial.OnExpired += OnInterstitialExpired;
        
            AppodealCallbacks.RewardedVideo.OnLoaded += OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad += OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShown += OnRewardedVideoShown;
            AppodealCallbacks.RewardedVideo.OnShowFailed += OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnClicked += OnRewardedVideoClicked;
            AppodealCallbacks.RewardedVideo.OnExpired += OnRewardedVideoExpired;
        
            AppodealCallbacks.Banner.OnLoaded += OnBannerLoaded;
            AppodealCallbacks.Banner.OnFailedToLoad += OnBannerFailedToLoad;
            AppodealCallbacks.Banner.OnShown += OnBannerShown;
            AppodealCallbacks.Banner.OnShowFailed += OnBannerShowFailed;
            AppodealCallbacks.Banner.OnClicked += OnBannerClicked;
            AppodealCallbacks.Banner.OnExpired += OnBannerExpired;


            Game.Instance.AppodealInitialized();
        }
        
        #endregion
        
        #region InterstitialAd Callbacks
        
        // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
        private void OnInterstitialLoaded(object sender, AdLoadedEventArgs e) {
            Debug.Log("Interstitial loaded");
        }
        
        // Called when interstitial failed to load
        private void OnInterstitialFailedToLoad(object sender, EventArgs e) {
            Debug.Log("Interstitial failed to load");
        }
        
        // Called when interstitial was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        private void OnInterstitialShowFailed(object sender, EventArgs e) {
            Debug.Log("Interstitial show failed");
        }
        
        // Called when interstitial is shown
        private void OnInterstitialShown(object sender, EventArgs e) {
            Debug.Log("Interstitial shown");
            PauseGame();
        }
        
        // Called when interstitial is closed
        private void OnInterstitialClosed(object sender, EventArgs e) {
            Debug.Log("Interstitial closed");
            UnPauseGame();
        }
        
        // Called when interstitial is clicked
        private void OnInterstitialClicked(object sender, EventArgs e) {
            Debug.Log("Interstitial clicked");
        }
        
        // Called when interstitial is expired and can not be shown
        private void OnInterstitialExpired(object sender, EventArgs e) {
            Debug.Log("Interstitial expired");
        }
        
        #endregion
        
        #region RewardedVideoAd Callbacks
        
        //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
        private void OnRewardedVideoLoaded(object sender, AdLoadedEventArgs e) {
        
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoLoaded(bool isPrecache:{e.IsPrecache})");
        }
        
        // Called when rewarded video failed to load
        private void OnRewardedVideoFailedToLoad(object sender, EventArgs e) {
            Game.Instance.ErrorLoadRewardedVideo();
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoFailedToLoad()");
        }
        
        // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        private void OnRewardedVideoShowFailed(object sender, EventArgs e) {
            
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShowFailed()");
            OnFailedReward();
        }
        
        // Called when rewarded video is shown
        private void OnRewardedVideoShown(object sender, EventArgs e) {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShown()");
            PauseGame();
            
            
        }

       
        // Called when rewarded video is closed
        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e) {
            //GD.ads.CloseRewardedVideo();
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoClosed(bool finished:{e.Finished})");
            UnPauseGame();
            

        }
        
        // Called when rewarded video is viewed until the end
        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e) {
            
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoFinished(double amount:{e.Amount}, string name:{e.Currency})");
            UnPauseGame();
            CallGameRewardSucces();

        }

        private void CallGameRewardSucces()
        {
            Game.Instance.OnSuccesRewarded();
        }

        // Called when rewarded video is clicked
        private void OnRewardedVideoClicked(object sender, EventArgs e) {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoClicked()");
            
        }
        
        //Called when rewarded video is expired and can not be shown
        private void OnRewardedVideoExpired(object sender, EventArgs e) {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoExpired()");
            OnFailedReward();
        }
        
        #endregion
        
        #region BannerAd Callbacks
        
        // Called when a banner is loaded (height arg shows banner's height, precache arg shows if the loaded ad is precache
        private void OnBannerLoaded(object sender, BannerLoadedEventArgs e) {
            Debug.Log("Banner loaded");
        }
        
        // Called when banner failed to load
        private void OnBannerFailedToLoad(object sender, EventArgs e) {
            Debug.Log("Banner failed to load");
        }
        
        // Called when banner failed to show
        private void OnBannerShowFailed(object sender, EventArgs e) {
            Debug.Log("Banner show failed");
        }
        
        // Called when banner is shown
        private void OnBannerShown(object sender, EventArgs e) {
            Debug.Log("Banner shown");
        }
        
        // Called when banner is clicked
        private void OnBannerClicked(object sender, EventArgs e) {
            Debug.Log("Banner clicked");
        }
        
        // Called when banner is expired and can not be shown
        private void OnBannerExpired(object sender, EventArgs e) {
            Debug.Log("Banner expired");
        }
        
        #endregion
        
        public void ShowInterstitialADS() {
            if (Appodeal.IsLoaded(AppodealAdType.Interstitial)) {
                Appodeal.Show(AppodealShowStyle.Interstitial);
            }
        }
        // пример вызова interstitial ads
        //Appodeal.Show(AppodealShowStyle.Interstitial);
        
        public void ShowRewardedAds() {
            if (CheckReadyToShowRewardedAds()) {
                Appodeal.Show(AppodealShowStyle.RewardedVideo);

            }
        }
        
        // Пример вызова rewarded ads
        // Appodeal.Show(AppodealShowStyle.RewardedVideo);
        public bool CheckReadyToShowRewardedAds() {
            return Appodeal.IsLoaded(AppodealAdType.RewardedVideo);
        }
        
        public void ShowBanner() {
          //  if (!Appodeal.IsLoaded(AppodealAdType.Banner)) return;
            // ПРИМЕРЫ ОСТАВИТЬ ТОЛЬКО ОДИН
            // Display banner at the bottom of the screen
            Appodeal.Show(AppodealShowStyle.BannerBottom);
        
            // Display banner at the top of the screen
            // Appodeal.Show(AppodealShowStyle.BannerTop);
            //
            // // Display banner at the left of the screen
            // Appodeal.Show(AppodealShowStyle.BannerLeft);
            //
            // // Display banner at the right of the screen
            // Appodeal.Show(AppodealShowStyle.BannerRight);
        }
        
        public void HideBanners() {
            Appodeal.Hide(AppodealAdType.Banner);
            
        }

        public void DestroyBanner()
        {
            Appodeal.Destroy(AppodealAdType.Banner);
        }
        
        
        public bool CheckInit() {
            return initComplete;
        }
        private void PauseGame() {

            Game.Instance.PauseGame();
        }
        private void UnPauseGame() {

            Game.Instance.UnPauseGame();
        }

       

        public void OnFailedReward() {
           Game.Instance.OnFailedReward();
        }

    }
}
