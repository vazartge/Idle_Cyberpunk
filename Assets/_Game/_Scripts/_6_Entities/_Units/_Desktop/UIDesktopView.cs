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
        [SerializeField] private DesktopStarUI[] _starScripList;
        [SerializeField] private Transform _starsContainer; // Родительский объект для звезд
        [SerializeField] private Image _indicator;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _textCost;
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject RewardButton;
       
        private int maxStarsForCurrentLevel;
        // Создание цвета
        Color activeColor = new Color(0.761f, 0.957f, 0.980f, 1.0f);

       
        public void Construct(DesktopViewModel viewModel, int maxStars) {
            _viewModel = viewModel;
            maxStarsForCurrentLevel=maxStars;
            SetActiveStars(maxStarsForCurrentLevel);
        }
        // Метод для установки активности звезд
        private void SetActiveStars(int maxStarsCount) {
            // Убедитесь, что maxStarsCount не больше длины массива _starsImages
            maxStarsCount = Mathf.Min(maxStarsCount, _starsImages.Length);

            for (int i = 0; i < _starsImages.Length; i++) {
                _starsImages[i].gameObject.SetActive(i < maxStarsCount);
            }

            InitStarsActiveList();
        }

        private void InitStarsActiveList() {
            // Инициализация _starScripList с той же длиной, что и активные _starsImages
            _starScripList = new DesktopStarUI[maxStarsForCurrentLevel];

            // Заполнение _starScripList и деактивация звезд
            for (int i = 0; i < maxStarsForCurrentLevel; i++) {
                DesktopStarUI starScript = _starsImages[i].GetComponent<DesktopStarUI>();
                if (starScript != null) {
                    _starScripList[i] = starScript;
                    starScript.DeactivateStar(); // Деактивировать звезду
                }
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
                 _textCost.color =activeColor;
                 _buyButton.interactable = true;
             }

             _buyButton.interactable = isButtonEnabled;// проверка достиг ли уровень прокачки стола предела для данного уровня игры
             // Заполнение рекламы
             if (_viewModel._desktop.IsUpgradedForLevel)
             {
                 if (RewardButton.activeSelf)
                 {
                     RewardButton.SetActive(false);
                 }
             }
        }
        // Обновляет количество активных звезд
        public void UpdateStars(int starsAmount) {
            for (int i = 0; i < _starScripList.Length; i++) {
                if (i < starsAmount) {
                    _starScripList[i].ActivateStar();
                } else {
                    _starScripList[i].DeactivateStar();
                }
            }
        }

        public void OnRewardedButton()
        {
            _viewModel._desktop.GameMode.OnRewardedButton(_viewModel._desktop);
        }
    }
}


