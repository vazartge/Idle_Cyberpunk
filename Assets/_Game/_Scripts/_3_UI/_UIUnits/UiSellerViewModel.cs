using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._UIUnits
{
    public class UiSellerViewModel : UIUnitViewModel
    {
        private Seller _sellerModel;
        private UiSellerView _sellerView;

        public UiSellerViewModel(Seller seller, UiSellerView view) {
            _sellerModel = seller;
            _sellerView = view;

            // Подписка на изменения в модели (например, на события изменения заказа)
            _sellerModel.OnUIChangedProgress += UpdateProgressView;
            _sellerModel.OnUIChangedShowProduct += UpdateIconShowProductView;
            _sellerModel.OnUIChangedHideProduct += UpdateIconHideProductView;
        }

       
       
        private void UpdateProgressView(float progress, bool isShow)
        {
            _sellerView.UpdateSellerProgressUI(progress, isShow);

        }
        private void UpdateIconHideProductView() {
            _sellerView.UpdateSellerHideIconUI();
        }

        private void UpdateIconShowProductView()
        {
            Sprite icon;
            if (_sellerModel.CurrentOrder != null)
            {
                // Получаем данные из модели и обновляем View
                icon = ResManager.Instance.GetIconByProductType(_sellerModel.CurrentOrder.GetProductType()); // Предполагаем, что метод GetIconByProductType возвращает Sprite
            }
            else
            {
                icon = null;
            }
            _sellerView.UpdateSellerShowIconUI(icon);
        }

      
    }
}