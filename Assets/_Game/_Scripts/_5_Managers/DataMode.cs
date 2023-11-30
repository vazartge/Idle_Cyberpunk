using System;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public class DataMode : MonoBehaviour
    {
        public ResourceData Data;
        private GameMode _gameMode;
        private UIMode _uiMode;

       
        public void Construct(GameMode gameMode, UIMode uiMode) {
            _gameMode = gameMode;
            _uiMode = uiMode;
        }
        public Sprite GetIconByProductType(Type type)
        {
            var productInfo = Data.ProductsInfo.FirstOrDefault(p => p.ProductType.ToString() == type.Name);
            return productInfo != null ? productInfo.ProductIcon : Data.BaseIcon;
        }

        
    }
}


