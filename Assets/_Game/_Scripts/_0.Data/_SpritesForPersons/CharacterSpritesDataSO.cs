using UnityEngine;

namespace Assets._Game._Scripts._0.Data._SpritesForPersons {
    // Используется для хранения спрайтов для сборных спрайтов персонажаей
    [CreateAssetMenu(fileName = "CharacterSpritesDataSO", menuName = "Game/Data/CharacterData")]
    public class CharacterSpritesDataSO : ScriptableObject
    {

        public CharacterView[] Sellers;
        public CharacterView[] Customers;
    }
}

