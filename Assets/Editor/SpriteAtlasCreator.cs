using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Assets._Game._Scripts._0.Data._SpritesForPersons;

public class SpriteAtlasGenerator {
    private const int SpriteSize = 256;
    private const int AtlasWidth = SpriteSize * 2; // ��� ���� �������� ��������: ����� Down, ������ Up

    [MenuItem("Tools/Create Sprite Atlas")]
    private static void CreateAtlas() {
        CharacterSpritesDataSO data = Selection.activeObject as CharacterSpritesDataSO;
        if (data == null) {
            Debug.LogError("No CharacterSpritesDataSO selected!");
            return;
        }

        List<CharacterView> characterViews = new List<CharacterView>(data.Sellers);
        characterViews.AddRange(data.Customers);

        int atlasHeight = characterViews.Count * SpriteSize;
        Texture2D atlas = new Texture2D(AtlasWidth, atlasHeight, TextureFormat.RGBA32, false);

        ClearTexture(atlas, AtlasWidth, atlasHeight);

        for (int i = 0; i < characterViews.Count; i++) {
            int yOffset = i * SpriteSize;
            MergeSprites(atlas, characterViews[i], 0, yOffset); // ����������� Down
            MergeSprites(atlas, characterViews[i], SpriteSize, yOffset); // ����������� Up
        }

        atlas.Apply();

        string filePath = EditorUtility.SaveFilePanel("Save Sprite Atlas", "", "CharacterAtlas", "png");
        if (!string.IsNullOrEmpty(filePath)) {
            SaveTextureAsPNG(atlas, filePath);
        }
    }

    private static void MergeSprites(Texture2D atlas, CharacterView view, int xOffset, int yOffset) {
        MergeSprite(atlas, view.BodyDown, xOffset, yOffset);
        MergeSprite(atlas, view.HeadDown, xOffset, yOffset);
        MergeSprite(atlas, view.HairDown, xOffset, yOffset);
        MergeSprite(atlas, view.ClothesDown, xOffset, yOffset);
        MergeSprite(atlas, view.FaceElementDown, xOffset, yOffset);

        xOffset += SpriteSize; // �������� ��� �������� Up
        MergeSprite(atlas, view.BodyUp, xOffset, yOffset);
        MergeSprite(atlas, view.HeadUp, xOffset, yOffset);
        MergeSprite(atlas, view.HairUp, xOffset, yOffset);
        MergeSprite(atlas, view.ClothesUp, xOffset, yOffset);
        MergeSprite(atlas, view.FaceElementUp, xOffset, yOffset);
    }

    private static void MergeSprite(Texture2D atlas, Sprite sprite, int xOffset, int yOffset) {
        if (sprite == null) return;

        Color[] spritePixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);

        atlas.SetPixels(xOffset, yOffset, (int)sprite.textureRect.width, (int)sprite.textureRect.height, spritePixels);
    }

    private static void ClearTexture(Texture2D texture, int width, int height) {
        var clearPixels = new Color[width * height];
        for (int i = 0; i < clearPixels.Length; i++) {
            clearPixels[i] = Color.clear;
        }
        texture.SetPixels(clearPixels);
        texture.Apply();
    }

    private static void SaveTextureAsPNG(Texture2D texture, string filePath) {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        AssetDatabase.Refresh();
        Debug.Log($"Atlas saved to {filePath}");
    }
}


