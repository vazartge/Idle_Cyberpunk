using Assets._Game._Scripts._3_UI._UIUnits._Base;
using UnityEngine;
using UnityEngine.UI;


namespace Assets._Game._Scripts._3_UI._UIUnits
{
    public class UiSellerView : UiUnitView
    {
        [SerializeField] private Image _productImage;
        [SerializeField] private Image _progressImage;
        [SerializeField] private Canvas _canvas;

        private void OnEnable() {
            _canvas.gameObject.SetActive(true);
        }
        public void UpdateSellerShowIconUI(Sprite icon)
        {
            if (icon == null)
            {
                _productImage.gameObject.SetActive(false);
                return;
            }
            else
            {
                if (!_productImage.gameObject.activeSelf) _productImage.gameObject.SetActive(true);
            }
            _productImage.sprite = icon;
        }
        public void UpdateSellerHideIconUI() {
            _productImage.gameObject.SetActive(false);
        }
        public void UpdateSellerProgressUI(float progress, bool isShow)
        {
            if (isShow)
            {
                if (!_progressImage.gameObject.activeSelf)
                {
                    _progressImage.gameObject.SetActive(true);
                }
                if (_progressImage != null) {
                    _progressImage.fillAmount = progress;
                } else {
                    Debug.LogWarning("ProgressBar is not set on the UI Seller");
                }
            }
            else
            {
                _progressImage.gameObject.SetActive(false);
            }
           

        }
       
        private void OnDisable() {
            _canvas.gameObject.SetActive(false);
        }

      
    }
}