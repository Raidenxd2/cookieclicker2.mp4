using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureSwitch", menuName = "Cookieclicker2.mp4/TextureSwitch", order = 1)]
public class GPG_PC_TextureSwitchSO : ScriptableObject
{
    public GPG_PC_TextureSwitch[] TextureSwitches;
}

[Serializable]
public class GPG_PC_TextureSwitch
{
    public Texture2D texture;

    public TextureImporterFormat textureFormatGPGPC;
    public TextureImporterFormat textureFormatAndroid;

    public TextureSize textureSizeGPGPC;
    public TextureSize textureSizeAndroid;

    public bool GenerateMipmapGPGPC;
    public bool GenerateMipmapAndroid;
}

public enum TextureSize
{
    [InspectorName("32")]
    a = 32,
    [InspectorName("64")]
    b = 64,
    [InspectorName("128")]
    c = 128,
    [InspectorName("256")]
    d = 256,
    [InspectorName("512")]
    e = 512,
    [InspectorName("1024")]
    f = 1024,
    [InspectorName("2048")]
    g = 2048,
    [InspectorName("4096")]
    h = 4096
}