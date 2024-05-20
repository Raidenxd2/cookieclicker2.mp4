using UnityEditor;

public class GPG_PC_TextureTools : Editor
{
    [MenuItem("Cookieclicker2.mp4/Switch textures to GPGPC")]
    public static void SwitchToGPGPC()
    {
        GPG_PC_TextureSwitchSO so = (GPG_PC_TextureSwitchSO)AssetDatabase.LoadAssetAtPath("Assets/TextureSwitch.asset", typeof(GPG_PC_TextureSwitchSO));

        foreach (var item in so.TextureSwitches)
        {
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(item.texture));
            
            TextureImporterPlatformSettings platformSettings = new();
            platformSettings = importer.GetPlatformTextureSettings("Android");

            platformSettings.maxTextureSize = (int)item.textureSizeGPGPC;
            platformSettings.format = item.textureFormatGPGPC;

            importer.mipmapEnabled = item.GenerateMipmapGPGPC;

            importer.SetPlatformTextureSettings(platformSettings);

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        AssetDatabase.Refresh();

        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.DXT;
    }

    [MenuItem("Cookieclicker2.mp4/Switch textures to Android")]
    public static void SwitchToAndroid()
    {
        GPG_PC_TextureSwitchSO so = (GPG_PC_TextureSwitchSO)AssetDatabase.LoadAssetAtPath("Assets/TextureSwitch.asset", typeof(GPG_PC_TextureSwitchSO));

        foreach (var item in so.TextureSwitches)
        {
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(item.texture));
            
            TextureImporterPlatformSettings platformSettings = new();
            platformSettings = importer.GetPlatformTextureSettings("Android");

            platformSettings.maxTextureSize = (int)item.textureSizeAndroid;
            platformSettings.format = item.textureFormatAndroid;

            importer.mipmapEnabled = item.GenerateMipmapAndroid;

            importer.SetPlatformTextureSettings(platformSettings);

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        AssetDatabase.Refresh();

        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
    }
}