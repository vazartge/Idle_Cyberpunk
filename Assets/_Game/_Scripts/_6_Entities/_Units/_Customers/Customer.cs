using System;
using System.Collections.Generic;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Customers {


    public class Customer : BaseUnitGame, ICharacterUnitChangableSprites {
        private enum CustomerState {
            None,
            MovingToTradeState,
            WaitSellerForOrderingState,
            WaitProductState,
            MovingFromTradeState,
        }
       
        public event Action OnUIChanged;
        public UiCustomerView CustomerView;
        public CustomerViewModel CustomerViewModel;

        public int IDSprites;
        public CharacterType CharacterType;
        public AnimationState AnimationState;
        private GameMode _gameMode;
        private Store _store;
        public CustomerSlot CustomerSlot;
        public List<Order> Orders;
        private CustomerState _customerState;

        private float speed => Game.Instance.StoreStats.SpeedMoveCustomer; // Скорость перемещения в единицах в секунду
        private Transform _startPointTransform;
        private Transform _endPointTransform;

        public CharacterSpritesAndAnimationController characterSpritesAndAnimationController { get; set; }
      //  public GameMode GameMode => _gameMode;
        public void Awake()
        {
            CustomerView = GetComponentInChildren<UiCustomerView>();
            CustomerViewModel  = new CustomerViewModel(this, CustomerView);
            characterSpritesAndAnimationController = GetComponentInChildren<CharacterSpritesAndAnimationController>();
        }
        public void Construct(GameMode gameMode, Store store, Transform startPoint,
            Transform endPoint, CharacterType characterType, int idSprites) {
            _gameMode = gameMode;
            _store = store;
            _startPointTransform = startPoint;
            _endPointTransform = endPoint;

            CharacterType = characterType;
            IDSprites = idSprites;
            characterSpritesAndAnimationController.Construct(this, idSprites, characterType);
        }

        public void SetupCustomer(CustomerSlot freeSlot, List<Order> orders) {
            CustomerSlot = freeSlot;
            CustomerSlot.Customer = this;
            Orders = orders;
            
            UpdateStates(CustomerState.MovingToTradeState);
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_down);
        }

        private void UpdateStates(CustomerState state) {
            
            _customerState = state;
            switch (_customerState) {
                case CustomerState.None:

                    break;
                case CustomerState.MovingToTradeState:
                    MovingToTradeRoutine();
                    break;
                case CustomerState.WaitSellerForOrderingState:
                    break;
                case CustomerState.WaitProductState:
                    break;
                case CustomerState.MovingFromTradeState:
                    characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);
                    MovingFromTradeRoutine();
                    break;
                default:
                    Debug.Log("Нет такого варианта действий Покупателя");
                    break;
            }

        }
        private void MovingToTradeRoutine() {
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_down);
            //  Debug.Log($"{this.IDSprites} идет к прилавку");
            // Вычисляем промежуточную точку на одной линии с прилавком, но по горизонтали от покупателя
            Vector3 intermediatePoint = new Vector3(CustomerSlot.transform.position.x, transform.position.y, transform.position.z);

            // Сначала перемещаемся к промежуточной точке
            float horizontalDistance = Vector3.Distance(transform.position, intermediatePoint);
            float horizontalDuration = horizontalDistance / speed;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(intermediatePoint, horizontalDuration).SetEase(Ease.Linear));

            // Затем перемещаемся от промежуточной точки к прилавку
            float verticalDistance = Vector3.Distance(intermediatePoint, CustomerSlot.transform.position);
            float verticalDuration = verticalDistance / speed;
            sequence.Append(transform.DOMove(CustomerSlot.transform.position, verticalDuration).SetEase(Ease.Linear));

            // Добавляем обработчик, который вызовется по завершении всей анимации
            sequence.OnComplete(ReachedDestinationToTrade);
        }


        private void ReachedDestinationToTrade() {
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_down);
            UpdateStates(CustomerState.WaitSellerForOrderingState);
            _store.CustomerIsReachedStore(this, CustomerSlot);
            // Действия после достижения цели

          //  Debug.Log($"{this.IDSprites}.Достигнута точка назначения");
        }

        public void TransferOrder() {
            OnUIChanged?.Invoke();
            UpdateStates(CustomerState.WaitProductState);
            _store.CustomerTransferedOrder(this);
            OnUIChanged?.Invoke();
        }

        public void DeliveredProduct(Order order) {
            Orders.Remove(order);
            OnUIChanged?.Invoke();
            _store.CustomerTakeProduct(this);
            if (Orders.Count == 0) {
                UpdateStates(CustomerState.MovingFromTradeState);
            }


        }
        private void MovingFromTradeRoutine() {
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);

            CustomerSlot.Customer = null;
            _store.CustomerLeftSlot(this);
           
            // Вычисляем расстояние и продолжительность движения
            float distance = Vector3.Distance(transform.position, _endPointTransform.position);
            float duration = distance / speed;

            // Создаем последовательность анимации
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(_endPointTransform.position, duration).SetEase(Ease.Linear));

            // Добавляем обработчик, который вызовется по завершении всей анимации
            sequence.OnComplete(ReachedDestinationFromTrade);
        }


        private void ReachedDestinationFromTrade() {
            UpdateStates(CustomerState.None);
            _gameMode.CustomerLeftScene(this);
            // Действия после достижения цели

           // Debug.Log($"Покупатель {this.IDSprites}Достигнута точка назначения");
        }

        private void OnDestroy()
        {
            OnUIChanged = null;
        }
    }
}