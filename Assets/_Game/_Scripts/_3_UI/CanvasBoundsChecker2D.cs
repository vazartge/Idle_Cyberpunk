using UnityEngine;

namespace Assets._Game._Scripts._3_UI
{
    // Удерживает тултипыстолов и пребилдеров в рамках камеры
    public class CanvasBoundsChecker2D : MonoBehaviour {
        public Camera mainCamera;
        public RectTransform canvasRectTransform;

        void OnEnable() {
            if (mainCamera == null) mainCamera = Camera.main;
            if (canvasRectTransform == null) canvasRectTransform = GetComponent<RectTransform>();

            AdjustCanvasPositionToCameraBounds();
        }

        private void AdjustCanvasPositionToCameraBounds() {
            Vector3 canvasPosition = canvasRectTransform.position;
            Vector2 canvasSize = new Vector2(canvasRectTransform.rect.width * canvasRectTransform.localScale.x,
                canvasRectTransform.rect.height * canvasRectTransform.localScale.y);

            // Получаем границы видимости камеры
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            Vector3 cameraPosition = mainCamera.transform.position;

            // Вычисляем минимальные и максимальные границы
            float minX = cameraPosition.x - cameraWidth / 2 + canvasSize.x / 2;
            float maxX = cameraPosition.x + cameraWidth / 2 - canvasSize.x / 2;
            float minY = cameraPosition.y - cameraHeight / 2 + canvasSize.y / 2;
            float maxY = cameraPosition.y + cameraHeight / 2 - canvasSize.y / 2;

            // Корректируем позицию
            canvasPosition.x = Mathf.Clamp(canvasPosition.x, minX, maxX);
            canvasPosition.y = Mathf.Clamp(canvasPosition.y, minY, maxY);

            canvasRectTransform.position = canvasPosition;
        }
    }
}