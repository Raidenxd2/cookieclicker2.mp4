using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;

public class CC2Toolbar
{
    [MainToolbarElement("Cookieclicker2.mp4/Scenes/PreInit", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement PreInitButton()
    {
        MainToolbarContent content = new("PI");
        return new MainToolbarButton(content, () => { LoadSceneWithSaveMessage("Assets/_Project/Scenes/PreInit.unity"); });
    }
    
    [MainToolbarElement("Cookieclicker2.mp4/Scenes/Init", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement InitButton()
    {
        MainToolbarContent content = new("I");
        return new MainToolbarButton(content, () => { LoadSceneWithSaveMessage("Assets/_Project/Scenes/Init.unity"); });
    }
    
    [MainToolbarElement("Cookieclicker2.mp4/Scenes/Game", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement GameButton()
    {
        MainToolbarContent content = new("G");
        return new MainToolbarButton(content, () => { LoadSceneWithSaveMessage("Assets/_Project/Scenes/Game.unity"); });
    }

    private static void LoadSceneWithSaveMessage(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(sceneName);
        }
    }
}