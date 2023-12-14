using UnityEngine;

namespace Assets._Game._Scripts._0.Data._SpritesForPersons {
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Game/Data/CharacterData")]
    public class CharacterDataSO: ScriptableObject {

        public CharacterAppearance[] sellers;
        public CharacterAppearance[] buyers;

        [System.Serializable]
        public class CharacterAppearance {
            public Sprite head;
            public Sprite hair;        // Может быть null
            public Sprite clothes;
            public Sprite faceElement; // Может быть null
        }
    }
}
