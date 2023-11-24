using System.Collections;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Customers {


    public class Customer : UnitGame {
        private enum CustomerState {
            MovingToTradeState,
            WaitSellerForOrderingState,
            WaitProductState,
            MovingFromTradeState,
        }
        private GameMode _gameMode;
        private Store _store;
        private CustomerSlot _freeSlot;
        private Order _order;
        private CustomerState _customerState;
        public float speed = 2f; // Скорость перемещения в единицах в секунду

        public void Construct(GameMode gameMode, Store store) {
            _gameMode = gameMode;
            _store = store;
        }

        public void SetupCustomer(CustomerSlot freeSlot, Order order) {
            _freeSlot = freeSlot;
            _freeSlot.Customer = this;
            _order = order;
            StartCoroutine( MovingToTradeRoutine());
        }

        private IEnumerator CustomerRoutine() {
            while (true) {
                switch (_customerState) {
                    case CustomerState.MovingToTradeState:
                        yield return StartCoroutine(MovingToTradeRoutine());
                        break;
                    case CustomerState.WaitSellerForOrderingState:
                        break;
                    case CustomerState.WaitProductState:
                        break;
                    case CustomerState.MovingFromTradeState:
                        break;
                    default:
                        Debug.Log("Нет такого варианта действий Покупателя");
                        break;
                }
            }
        }
        private IEnumerator MovingToTradeRoutine() {
            // Вычисляем промежуточную точку на одной линии с прилавком, но по горизонтали от покупателя
            Vector3 intermediatePoint = new Vector3(_freeSlot.transform.position.x, transform.position.y, transform.position.z);

            // Сначала перемещаемся к промежуточной точке
            float horizontalDistance = Vector3.Distance(transform.position, intermediatePoint);
            float horizontalDuration = horizontalDistance / speed;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(intermediatePoint, horizontalDuration).SetEase(Ease.Linear));

            // Затем перемещаемся от промежуточной точки к прилавку
            float verticalDistance = Vector3.Distance(intermediatePoint, _freeSlot.transform.position);
            float verticalDuration = verticalDistance / speed;
            sequence.Append(transform.DOMove(_freeSlot.transform.position, verticalDuration).SetEase(Ease.Linear));
            yield return null;
            // Добавляем обработчик, который вызовется по завершении всей анимации
            sequence.OnComplete(ReachedDestination);
        }

        private void ReachedDestination() {
            _store.CustomerIsReachedStore(this, _freeSlot);
            // Действия после достижения цели
            Debug.Log("Достигнута точка назначения");
        }

        public Order TransferOrder() {
            return _order;
        }
    }
}