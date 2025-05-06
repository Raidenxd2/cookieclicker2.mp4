using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PreInitScene : MonoBehaviour
{
    [SerializeField] private GameObject ErrorImage;

    private IEnumerator Start()
    {
        if (VRManager.instance.VREnabled)
        {
            VRManager.instance.InitVR();
        }

        AsyncOperationHandle initHandle = LocalizationSettings.InitializationOperation;
        while (!initHandle.IsDone)
        {
            yield return null;
        }

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        AddressableHandles.instance.initSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.initSceneRef, UnityEngine.SceneManagement.LoadSceneMode.Single);

        if (AddressableHandles.instance.initSceneHandle.OperationException != null)
        {
            Debug.LogError("Init scene failed to load.");
            
            ErrorImage.SetActive(true);

            yield break;
        }

        while (!AddressableHandles.instance.initSceneHandle.IsDone)
        {
            yield return null;
        }
    }
}