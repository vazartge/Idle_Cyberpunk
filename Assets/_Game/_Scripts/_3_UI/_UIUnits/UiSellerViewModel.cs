using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._UIUnits
{
    class UiSellerViewModel : UIUnitViewModel
    {
        private Seller _sellerModel;
        private UiSellerUnitView _sellerView;

        public UiSellerViewModel(Seller seller, UiSellerUnitView view)
        {
            _sellerModel = seller;
            _sellerView = view;

            // Подписка на изменения в модели (например, на события изменения заказа)
            _sellerModel.OnUIChangedProgress += UpdateProgressView;
            _sellerModel.OnUIChangedProgress += UpdateIconProductView;
        }


        private void UpdateProgressView()
        {


        }

        private void UpdateIconProductView()
        {
            Sprite icon;
            if (_sellerModel.TargetCustomer.Orders.Count != 0)
            {
                // Получаем данные из модели и обновляем View
                icon = ResManager.Instance.GetIconByProductType(_sellerModel.TargetCustomer.Orders[0]
                    .GetProductType()); // Предполагаем, что метод GetIconByProductType возвращает Sprite
            }
        }
    }
}