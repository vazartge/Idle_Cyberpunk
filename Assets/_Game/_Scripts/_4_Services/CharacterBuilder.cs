using Assets._Game._Scripts._0.Data._SpritesForPersons;
using UnityEngine;

public class CharacterBuilder {
    private CharacterDataSO characterData;

    public CharacterBuilder(CharacterDataSO data) {
        characterData = data;
    }

    public GameObject BuildCharacter(int id, CharacterType type) {
        // Создание нового объекта персонажа
        GameObject characterObj = new GameObject("Character");

        // Получение данных о внешности персонажа
        CharacterDataSO.CharacterAppearance appearance =
            type == CharacterType.Seller ? characterData.sellers[id] : characterData.buyers[id];

        // Добавление и настройка компонентов SpriteRenderer
        AddSpriteRenderer(characterObj, appearance.head, BodyPart.Head);
        AddSpriteRenderer(characterObj, appearance.hair, BodyPart.Hair);
        AddSpriteRenderer(characterObj, appearance.clothes, BodyPart.Clothes);
        AddSpriteRenderer(characterObj, appearance.faceElement, BodyPart.FaceElement);

        // Добавление и настройка Animator
        var animator = characterObj.AddComponent<Animator>();
        // Настройка Animator здесь...

        return characterObj;
    }

    private void AddSpriteRenderer(GameObject parent, Sprite sprite, BodyPart part) {
        if (sprite != null) {
            GameObject child = new GameObject(part.ToString());
            child.transform.SetParent(parent.transform);
            var renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }
    }

    public enum CharacterType {
        Seller,
        Buyer
    }

    public enum BodyPart {
        Head,
        Hair,
        Clothes,
        FaceElement
    }
}