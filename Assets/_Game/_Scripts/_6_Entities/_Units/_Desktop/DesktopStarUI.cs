using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class DesktopStarUI : MonoBehaviour
    {
        public GameObject ActiveStar;

        public void ActivateStar()
        {
            ActiveStar.SetActive(true);
        }

        public void DeactivateStar()
        {
            ActiveStar.SetActive(false);
        }
    }
}
