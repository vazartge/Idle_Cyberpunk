using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(Canvas))]
    public class FitCanvasToSprite : MonoBehaviour {
        public SpriteRenderer targetSpriteRenderer;

        private Canvas canvas;
        private RectTransform canvasRectTransform;

        void Awake() {
            // �������� ���������� Canvas � RectTransform
            canvas = GetComponent<Canvas>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();

            // ���������, ��� Canvas �������� �� World Space
            if (canvas.renderMode != RenderMode.WorldSpace) {
                Debug.LogError("Canvas ������ ���� �������� �� World Space.", this);
                return;
            }

            // ���������, ��� SpriteRenderer �����
            if (targetSpriteRenderer == null) {
                Debug.LogError("���������� ������ targetSpriteRenderer.", this);
                return;
            }

            ResizeCanvasToSprite();
            CenterAllChildren();
        }

        void ResizeCanvasToSprite() {
            // �������� ������ ������� � ��������
            Vector2 spriteSize = targetSpriteRenderer.sprite.rect.size;

            // ������� �� ������� ������� (������ 100)
            float pixelsPerUnit = targetSpriteRenderer.sprite.pixelsPerUnit;

            // ������������� ������ Canvas � ������ �������� �� ������� �������
            Vector2 canvasSize = spriteSize / pixelsPerUnit;

            // ��������� ������ � RectTransform Canvas
            canvasRectTransform.sizeDelta = canvasSize;

            // ��������� �������� canvas � ������������ � ��������� �������
            canvasRectTransform.localScale = new Vector3(1, 1, 1);
        }

        void CenterAllChildren() {
            // ���������� ��� �������� �������� UI � Canvas
            foreach (RectTransform child in canvasRectTransform) {
                // ��������� ������ � �����
                child.anchorMin = new Vector2(0.5f, 0.5f);
                child.anchorMax = new Vector2(0.5f, 0.5f);
                child.pivot = new Vector2(0.5f, 0.5f);

                // ����� ���������������� � ��������
                child.anchoredPosition = Vector2.zero;
                child.sizeDelta = Vector2.zero;
                child.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
