using UnityEngine;
using UnityEditor;

public class AssetPost : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.spritePixelsPerUnit = 512;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.maxTextureSize = 512;
    }
}