//
//
//
// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Linq;
// using Assets._Game._Scripts._0.Data._SpritesForPersons;
//
// public class SpriteAtlasGenerator {
//     private const int SpriteSize = 256; // ������ ������� �������
//     private const int AtlasWidth = SpriteSize * 2; // ������ ������ ��� ���� ��������
//
//     [MenuItem("Tools/Create Sprite Atlas")]
//     private static void CreateAtlas() {
//         var selectedObject = Selection.activeObject as CharacterSpritesDataSO;
//         if (selectedObject == null) {
//             Debug.LogError("You must select a CharacterSpritesDataSO asset!");
//             return;
//         }
//
//         int totalCharacters = selectedObject.Sellers.Length + selectedObject.Customers.Length;
//         int atlasHeight = SpriteSize * totalCharacters;
//         Texture2D atlas = new Texture2D(AtlasWidth, atlasHeight, TextureFormat.RGBA32, false);
//         ClearTexture(atlas, AtlasWidth, atlasHeight);
//
//         int currentY = 0;
//         foreach (var characterView in selectedObject.Sellers.Concat(selectedObject.Customers)) {
//             // ����������� ������� ��� ����������� Down � Up
//             OverlaySpritesOnAtlas(atlas, new[] { characterView.BodyDown, characterView.HeadDown, characterView.HairDown, characterView.ClothesDown, characterView.FaceElementDown }, 0, currentY);
//             OverlaySpritesOnAtlas(atlas, new[] { characterView.BodyUp, characterView.HeadUp, characterView.HairUp, characterView.ClothesUp, characterView.FaceElementUp }, SpriteSize, currentY);
//             currentY += SpriteSize;
//         }
//
//         atlas.Apply();
//
//         // ��������� �����
//         string folderPath = EditorUtility.SaveFolderPanel("Save Sprite Atlases", "", "");
//         if (string.IsNullOrEmpty(folderPath))
//             return;
//
//         string atlasPath = Path.Combine(folderPath, "CharacterAtlas.png");
//         SaveTextureAsPNG(atlas, atlasPath);
//     }
//
//
//     private static void OverlaySpritesOnAtlas(Texture2D atlas, Sprite[] sprites, int xOffset, int yOffset) {
//         foreach (Sprite sprite in sprites) {
//             if (sprite == null) continue;
//
//             Color[] spritePixels = sprite.texture.GetPixels(
//                 (int)sprite.textureRect.x,
//                 (int)sprite.textureRect.y,
//                 (int)sprite.textureRect.width,
//                 (int)sprite.textureRect.height);
//
//             // ������������ �������� ��� ������������� ������� (bottom center)
//             int centeredXOffset = xOffset + (SpriteSize - (int)sprite.textureRect.width) / 2;
//             int centeredYOffset = yOffset + (SpriteSize - (int)sprite.textureRect.height);
//
//             // ���������, ����� ������ �� ������� �� ������� ������
//             if (centeredXOffset + sprite.textureRect.width > atlas.width || centeredYOffset + sprite.textureRect.height > atlas.height) {
//                 Debug.LogError($"Sprite {sprite.name} exceeds the atlas bounds.");
//                 continue;
//             }
//
//             // ����������� ������ �� �����, �������� ������������
//             for (int y = 0; y < (int)sprite.textureRect.height; y++) {
//                 for (int x = 0; x < (int)sprite.textureRect.width; x++) {
//                     int atlasPixelIndex = (centeredXOffset + x) + (centeredYOffset + y) * atlas.width;
//                     Color currentColor = atlas.GetPixel(centeredXOffset + x, centeredYOffset + y);
//                     Color newColor = spritePixels[x + y * (int)sprite.textureRect.width];
//
//                     // ��������� �����, �������� ������������ ������ �������
//                     atlas.SetPixel(centeredXOffset + x, centeredYOffset + y, Color.Lerp(currentColor, newColor, newColor.a));
//                 }
//             }
//         }
//
//         atlas.Apply();
//     }
//     private static void ClearTexture(Texture2D texture, int width, int height) {
//         var clearPixels = Enumerable.Repeat(Color.clear, width * height).ToArray();
//         texture.SetPixels(clearPixels);
//     }
//
//
//
//
//     private static void SaveTextureAsPNG(Texture2D texture, string filePath) {
//         byte[] bytes = texture.EncodeToPNG();
//         File.WriteAllBytes(filePath, bytes);
//         AssetDatabase.Refresh();
//         Debug.Log($"Saved atlas to {filePath}");
//     }
// }
