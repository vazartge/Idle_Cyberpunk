using UnityEngine;

namespace Assets._Game._Scripts._0.Data._SpritesForPersons
{
    // В сборных спрайтах персонажей некоторые спрайты меняют положение в зависимости от положения игрока вверх или вниз
    
    public enum NeedChangeOrderGameobjectSpriteRenderer
    {
        none = 0,
        highClothesDown = 1, // надо переместить объект головы в префабе персонажа выше одежды
    }

    [System.Serializable]
    public class CharacterView {
        public NeedChangeOrderGameobjectSpriteRenderer NeedChangeOrderGameobjectSpriteRenderer;

        public Sprite BodyDown;
        public Sprite HeadDown;
        public Sprite HairDown;        // Может быть null
        public Sprite ClothesDown;
        public Sprite FaceElementDown; // Может быть null

        public Sprite BodyUp;
        public Sprite HeadUp;
        public Sprite HairUp;          // Может быть null
        public Sprite ClothesUp;
        public Sprite FaceElementUp;   // Может быть null
    }
}