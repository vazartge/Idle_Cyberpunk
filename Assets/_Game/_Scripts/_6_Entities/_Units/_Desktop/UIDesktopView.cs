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

        [SerializeField] private UIDesktopViewModel _viewModel;
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

        public void Construct(UIDesktopViewModel viewModel, int maxStars) {
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

        public void HideWindow() {
            Canvas.gameObject.SetActive(false);
        }

        public void OnButtonUpgrade() {
            _viewModel.OnButtonUpgrade();
        }

        public void UpdateOnChangeMoney(long сost, int level, long money, string productName
             , int incomeValue, int starsAmount, float progressStarIndicator) {
             // Debug.Log($"Прогресс индикатора: {progressStarIndicator}");
             // Заполнение текста уровня
             _textLevel.text = (level+1).ToString();
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


// using Assets._Game._Scripts._3_UI._UIUnits._Base;
// using Assets._Game._Scripts._4_Services;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
//     public class UIDesktopView : UiUnitView {
//         public Canvas Canvas {
//             get => _canvas;
//             set => _canvas = value;
//         }
//
//         [SerializeField] private UIDesktopViewModel _viewModel;
//
//         [SerializeField] private Canvas _canvas;
//
//         [SerializeField] private TMP_Text _textLevel;
//
//         [SerializeField] private TMP_Text _typeProduct;
//
//         [SerializeField] private Image[] _starsImages = new Image[5];
//
//         [SerializeField] private Transform _starsContainer;//Родительский объект для звезд
//
//         [SerializeField] private Image _indicator;
//
//         [SerializeField] private TMP_Text _incomeText;
//
//         [SerializeField] private TMP_Text _textCost;
//         [SerializeField] private Button _buyButton;
//         private GameObject _starPrefab;
//         private Image[] starInstances; // Массив для хранения инстансов звезд
//
//         public void Construct(UIDesktopViewModel viewModel, int maxStars, GameObject starPrefab) {
//             _viewModel = viewModel;
//             _starPrefab = starPrefab;
//
//             InitializeStars(maxStars);
//         }
//         // Этот метод будет вызван один раз для инициализации всех звезд
//         public void InitializeStars(int maxStarsCount) {
//             SetStars(maxStarsCount);
//
//             _starsImages = new Image[maxStarsCount]; // Пересоздаем массив для звезд
//
//             // Создаем звезды и добавляем их в UI
//             for (int i = 0; i < maxStarsCount; i++) {
//                 var starObj = Instantiate(_starPrefab, _starsContainer);
//                 _starsImages[i] = starObj.GetComponent<Image>();
//                 _starsImages[i].color = Color.gray; // Устанавливаем начальный цвет звезд
//             }
//         }
//         // Метод для обновления видимости звезд
//         public void SetStars(int numberOfActiveStars) {
//             for (int i = 0; i < _starsImages.Length; i++) {
//                 if (i < numberOfActiveStars) {
//                     _starsImages[i].gameObject.SetActive(true); // Активируем нужное количество звезд
//                 } else {
//                     _starsImages[i].gameObject.SetActive(false); // Остальные делаем неактивными
//                 }
//             }
//         }
//         public void ShowWindow() {
//             Canvas.gameObject.SetActive(true);
//         }
//         public void HideWindow() {
//             Canvas.gameObject.SetActive(false);
//         }
//
//         public void OnButtonUpgrade() {
//
//             _viewModel.OnButtonUpgrade();
//         }
//
//         public void UpdateOnChangeMoney(long сost, int level, long money, string productName
//             , int incomeValue, int starsAmount, float progressStarIndicator) {
//             // Debug.Log($"Прогресс индикатора: {progressStarIndicator}");
//             // Заполнение текста уровня
//             _textLevel.text = (level+1).ToString();
//             // Заполнение текста продукта
//             _typeProduct.text = productName;
//             // Заполнение звезд
//             UpdateStars(starsAmount);
//             // foreach (var starsImage in _starsImages) {
//             //     starsImage.color = Color.gray;
//             // }
//             // for (int i = 0; i< starsAmount; i++) {
//             //     _starsImages[i].color = Color.yellow;
//             // }
//
//             /* Заполение индикатора прогресса открытия звезд - надо сделать формулу поискаколичества уровней
//               до следующего изменения уровня (общее количество уровней для данной прокачки), разделить 
//               текущее количество уровней на найденное общее*/
//             _indicator.fillAmount = progressStarIndicator;
//             // Заполнение доходности
//             _incomeText.text = NumberFormatterService.FormatNumber(incomeValue);
//             // Заполнение цены на кнопке
//             _textCost.text = NumberFormatterService.FormatNumber(сost);
//             if (сost > money) {
//                 _textCost.color = Color.red;
//                 _buyButton.interactable = false;
//             } else {
//                 _textCost.color = Color.black;
//                 _buyButton.interactable = true;
//             }
//
//             // Заполнение рекламы
//         }
//         // Вызывай этот метод при обновлении данных о столе
//         public void UpdateStars(int starsAmount) {
//             // Обновляем цвет активных звезд
//             for (int i = 0; i < _starsImages.Length; i++) {
//                 _starsImages[i].color = i < starsAmount ? Color.yellow : Color.gray;
//             }
//         }
//
//
//     }
// }