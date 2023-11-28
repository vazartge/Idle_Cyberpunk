using System;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public class ResManager : MonoBehaviour
    {
        public static ResManager Instance;
        public ResourceData Data;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }

        }

        public Sprite GetIconByProductType(Type type)
        {
            var productInfo = Data.ProductsInfo.FirstOrDefault(p => p.ProductType.ToString() == type.Name);
            return productInfo != null ? productInfo.ProductIcon : Data.BaseIcon;
        }
    }
}


