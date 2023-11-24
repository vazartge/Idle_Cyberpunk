using System.Collections;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Sellers
{
   
    public class Seller : UnitGame
    {
        private enum SellerState {
            SearchingForCustomerState,
            MovingToCustomerForOrderState,
            TakingOrderState,
            CheckCustomersWaitingSellersState,
            TakingNewJobState,
            MovingToDesktopState,
            CollectingOrderState,
            MovingToCustomerForDeliverState,
            DeliveredOrderState
        }
        public float TimeTakingOrder { get; } = 2f;
        // Параметр скорости движения продавца
        [SerializeField]
        private float _moveSpeed = 3f;
        private GameMode _gameMode;
        private Store _store;

        private Customer _currentCustomer;
        private SellerState _sellerState = SellerState.SearchingForCustomerState;
        private Order _currentOrder;
        private int _currentOrderItemIndex; // Индекс текущего товара в заказе
        private bool _isCustomerAvailable;
        private CustomerSlot _currentCustomerSlot;
        private SellerSlot _currenSellerSlot;


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
                        yield return StartCoroutine(WaitingForCustomerRoutine());
                        break;
                    case SellerState.MovingToCustomerForOrderState:
                        yield return StartCoroutine(MovingToCustomerForOrderRoutine());
                        break;
                    case SellerState.TakingOrderState:
                        yield return StartCoroutine(TakingOrderRoutine());
                        break;
                    case SellerState.CheckCustomersWaitingSellersState:
                        yield return StartCoroutine(CheckCustomersWaitingSellersRoutine());
                        break;
                    case SellerState.TakingNewJobState:
                        yield return StartCoroutine(TakingNewJobRoutine());
                        break;
                    case SellerState.MovingToDesktopState:
                        yield return StartCoroutine(MoveToDesktopRoutine());
                        break;
                    case SellerState.CollectingOrderState:
                        yield return StartCoroutine(CollectingOrderRoutine());
                        break;
                    case SellerState.MovingToCustomerForDeliverState:
                        yield return StartCoroutine(MoveToCustomerRoutineForDeliver());
                        break;
                    case SellerState.DeliveredOrderState:
                        yield return StartCoroutine(DelivedOrderRoutine());
                        break;
                }
            }
        }

        private IEnumerator WaitingForCustomerRoutine() {
            yield return new WaitUntil(() => _store.IsCustomerAvailable); // Ожидаем, пока флаг не станет true
            _currentCustomerSlot = _store.GetWaitingCustomerSlot();
            _currentCustomer = _currentCustomerSlot.Customer;
            _currenSellerSlot = _store.GetSellerSlotByCustomerSlot(_currentCustomerSlot);
            // Переключение на следующее состояние
            _sellerState = SellerState.MovingToCustomerForOrderState;
            Debug.Log("New Customer");
        }

        

        private IEnumerator MovingToCustomerForOrderRoutine() {

            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = _currenSellerSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(customerPosition, SellerState.MovingToCustomerForOrderState);
            _sellerState = SellerState.TakingOrderState;
        }

        private IEnumerator TakingOrderRoutine() {
            // Взятие заказа у покупателя
            // ...
            yield return new WaitForSeconds(TimeTakingOrder);
            _store.AddNewOrder(_currentCustomer.GetOrder());
        }
        private IEnumerator CheckCustomersWaitingSellersRoutine() {
            // Поиск ожидающих приема Заказа Покупателей 
            if (_store.IsCustomerAvailable)
            {
                _sellerState = SellerState.SearchingForCustomerState;
            }
            else
            {
                _sellerState = SellerState.TakingNewJobState;}


            yield return null;
        }

        private IEnumerator TakingNewJobRoutine()
        {
            //Получение задания из списка
            _currentOrder = _store.GetNewJobForSeller();

            yield return null;
        }
        private IEnumerator MoveToDesktopRoutine() {
            // Определите позицию свободного стола
            //Vector3 tablePosition = _store.GetFreeDesktopSlotPosition();

            // Запустите Dotween анимацию и ждите её завершения
            yield return null;//transform.DOMove(tablePosition, GetMoveDuration(tablePosition)).SetEase(Ease.Linear).WaitForCompletion();

            // После достижения стола продолжаем следующую корутину
            StartCoroutine(CollectingOrderRoutine());
        }

        private IEnumerator CollectingOrderRoutine() {
            // Сбор заказа
            // ...
            yield return null;
        }
        private IEnumerator MoveToCustomerRoutineForDeliver() {
            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = _currentCustomer.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return null;// transform.DOMove(customerPosition, GetMoveDuration(customerPosition)).SetEase(Ease.Linear).WaitForCompletion();

            // После достижения покупателя продолжаем следующую корутину
            StartCoroutine(TakingOrderRoutine());
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
            yield return transform.DOMove(target, duration).SetEase(Ease.Linear).WaitForCompletion();
            ReachedDestination(sender);
        }
        private void ReachedDestination(SellerState sender){
           
            // Действия после достижения цели
            Debug.Log("Достигнута точка назначения Продавцом");
            switch (sender)
            {
                case SellerState.SearchingForCustomerState:
                    break;
                case SellerState.MovingToCustomerForOrderState:
                    break;
                case SellerState.TakingOrderState:
                    break;
                case SellerState.CheckCustomersWaitingSellersState:
                    break;
                case SellerState.MovingToDesktopState:
                    break;
                case SellerState.CollectingOrderState:
                    break;
                case SellerState.MovingToCustomerForDeliverState:
                    break;
                case SellerState.DeliveredOrderState:
                    break;
                default:
                   Debug.Log("Нет вариантов при достижении цели при данном состоянии");
                    break;
            }
        }
    }
}