using Assets._Game._Scripts._3_UI._UIUnits._Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Для работы с элементами UI

namespace Assets._Game._Scripts._6_Entities._Units._Customers {
    public class UiCustomerView : UiUnitView {
        [SerializeField] private Image _productImage; // Иконка товара
        [SerializeField] private TMP_Text _productQuantity; // Количество товара
        [SerializeField] private Image _quantityImage;
        [SerializeField] private Canvas _canvas;
        // private void Awake()
        // {
        //     _canvas = GetComponentInChildren<Canvas>();
        // }
        
        // Метод для обновления UI на основе данных
        public void UpdateCustomerUI(Sprite icon, int quantity) {
            
            if (icon == null || quantity < 1)
            {
                _canvas.gameObject.SetActive(false);
                return;
            }
            else
            {
                if (!_canvas.gameObject.activeSelf) _canvas.gameObject.SetActive(true);
            }
            _productImage.sprite = icon;

            if (quantity < 2)
            {
                _quantityImage.gameObject.SetActive(false);
                return;
            }
            else
            {
                if (quantity > 1 && !_quantityImage.gameObject.activeSelf)
                {
                    _quantityImage.gameObject.SetActive(true);
                    _productQuantity.text = quantity.ToString();
                }
                else
                {
                    _productQuantity.text = quantity.ToString();
                }
            }

            
        }
        private void OnDisable() {
            _canvas.gameObject.SetActive(false);
        }
    }
}