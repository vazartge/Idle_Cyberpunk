using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class UIDesktopView : UiUnitView {
        public Canvas Canvas {
            get => _canvas;
            set => _canvas = value;
        }

        [SerializeField] private DesktopViewModel _viewModel;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _textLevel;
        [SerializeField] private TMP_Text _typeProduct;
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Transform _starsContainer; // Родительский объект для звезд
        [SerializeField] private Image _indicator;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _textCost;
        [SerializeField] private Button _buyButton;
       
        private int maxStarsForCurrentLevel;


        private void Awake() {
            // Инициализируем все звезды серым цветом
            foreach (var star in _starsImages) {
                star.color = Color.gray;
            }
        }

        public void Construct(DesktopViewModel viewModel, int maxStars) {
            _viewModel = viewModel;
            maxStarsForCurrentLevel=maxStars;
            SetActiveStars(maxStarsForCurrentLevel);
        }
        // Метод для установки активности звезд
        private void SetActiveStars(int maxStarsCount) {
            for (int i = 0; i < _starsImages.Length; i++) {
                _starsImages[i].gameObject.SetActive(i < maxStarsCount);
            }
        }
        public void ShowWindow() {
            Canvas.gameObject.SetActive(true);
        }

        public override void HideWindow() {
            Canvas?.gameObject.SetActive(false);
        }

        public void OnButtonUpgrade() {
            _viewModel.OnButtonUpgrade();
        }

        public void UpdateOnChangeMoney(long сost, int level, long money, string productName
            , int incomeValue, int starsAmount, float progressStarIndicator, bool isButtonEnabled) {
             // Debug.Log($"Прогресс индикатора: {progressStarIndicator}");
             // Заполнение текста уровня
             _textLevel.text = level.ToString();
             // Заполнение текста продукта
             _typeProduct.text = productName;
             // Заполнение звезд
             UpdateStars(starsAmount);
            
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

             _buyButton.interactable = isButtonEnabled;// проверка достиг ли уровень прокачки стола предела для данного уровня игры
             // Заполнение рекламы
        }
        // Обновляет количество активных звезд
        public void UpdateStars(int starsAmount) {
            for (int i = 0; i < maxStarsForCurrentLevel; i++) {
                _starsImages[i].color = i < starsAmount ? Color.yellow : Color.gray;
            }
        }
        // Обновляет количество активных звезд
        // public void UpdateStars(int starsAmount) {
        //     for (int i = 0; i < _starsImages.Length; i++) {
        //         // Если индекс звезды меньше количества активных звезд, делаем её жёлтой
        //         if (i < starsAmount) {
        //             _starsImages[i].color = Color.yellow;
        //         } else {
        //             _starsImages[i].color = Color.gray; // Остальные звезды делаем серыми
        //         }
        //     }
        // }
    }
}


