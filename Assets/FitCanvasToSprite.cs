using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(Canvas))]
    public class FitCanvasToSprite : MonoBehaviour {
        public SpriteRenderer targetSpriteRenderer;

        private Canvas canvas;
        private RectTransform canvasRectTransform;

        void Awake() {
            // Получаем компоненты Canvas и RectTransform
            canvas = GetComponent<Canvas>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();

            // Проверяем, что Canvas настроен на World Space
            if (canvas.renderMode != RenderMode.WorldSpace) {
                Debug.LogError("Canvas должен быть настроен на World Space.", this);
                return;
            }

            // Проверяем, что SpriteRenderer задан
            if (targetSpriteRenderer == null) {
                Debug.LogError("Необходимо задать targetSpriteRenderer.", this);
                return;
            }

            ResizeCanvasToSprite();
            CenterAllChildren();
        }

        void ResizeCanvasToSprite() {
            // Получаем размер спрайта в пикселях
            Vector2 spriteSize = targetSpriteRenderer.sprite.rect.size;

            // Пиксели на единицу спрайта (обычно 100)
            float pixelsPerUnit = targetSpriteRenderer.sprite.pixelsPerUnit;

            // Пересчитываем размер Canvas с учетом пикселей на единицу спрайта
            Vector2 canvasSize = spriteSize / pixelsPerUnit;

            // Применяем размер к RectTransform Canvas
            canvasRectTransform.sizeDelta = canvasSize;

            // Установка масштаба canvas в соответствии с масштабом спрайта
            canvasRectTransform.localScale = new Vector3(1, 1, 1);
        }

        void CenterAllChildren() {
            // Центрируем все дочерние элементы UI в Canvas
            foreach (RectTransform child in canvasRectTransform) {
                // Установка якорей в центр
                child.anchorMin = new Vector2(0.5f, 0.5f);
                child.anchorMax = new Vector2(0.5f, 0.5f);
                child.pivot = new Vector2(0.5f, 0.5f);

                // Сброс позиционирования и масштаба
                child.anchoredPosition = Vector2.zero;
                child.sizeDelta = Vector2.zero;
                child.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
