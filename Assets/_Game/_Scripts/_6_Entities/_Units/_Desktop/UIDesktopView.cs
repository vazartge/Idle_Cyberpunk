using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class UIDesktopView : UiUnitView {
        public Canvas Canvas
        {
            get => _canvas;
            set => _canvas = value;
        }

        [SerializeField] private UIDesktopViewModel _viewModel;

        [SerializeField] private Canvas _canvas;

        [SerializeField] private TMP_Text _textLevel;

        [SerializeField] private TMP_Text _typeProduct;

        [SerializeField] private Image[] _starsImages;

        [SerializeField] private Image _indicator;

        [SerializeField] private TMP_Text _incomeText;

        [SerializeField] private TMP_Text _textCost;
        [SerializeField] private Button _buyButton;

        public void Construct(UIDesktopViewModel viewModel) {
            _viewModel = viewModel;
        }
        public void ShowWindow() {
            Canvas.gameObject .SetActive(true);
        }
        public void HideWindow() {
            Canvas.gameObject.SetActive(false);
        }

        public void OnButtonUpgrade() {

            _viewModel.OnButtonUpgrade();
        }

        public void UpdateOnChangeMoney(long сost, int level, long money, string productName, int incomeValue, int starsAmount, float progressStarIndicator) {
            Debug.Log($"Прогресс индикатора: {progressStarIndicator}");
            // Заполнение текста уровня
            _textLevel.text = level.ToString();
            // Заполнение текста продукта
            _typeProduct.text = productName;
            // Заполнение звезд
            foreach (var starsImage in _starsImages)
            {
                starsImage.color = Color.gray;
            }
            for (int i =0; i< starsAmount; i++)
            {
                _starsImages[i].color = Color.yellow;
            }

            /* Заполение индикатора прогресса открытия звезд - надо сделать формулу поискаколичества уровней
              до следующего изменения уровня (общее количество уровней для данной прокачки), разделить 
              текущее количество уровней на найденное общее*/
            _indicator.fillAmount = progressStarIndicator;
            // Заполнение доходности
            _incomeText.text = NumberFormatterService.FormatNumber(incomeValue);
            // Заполнение цены на кнопке
            _textCost.text = NumberFormatterService.FormatNumber(сost);
            if (сost > money) {
                _textCost.color = Color.red;
                _buyButton.interactable = false;
            } else {
                _textCost.color = Color.black;
                _buyButton.interactable = true;
            }
            // Заполнение рекламы
        }



    }
}