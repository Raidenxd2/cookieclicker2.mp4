// ScreenshotTool.cs
/*
 * Original: https://entitycrisis.blogspot.com/2017/02/take-unity-screenshot-with-alpha.html
 */
 #if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class ScreenshotTool : MonoBehaviour {
    [Range(1, 10)]
    [Tooltip("Specifies how many times to multiple the final image dimensions")]
    public int UpScale = 4;
    [Tooltip("Specifies to use a transparent background.\n\nNote: This may not create an image with a trasparent background if the camera is filled with objects. This basically just clears the skybox. So if you want a to capture a 3D object with a transparent background, place it in an empty scene.")]
    public bool AlphaBackground = true;
    public void SaveScreenshot() {
        string path = Application.persistentDataPath;
        string filename = "SS-" + DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
        File.WriteAllBytes(Path.Combine(path, filename), CreateScreenshot().EncodeToPNG());
    }

    private Texture2D CreateScreenshot() {
        Camera camera = GetComponent<Camera>();
        int w = camera.pixelWidth * UpScale;
        int h = camera.pixelHeight * UpScale;
        
        RenderTexture rt = new RenderTexture(w, h, 32);

        // Enable Anti-Aliasing on Render Texture
        rt.antiAliasing = 8;

        camera.targetTexture = rt;

        var screenShot = new Texture2D(w, h, TextureFormat.ARGB32, false);
        var clearFlags = camera.clearFlags;
        if (AlphaBackground) {
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0, 0, 0, 0);
        }
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        screenShot.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        camera.clearFlags = clearFlags;
        return screenShot;
    }
}
[CustomEditor(typeof(ScreenshotTool))]
public class ScreenshotToolEditor : Editor {
    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
        if (GUILayout.Button("Take Screenshot")) {
            ScreenshotTool capture = (ScreenshotTool) target;
            capture.SaveScreenshot();
        }
    }
}
#endif