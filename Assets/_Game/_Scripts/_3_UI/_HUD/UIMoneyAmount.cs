using System;
using System.Collections;
using Assets._Game._Scripts._4_Services;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._HUD {
    public class UIMoneyAmount : MonoBehaviour {

        [SerializeField] private TMP_Text _moneyAmountText;
        [SerializeField] private long amount = 0;

        private void Start()
        {
           
            
        }

        private void Update()
        {
            _moneyAmountText.text = NumberFormatterService.FormatNumber(amount);
        }
    }
}