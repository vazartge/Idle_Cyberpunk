using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;

// Предполагаем, что здесь находится класс Customer

namespace Assets._Game._Scripts._6_Entities._Units._Customers {
    public class CustomerViewModel : UnitViewModel {
        private Customer _customerModel; // Модель данных покупателя
        private UiCustomerView _customerView; // View для отображения данных покупателя

        public CustomerViewModel(Customer customer, UiCustomerView view) {
            _customerModel = customer;
            _customerView = view;
            // Подписка на изменения в модели (например, на события изменения заказа)
            _customerModel.OnUIChanged += UpdateView;
        }

        private void OnEnable()
        {
            
        }
        // Метод для обновления View
        private void UpdateView() {
            Sprite icon;
            int quantity;
            if (_customerModel.Orders.Count != 0) {
                // Получаем данные из модели и обновляем View
                icon = Game.Instance.DataMode.GetIconByProductType(_customerModel.Orders[0]
                    .GetProductType()); // Предполагаем, что метод GetIconByProductType возвращает Sprite
                quantity = _customerModel.Orders.Count;


            }
            else
            {
                icon = null;
                quantity = 0;
            }
            _customerView.UpdateCustomerUI(icon, quantity);
        }

     
        // Дополнительные методы для работы с моделью и View...
    }
}