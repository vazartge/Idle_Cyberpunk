using System;
using System.Collections;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Sellers {
    
    public class Seller : BaseUnitGame, ICharacterUnitChangableSprites {
        private enum SellerState {
            SearchingForCustomerState,
            MovingToCustomerForOrderState,
            TakingOrderState,
            MovingToDesktopState,
            CollectingOrderState,
            MovingToCustomerForDeliverState,
            DeliveredOrderState
        }
        public event Action<float, bool> OnUIChangedProgress;
        public event Action OnUIChangedShowProduct;
        public event Action OnUIChangedHideProduct;
        public UiSellerView SellerView;
        public SellerViewModel SellerViewModel;

        public int IDSprites;
        public CharacterType CharacterType;
        public AnimationState AnimationState;
        private Animator _animator;
        private float timeTakingOrder => _store.Stats.TakingOrder;
        private float productionSpeed => _store.Stats.ProductionSpeed;
       private float moveSpeed => _store.Stats.SpeedMoveSeller;
        private GameMode _gameMode;
        private Store _store;


        private SellerState _sellerState = SellerState.SearchingForCustomerState;
        public Order CurrentOrder;
        private int _currentOrderItemIndex; // Индекс текущего товара в заказе
        private bool _isCustomerAvailable;
        public Customer TargetCustomer;
        public CustomerSlot TargetCustomerSlot;
        public SellerSlot TargetSellerSlot;
        public SellerSlot CurrentSellerSlot;
        [SerializeField] DesktopSlot TargetDesktopSlot;

        public CharacterSpritesAndAnimationController characterSpritesAndAnimationController { get; set; }

       // public GameMode GameMode => _gameMode;


        public void Awake() {
            SellerView = GetComponentInChildren<UiSellerView>();
            SellerViewModel = new SellerViewModel(this, SellerView);
            _animator = GetComponentInChildren<Animator>();
            characterSpritesAndAnimationController = GetComponentInChildren<CharacterSpritesAndAnimationController>();
            
        }
        public void Construct(GameMode gameMode, Store store, CharacterType characterType, int idSprites) {
            _gameMode = gameMode;
            _store = store;
            
            CharacterType = characterType;
            IDSprites = idSprites;
            characterSpritesAndAnimationController.Construct(this, idSprites, characterType);
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
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_up);
                        yield return StartCoroutine(SearchingForCustomerRoutine());
                        break;
                    case SellerState.MovingToCustomerForOrderState:
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);
                        yield return StartCoroutine(MovingToCustomerForOrderRoutine());
                        break;
                    case SellerState.TakingOrderState:
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_up);
                        yield return StartCoroutine(TakingOrderRoutine());
                        break;

                    case SellerState.MovingToDesktopState:
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_down);
                        yield return StartCoroutine(MovingToDesktopRoutine());
                        break;
                    case SellerState.CollectingOrderState:
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_down);
                        yield return StartCoroutine(CollectingOrderRoutine());
                        break;
                    case SellerState.MovingToCustomerForDeliverState:
                        characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);
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
                  //  Debug.Log($"{this.IDSprites} + Получил данные нового покупателя {TargetCustomer.IDSprites}");

                    CurrentSellerSlot = TargetSellerSlot;

                    break;
                }

                if (_store.IsDesktopAvailable) {
                    (CurrentOrder, TargetDesktopSlot) = _store.SellerTryGetNewJob();
                    if (CurrentOrder != null) {
                        
                        TargetDesktopSlot.Seller = this;
                        if (CurrentSellerSlot!=null) CurrentSellerSlot.Seller = null;
                        TargetCustomer = CurrentOrder.Customer;
                        TargetCustomerSlot = TargetCustomer.CustomerSlot;
                        TargetSellerSlot = _store.GetSellerSlotByCustomerSlot(TargetCustomerSlot);
                        // Заказ найден, переключаемся на следующее состояние
                        _sellerState = SellerState.MovingToDesktopState;
                      //  Debug.Log("Заказ получен в работу");
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
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);
            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(customerPosition, SellerState.MovingToCustomerForOrderState);

            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_up);
            //  Debug.Log($"{this.IDSprites} Продавец подошел к новому покупателю {TargetCustomer.IDSprites} за заказом");
            _sellerState = SellerState.TakingOrderState;

        }

        // private IEnumerator TakingOrderRoutine() {
        //
        //     // Взятие заказа у покупателя
        //     Debug.Log($"{this.IDSprites} Продавец берет заказ у нового покупателя {TargetCustomer.IDSprites}");
        //     yield return new WaitForSeconds(timeTakingOrder);
        //     TargetCustomer.TransferOrder();
        //     Debug.Log("Продавец заказ у нового покупателя в лист заказов");
        //     _sellerState = SellerState.SearchingForCustomerState;
        //
        // }
        private IEnumerator TakingOrderRoutine() {
            float elapsedTime = 0;

            // Взятие заказа у покупателя
         //   Debug.Log($"{this.IDSprites} Продавец берет заказ у нового покупателя {TargetCustomer.IDSprites}");
            while (elapsedTime < timeTakingOrder) {
                elapsedTime += Time.deltaTime;
                OnUIChangedProgress?.Invoke(elapsedTime / timeTakingOrder, true);
                yield return null;
            }

            TargetCustomer.TransferOrder();
          //  Debug.Log("Продавец заказ у нового покупателя в лист заказов");
            _sellerState = SellerState.SearchingForCustomerState;
            OnUIChangedProgress?.Invoke(timeTakingOrder, false); // Убедитесь, что UI показывает полный прогресс после завершения
        }


        private IEnumerator MovingToDesktopRoutine() {
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_down);
            // Определите позицию свободного стола
            Vector3 tablePosition = TargetDesktopSlot.transform.position;

            // Запустите Dotween анимацию и ждите её завершения
            yield return MoveToTargetWithSpeed(tablePosition, SellerState.MovingToDesktopState);
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_down);
            _sellerState = SellerState.CollectingOrderState;


        }

        // private IEnumerator CollectingOrderRoutine() {
        //     // Сбор заказа
        //     Debug.Log("Собирает заказ за столом");
        //     yield return new WaitForSeconds(_timeProductionSpeed);
        //     Debug.Log("Заказ собран");
        //     TargetDesktopSlot.Seller = null;
        //     _sellerState = SellerState.MovingToCustomerForDeliverState;
        // } 
        private IEnumerator CollectingOrderRoutine() {
            float elapsedTime = 0;
            // Сбор заказа
           // Debug.Log("Собирает заказ за столом");
            while (elapsedTime < productionSpeed) {
                elapsedTime += Time.deltaTime;
                OnUIChangedProgress?.Invoke(elapsedTime / productionSpeed, true);
                yield return null;
            }
           // Debug.Log("Заказ собран");
            TargetDesktopSlot.Seller = null;
            _sellerState = SellerState.MovingToCustomerForDeliverState;
            OnUIChangedProgress?.Invoke(productionSpeed, false); // Убедитесь, что UI показывает полный прогресс после завершения
            OnUIChangedShowProduct?.Invoke();
        }
        private IEnumerator MovingToCustomerForDeliverRoutine() {
            //TargetSellerSlot = _store.GetSellerSlotByCustomerSlot(TargetCustomerSlot);

            // Определите точку, к которой нужно переместиться
            Vector3 customerPosition = TargetSellerSlot.transform.position;

            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.walk_up);
            // Запустите Dotween анимацию и ждите её завершения
            yield return
                MoveToTargetWithSpeed(customerPosition,
                    SellerState
                        .MovingToCustomerForDeliverState); //null;// transform.DOMove(customerPosition, GetMoveDuration(customerPosition)).SetEase(Ease.Linear).WaitForCompletion();
            characterSpritesAndAnimationController.UpdateAnimationAndSprites(AnimationState.idle_up);
            // После достижения покупателя продолжаем следующую корутину
            _sellerState = SellerState.DeliveredOrderState;
        }
        private IEnumerator DeliveredOrderRoutine() {
            // Доставка заказа покупателю
            TargetCustomer.DeliveredProduct(CurrentOrder);
            _store.DeliveredForSellProductSuccess(CurrentOrder);
            CurrentOrder = null;
            yield return null;
           // Debug.Log("Товар доставлен покупателю");
            OnUIChangedHideProduct?.Invoke();
            
            _sellerState = SellerState.SearchingForCustomerState; // Возвращаемся к поиску нового покупателя
            // Продавец возвращается в ожидающий слот (если такой используется)
            if (CurrentSellerSlot != null && CurrentSellerSlot.Seller != this) CurrentSellerSlot.Seller = this;
        }

        private IEnumerator MoveToTargetWithSpeed(Vector3 target, SellerState sender) {
            //Debug.Log($"{this.IDSprites} Продавец начал движение");
            float distance = Vector3.Distance(transform.position, target);
            float duration = distance / moveSpeed; // Рассчитываем продолжительность на основе скорости и расстояния
                                                    // Добавляем обработчик, который вызовется по завершении анимации
            if (Vector3.Distance(transform.position, target) > 0.1f) {
                yield return transform.DOMove(target, duration).SetEase(Ease.Linear).WaitForCompletion();
            }

        }

  

        private void OnDestroy() {
            OnUIChangedProgress = null;
            OnUIChangedShowProduct= null;
            OnUIChangedHideProduct= null;
        }

        
    }

}