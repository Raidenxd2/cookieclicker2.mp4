using UnityEngine;
using UnityToolbarExtender;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class ToolbarGameScene : MonoBehaviour
{
    static ToolbarGameScene()
	{
		ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
	}

	static void OnToolbarGUI()
	{
		GUILayout.FlexibleSpace();

		if (GUILayout.Button(new GUIContent("PI", "PreInit Scene")))
		{
			EditorSceneManager.OpenScene("Assets/_Project/Scenes/PreInit.unity");
		}

		if(GUILayout.Button(new GUIContent("I", "Init Scene")))
		{
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/Init.unity");
		}

		if(GUILayout.Button(new GUIContent("G", "Game Scene")))
		{
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/Game.unity");
		}
	}
}