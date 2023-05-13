using System.Collections;
using System.Collections.Generic;
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

		if(GUILayout.Button(new GUIContent("I", "Init Scene")))
		{
			// SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene1.unity");
            EditorSceneManager.OpenScene("Assets/Scenes/Init.unity");
		}

		if(GUILayout.Button(new GUIContent("G", "Game Scene")))
		{
			// SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene2.unity");
            EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
		}
		}
}
