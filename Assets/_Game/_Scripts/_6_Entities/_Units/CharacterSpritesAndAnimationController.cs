using Assets._Game._Scripts._0.Data._SpritesForPersons;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units {
    public enum AnimationState {
        walk_down,
        walk_up,
        idle_down,
        idle_up
    }
    public enum CharacterType {
        Customer, Seller,
    }
    public class CharacterSpritesAndAnimationController : MonoBehaviour
    {
        private BaseUnitGame _controller;
        [SerializeField] private int _idSprites;
        [SerializeField] private CharacterType _characterType;
        [SerializeField] public CharacterView ViewData;
        [SerializeField] public SpriteRenderer Body;
        [SerializeField] public SpriteRenderer Head;
        [SerializeField] public SpriteRenderer Hair;
        [SerializeField] public SpriteRenderer FaceElement;
        [SerializeField] public SpriteRenderer Clothes;
        private Animator _animator;

       

        private void Awake() {
            // Получение компонента Animator при инициализации
            _animator = GetComponentInChildren<Animator>();
           
        }

        public void Construct(BaseUnitGame controller, int idSprites, CharacterType characterType) {
            _controller = controller;
            // Инициализация данных персонажа
            _idSprites = idSprites;
            _characterType = characterType;
            GetCharacterSprites();
            
        }

        public void GetCharacterSprites()
        {
            var dataMode = FindObjectOfType<DataMode_>();
            // Получение данных о внешности персонажа
            if (_characterType == CharacterType.Customer)
            {
                int seed = (int)System.DateTime.Now.Ticks;
                Random.InitState(seed);
                // Получение рандомного индекса для массива внешности
                int randomIndex = Random.Range(0, dataMode.CharacterSpritesDataSo.Customers.Length);
                ViewData = dataMode.CharacterSpritesDataSo.Customers[randomIndex];
            }
            else
            {
                // Получение данных о внешности персонажа
                ViewData = dataMode.CharacterSpritesDataSo.Sellers[_idSprites];
            }
            SetSpritesForDirection(true);
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
            // Сброс всех параметров перед установкой нового состояния
            _animator.SetBool("walk_down", false);
            _animator.SetBool("walk_up", false);
            _animator.SetBool("idle_down", false);
            _animator.SetBool("idle_up", false);

            // Установка соответствующего анимационного состояния
            switch (state) {
                case AnimationState.walk_down:
                    _animator.SetBool("walk_down", true);
                    break;
                case AnimationState.walk_up:
                    _animator.SetBool("walk_up", true);
                    break;
                case AnimationState.idle_down:
                    _animator.SetBool("idle_down", true);
                    break;
                case AnimationState.idle_up:
                    _animator.SetBool("idle_up", true);
                    break;
            }

            // Определение, куда смотрит персонаж, и установка спрайтов
            bool isFacingDown = state == AnimationState.walk_down || state == AnimationState.idle_down;
            SetSpritesForDirection(isFacingDown);
        }

    }
}
