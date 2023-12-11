using System;
using System.Collections.Generic;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    [Serializable]
    public class StoreStats
    {

        [SerializeField] private long coins = 1000;
        [SerializeField] private int levelGame = 1;
        [SerializeField] private float speedMoveCustomer = 5f;
        [SerializeField] private float speedMoveSeller = 5f;
        [SerializeField] private float productionSpeed = 2f;
        [SerializeField] private float takingOrder = 2f;

        [SerializeField] private LevelUpgrade levelUpgrade = new LevelUpgrade(

            new LevelUpgrade(
                new List<UpgradeCustomer>
                {
                    new UpgradeCustomer("Второй покупатель", 2, 0, false), // Изначально
                    new UpgradeCustomer("Третий покупатель", 3, 20, false),
                    new UpgradeCustomer("Четвертый покупатель", 4, 250, false)
                },
                new List<UpgradeSeller>
                {
                    new UpgradeSeller("Первый продавец", 1, 0, false), // Изначально
                    new UpgradeSeller("Второй продавец", 2, 35, false)
                },
                new ProductBoost("Ускорение производства 30%", 1.3f, 350, false), // на 30% быстрее

                new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 500, false) // на 20% быстрее
            ));


    

    public long Coins
        {
            get => coins;
            set => coins = value;
        }

        public int LevelGame
        {
            get => levelGame;
            set => levelGame = value;
        }

        public float SpeedMoveCustomer
        {
            get => speedMoveCustomer;
            set => speedMoveCustomer = value;
        }

        public float SpeedMoveSeller
        {
            get => speedMoveSeller;
            set => speedMoveSeller = value;
        }

        public float ProductionSpeed
        {
            get => productionSpeed;
            set => productionSpeed = value;
        }

        public float TakingOrder
        {
            get => takingOrder;
            set => takingOrder = value;
        }

        public LevelUpgrade LevelUpgrade
        {
            get => levelUpgrade;
            set => levelUpgrade = value;
        }
    }
}
