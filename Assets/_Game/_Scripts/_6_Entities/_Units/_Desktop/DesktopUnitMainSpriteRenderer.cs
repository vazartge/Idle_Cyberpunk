using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class DesktopUnitMainSpriteRenderer : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        private void Avake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
