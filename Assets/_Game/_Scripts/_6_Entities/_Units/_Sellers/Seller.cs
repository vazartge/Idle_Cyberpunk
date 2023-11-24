using System.Collections;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Sellers {

    public class Seller : UnitGame {
        private enum SellerState {
            SearchingForCustomerState,
            MovingToCustomerForOrderState,
            TakingOrderState,
            MovingToDesktopState,
            CollectingOrderState,
            MovingToCustomerForDeliverState,
            DeliveredOrderState
        }
        public float TimeTakingOrder { get; } = 2f;
        // Параметр скорости движения продавца
        [SerializeField]
        private float _moveSpeed = 6f;
        private GameMode _gameMode;
        private Store _store;

        private Customer _currentCustomer;
        private SellerState _sellerState = SellerState.SearchingForCustomerState;
        private Order _currentOrder;
        private int _currentOrderItemIndex; // Индекс текущего товара в заказе
        private bool _isCustomerAvailable;
        private CustomerSlot _currentCustomerSlot;
        private SellerSlot _currentSellerSlot;
        private DesktopSlot _desktopSlot;


        public void Construct(GameMode gameMode, Store store) {
            _gameMode = gameMode;
            _store = store;
            SetupSeller();
        }

        private void SetupSeller() {
            _sellerState = SellerState.SearchingForCustomerState;
            StartCoroutine(WorkRoutine());

        }



        private IEnumerator WorkRoutine() {
            while (true) {
                switch (_sellerState) {
                    case SellerState.SearchingForCustomerState:
                        yield return StartCoroutine(SearchingForCustomerRoutine());
                        break;
                    case SellerState.MovingToCustomerForOrderState:
                        yield return StartCoroutine(MovingToCustomerForOrderRoutine());
                        break;
                    case SellerState.TakingOrderState:
                        yield return StartCoroutine(TakingOrderRoutine());
                        break;
                   
                    case SellerState.MovingToDesktopState:
                        yield return StartCoroutine(MovingToDesktopRoutine());
                        break;
                    case SellerState.CollectingOrderState:
                        yield return StartCoroutine(CollectingOrderRoutine());
                        break;
                    case SellerState.MovingToCustomerForDeliverState:
                        yield return StartCoroutine(MovingToCustomerForDeliverRoutine());
                        break;
                    case SellerState.DeliveredOrderState:
                        yield return StartCoroutine(DelivedOrderRoutine());
                        break;
                }
            }
        }

        private IEnumerator SearchingForCustomerRoutine() {
            yield return new WaitUntil(() => _store.IsCustomerAvailable
                                             || _store.IsDesktopAvailable); // Ожидаем, пока флаг не станет true
            if (_store.IsCustomerAvailable)
            {
                
                _currentCustomerSlot = _store.GetWaitingCustomerSlot();
                _currentCustomer = _currentCustomerSlot.Customer;
                _currentSellerSlot = _store.GetSellerSlotByCustomerSlot(_currentCustomerSlot);
                _currentSellerSlot.Seller = this;
                // Переключение на следующее состояние
                _sellerState = SellerState.MovingToCustomerForOrderState;
                Debug.Log("New Customer");

            }

            if (_store.IsDesktopAvailable) {
                (_currentOrder, _desktopSlot) = _store.TryGetNewJobForSeller();
                if (_currentOrder != null) {
                    if (_currentOrder.TryTakeProduct())
                    {
                        _desktopSlot.Seller = this;
                        _currentSellerSlot.Seller = null;
                        _store.AvailableDesktopSlots();
                        // Заказ найден, переключаемся на следующее состояние
                        _sellerState = SellerState.MovingToDesktopState;
                        Debug.Log("Заказ получен в работу");
                    }
                    else
                    {
                        _sellerState = SellerState.SearchingForCustomerState;
                    }
                   
                }
            }

        }

        private IEnumerator MovingToCustomerForOrderRoutine() {

            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = _currentSellerSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(customerPosition, SellerState.MovingToCustomerForOrderState);
            Debug.Log("Продавец подошел к новому покупателю за заказом");
            _sellerState = SellerState.TakingOrderState;
        }

        private IEnumerator TakingOrderRoutine() {
            // Взятие заказа у покупателя
            Debug.Log("Продавец берет заказ у нового покупателя");
            yield return new WaitForSeconds(TimeTakingOrder);
            _store.AddNewOrder(_currentCustomer.TransferOrder());
            Debug.Log("Продавец заказ у нового покупателя в лист заказов");
            _sellerState = SellerState.SearchingForCustomerState;

        }
       
        private IEnumerator MovingToDesktopRoutine() {
            // Определите позицию свободного стола
            Vector3 tablePosition = _desktopSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(tablePosition, SellerState.MovingToDesktopState);
            _sellerState = SellerState.CollectingOrderState;


        }

        private IEnumerator CollectingOrderRoutine() {
            // Сбор заказа
            Debug.Log("Собирает заказ за столом");
            yield return new WaitForSeconds(2f);
        }
        private IEnumerator MovingToCustomerForDeliverRoutine() {
            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = _currentCustomer.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return null;// transform.DOMove(customerPosition, GetMoveDuration(customerPosition)).SetEase(Ease.Linear).WaitForCompletion();

            // После достижения покупателя продолжаем следующую корутину
            
        }
        private IEnumerator DelivedOrderRoutine() {
            // Доставка заказа покупателю
            // ...
            yield return new WaitForSeconds(1f);
            _sellerState = SellerState.SearchingForCustomerState; // Возвращаемся к поиску нового покупателя
        }

        private IEnumerator MoveToTargetWithSpeed(Vector3 target, SellerState sender) {
            Debug.Log("Продавец начал движение");
            float distance = Vector3.Distance(transform.position, target);
            float duration = distance / _moveSpeed; // Рассчитываем продолжительность на основе скорости и расстояния
            // Добавляем обработчик, который вызовется по завершении анимации
            if (Vector3.Distance(transform.position, target) > 0.3f) {
                yield return transform.DOMove(target, duration).SetEase(Ease.Linear).WaitForCompletion();
            }

        }
       
    }
}