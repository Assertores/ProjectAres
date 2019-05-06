using UnityEngine;
using UnityEditor;

public class AssetPost : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        if (assetPath.Contains("UI"))//distinguishes different import settings for different asset types e.g. UI, Background etc.
            return;
        if (assetPath.Contains("Environment"))
            return;
        if (assetPath.Contains("Logo"))
            return;

        textureImporter.spritePixelsPerUnit = 512;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.maxTextureSize = 512;
    }
}