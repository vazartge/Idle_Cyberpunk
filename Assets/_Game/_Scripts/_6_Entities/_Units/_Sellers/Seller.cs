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

        public float TimeTakingOrder { get; } = 1f;
        // Параметр скорости движения продавца
        [SerializeField]
        private float _moveSpeed = 6f;
        private GameMode _gameMode;
        private Store _store;


        private SellerState _sellerState = SellerState.SearchingForCustomerState;
        private Order _currentOrder;
        private int _currentOrderItemIndex; // Индекс текущего товара в заказе
        private bool _isCustomerAvailable;
        public Customer TargetCustomer;
        public CustomerSlot TargetCustomerSlot;
        public SellerSlot TargetSellerSlot;
        public SellerSlot CurrentSellerSlot;
        private DesktopSlot TargetDesktopSlot;
        private float _timeCollectingProduct = 1f;

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
                        yield return StartCoroutine(DeliveredOrderRoutine());
                        break;
                }
            }
        }

        private IEnumerator SearchingForCustomerRoutine() {
            while (true) {

                yield return new WaitUntil(() => _store.IsCustomerAvailable
                                                 || _store.IsDesktopAvailable); // Ожидаем, пока флаг не станет true
                if (_store.IsCustomerAvailable) {

                    TargetCustomer = _store.GetWaitingCustomer();
                    TargetCustomerSlot = TargetCustomer.CustomerSlot;
                    TargetSellerSlot = _store.GetSellerSlotByCustomerSlot(TargetCustomerSlot);
                    TargetSellerSlot.Seller = this;

                    // Переключение на следующее состояние
                    _sellerState = SellerState.MovingToCustomerForOrderState;
                    Debug.Log($"{this.ID} + Получил данные нового покупателя {TargetCustomer.ID}");

                    CurrentSellerSlot = TargetSellerSlot;

                    break;
                }

                if (_store.IsDesktopAvailable) {
                    (_currentOrder, TargetDesktopSlot) = _store.SellerTryGetNewJob();
                    if (_currentOrder != null) {

                        TargetDesktopSlot.Seller = this;
                        CurrentSellerSlot.Seller = null;
                        TargetCustomer = _currentOrder.Customer;
                        TargetCustomerSlot = TargetCustomer.CustomerSlot;
                        TargetSellerSlot = _store.GetSellerSlotByCustomerSlot(TargetCustomerSlot);
                        // Заказ найден, переключаемся на следующее состояние
                        _sellerState = SellerState.MovingToDesktopState;
                        Debug.Log("Заказ получен в работу");
                    } else {
                        _sellerState = SellerState.SearchingForCustomerState;
                    }

                    break;
                }
                
            }
        }

        private IEnumerator MovingToCustomerForOrderRoutine() {

            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = TargetSellerSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(customerPosition, SellerState.MovingToCustomerForOrderState);
            Debug.Log($"{this.ID} Продавец подошел к новому покупателю {TargetCustomer.ID} за заказом");
            _sellerState = SellerState.TakingOrderState;

        }

        private IEnumerator TakingOrderRoutine() {

            // Взятие заказа у покупателя
            Debug.Log($"{this.ID} Продавец берет заказ у нового покупателя {TargetCustomer.ID}");
            yield return new WaitForSeconds(TimeTakingOrder);
            TargetCustomer.TransferOrder();
            Debug.Log("Продавец заказ у нового покупателя в лист заказов");
            _sellerState = SellerState.SearchingForCustomerState;

        }

        private IEnumerator MovingToDesktopRoutine() {
            // Определите позицию свободного стола
            Vector3 tablePosition = TargetDesktopSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(tablePosition, SellerState.MovingToDesktopState);
            _sellerState = SellerState.CollectingOrderState;


        }

        private IEnumerator CollectingOrderRoutine() {
            // Сбор заказа
            Debug.Log("Собирает заказ за столом");
            yield return new WaitForSeconds(_timeCollectingProduct);
            Debug.Log("Заказ собран");
            TargetDesktopSlot.Seller = null;
            _sellerState = SellerState.MovingToCustomerForDeliverState;
        }
        private IEnumerator MovingToCustomerForDeliverRoutine() {
            //TargetSellerSlot = _store.GetSellerSlotByCustomerSlot(TargetCustomerSlot);

            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = TargetSellerSlot.transform.position;


            // Запустите Dotween анимацию и ждите её завершения
            yield return
                MoveToTargetWithSpeed(customerPosition,
                    SellerState
                        .MovingToCustomerForDeliverState); //null;// transform.DOMove(customerPosition, GetMoveDuration(customerPosition)).SetEase(Ease.Linear).WaitForCompletion();

            // После достижения покупателя продолжаем следующую корутину
            _sellerState = SellerState.DeliveredOrderState;
        }
        private IEnumerator DeliveredOrderRoutine() {
            // Доставка заказа покупателю
            TargetCustomer.DeliveredProduct(_currentOrder);
            _currentOrder = null;
            yield return null;
            Debug.Log("Товар доставлен покупателю");
            _sellerState = SellerState.SearchingForCustomerState; // Возвращаемся к поиску нового покупателя
            // Продавец возвращается в ожидающий слот (если такой используется)
            if (CurrentSellerSlot.Seller != this) CurrentSellerSlot.Seller = this;
        }

        private IEnumerator MoveToTargetWithSpeed(Vector3 target, SellerState sender) {
            Debug.Log($"{this.ID} Продавец начал движение");
            float distance = Vector3.Distance(transform.position, target);
            float duration = distance / _moveSpeed; // Рассчитываем продолжительность на основе скорости и расстояния
                                                    // Добавляем обработчик, который вызовется по завершении анимации
            if (Vector3.Distance(transform.position, target) > 0.1f) {
                yield return transform.DOMove(target, duration).SetEase(Ease.Linear).WaitForCompletion();
            }

        }

    }
}