using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using DG.Tweening;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Customers
{
    public enum StateCustomer{WaitSellerForOrdering, WaitProduct}

    public class Customer : MonoBehaviour {
        private GameMode _gameMode;
        private Store _store;
        private CustomerSlot _freeSlot;
        private Order _order;
        private StateCustomer _stateCustomer;
        public float speed = 2f; // Скорость перемещения в единицах в секунду

        public void Construct(GameMode gameMode, Store store)
        {
            _gameMode = gameMode;
            _store = store;
        }

        public void SetupCustomer(CustomerSlot freeSlot, Order order)
        {
            _freeSlot = freeSlot;
            _order = order;
            MoveToTargetWithSpeed();
        }
        private void MoveToTargetWithSpeed() {
            float distance = Vector3.Distance(transform.position, _freeSlot.transform.position);
            float duration = distance / speed; // Рассчитываем продолжительность на основе скорости и расстояния
            // Добавляем обработчик, который вызовется по завершении анимации
            transform.DOMove(_freeSlot.transform.position, duration).SetEase(Ease.Linear).OnComplete(ReachedDestination);
        }
        private void ReachedDestination()
        {
            _store.CustomerIsReachedStore(this,_freeSlot);
            // Действия после достижения цели
            Debug.Log("Достигнута точка назначения");
        }
    }
}