using UnityEngine;
using System.IO;

public class DeleteSave : MonoBehaviour
{
    public Game game;

    public void ForceDeleteSave()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/cookie2");
            game.SavePlayer();
            game.Reload();
        }
        catch
        {
            game.ErrorText.text = "Could not delete save file, please delete manualy at: " + Application.persistentDataPath + "/cookie2";
        }
    }
}