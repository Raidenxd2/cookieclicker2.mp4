 using UnityEditor;
 using UnityEditor.SceneManagement;
 using UnityEngine;
 using System.Collections;
 
 [InitializeOnLoad]
 public static class Play
{  
    [MenuItem("Edit/Play-Unplay, But From Prelaunch Scene %0")]
    public static void PlayFromPrelaunchScene()
    {
        if ( EditorApplication.isPlaying == true )
        {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Init.unity");
        EditorApplication.isPlaying = true;
    }
}