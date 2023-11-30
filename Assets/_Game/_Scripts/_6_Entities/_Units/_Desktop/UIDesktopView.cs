using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class UIDesktopView: UiUnitView
    {
        [SerializeField] UIDesktopViewModel _viewModel;
        [SerializeField] GameObject _canvas;
        [SerializeField] TMP_Text _text;

        public void Construct(UIDesktopViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public void ShowWindow() {
            _canvas.SetActive(true);
        }

        public void OnButtonUpgrade() {

            _viewModel.OnButtonUpgrade();
        }

        public void UpdateOnChangeMoney(long money)
        {
            _text.text = NumberFormatterService.FormatNumber(money);
        }
    }
}