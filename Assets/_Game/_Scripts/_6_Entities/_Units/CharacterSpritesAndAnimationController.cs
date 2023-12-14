using Assets._Game._Scripts._0.Data._SpritesForPersons;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units {
    public enum AnimationState {
        walk_down,
        walk_up,
        idle_down,
        idle_up
    }

    public class CharacterSpritesAndAnimationController : MonoBehaviour {
        [SerializeField] private int _idSprites;
        [SerializeField] private CharacterType _characterType;
        [SerializeField] public CharacterSpritesDataSO.CharacterView ViewData;
        [SerializeField] public SpriteRenderer Body;
        [SerializeField] public SpriteRenderer Head;
        [SerializeField] public SpriteRenderer Hair;
        [SerializeField] public SpriteRenderer FaceElement;
        [SerializeField] public SpriteRenderer Clothes;
        private Animator _animator;

        public int IdSprites => _idSprites;
        public CharacterType CharacterType => _characterType;

        private void Awake() {
            // Получение компонента Animator при инициализации
            _animator = GetComponentInChildren<Animator>();
        }

        public void Construct(BaseUnitGame controller, int idSprites, CharacterType characterType) {
            // Инициализация данных персонажа
            _idSprites = idSprites;
            _characterType = characterType;
        }

        public void SetSpritesForDirection(bool isFacingDown) {
            // Установка спрайтов в зависимости от направления
            Body.sprite = isFacingDown ? ViewData.BodyDown : ViewData.BodyUp;
            Head.sprite = isFacingDown ? ViewData.HeadDown : ViewData.HeadUp;
            Hair.sprite = isFacingDown ? ViewData.HairDown : ViewData.HairUp;
            Clothes.sprite = isFacingDown ? ViewData.ClothesDown : ViewData.ClothesUp;

            // Установка видимости FaceElement и проверка на null
            if (FaceElement != null) {
                FaceElement.sprite = isFacingDown ? ViewData.FaceElementDown : ViewData.FaceElementUp;
                FaceElement.enabled = isFacingDown && ViewData.FaceElementDown != null;
            }

            // Убедитесь, что все спрайты, которые могут быть null, обрабатываются корректно
            Hair.enabled = (isFacingDown ? ViewData.HairDown : ViewData.HairUp) != null;
        }

        public void UpdateAnimationAndSprites(AnimationState state) {
            // Определение, куда смотрит персонаж, и установка спрайтов
            bool isFacingDown = state == AnimationState.walk_down || state == AnimationState.idle_down;

            // Если направление изменилось, обновляем спрайты
            if ((_animator.GetBool("walk_down") || _animator.GetBool("idle_down")) != isFacingDown) {
                SetSpritesForDirection(isFacingDown);
            }

            // Установка соответствующего анимационного состояния
            _animator.SetBool("walk_down", state == AnimationState.walk_down);
            _animator.SetBool("walk_up", state == AnimationState.walk_up);
            _animator.SetBool("idle_down", state == AnimationState.idle_down);
            _animator.SetBool("idle_up", state == AnimationState.idle_up);
        }
    }
}
