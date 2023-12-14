using UnityEngine;

namespace Assets._Game._Scripts._0.Data._SpritesForPersons {
    [CreateAssetMenu(fileName = "CharacterSpritesDataSO", menuName = "Game/Data/CharacterData")]
    public class CharacterSpritesDataSO: ScriptableObject {

        public CharacterView[] sellers;
        public CharacterView[] buyers;

       
        [System.Serializable]
        public class CharacterView {
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
}
