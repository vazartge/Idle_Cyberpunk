using Assets._Game._Scripts._0.Data._SpritesForPersons;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._4_Services {
    public enum CharacterType {
        Seller,
        Customer
    }


    public class CharacterSpritesBuilder {
        private CharacterSpritesDataSO _characterSpritesData;
        private GameMode _gameMode;

        public CharacterSpritesBuilder(GameMode gameMode) {
            _gameMode = gameMode;
            _characterSpritesData = _gameMode.DataMode.GetCharacterDataForSprites();
        }

        public bool SetupCharacterSprites(ICharacterUnitChangableSprites sritesStates) {

            CharacterSpritesAndAnimationController andAnimationController = sritesStates.characterSpritesAndAnimationController;
            CharacterSpritesDataSO.CharacterView viewData;
            // Получение данных о внешности персонажа
            if (andAnimationController.CharacterType == CharacterType.Customer) {
                // Получение рандомного индекса для массива внешности
                int randomIndex = Random.Range(0, andAnimationController.CharacterType == CharacterType.Seller ?
                    _characterSpritesData.sellers.Length : _characterSpritesData.buyers.Length);
                viewData = _characterSpritesData.buyers[randomIndex];
            }
            else
            {
                // Получение данных о внешности персонажа
                viewData = _characterSpritesData.sellers[andAnimationController.IdSprites];
            }
            andAnimationController.ViewData = viewData;

            // Установка спрайтов и управление видимостью
            SetSpriteAndVisibility(andAnimationController.Body, viewData.BodyDown);
            SetSpriteAndVisibility(andAnimationController.Head, viewData.HeadDown);
            SetSpriteAndVisibility(andAnimationController.Hair, viewData.HairDown);
            SetSpriteAndVisibility(andAnimationController.Clothes, viewData.ClothesDown);
            SetSpriteAndVisibility(andAnimationController.FaceElement, viewData.FaceElementDown);


            return true;
        }

        private void SetSpriteAndVisibility(SpriteRenderer renderer, Sprite sprite) {
            if (sprite != null) {
                renderer.sprite = sprite;
                renderer.enabled = true; // Сделать SpriteRenderer видимым
            } else {
                renderer.enabled = false; // Скрыть SpriteRenderer, если спрайт отсутствует
            }
        }


    }
}