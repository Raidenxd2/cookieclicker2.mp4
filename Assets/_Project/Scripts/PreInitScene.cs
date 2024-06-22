using System.Collections;
using LoggerSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PreInitScene : MonoBehaviour
{
    [SerializeField] private Image ProgressBar;

    [SerializeField] private GameObject ErrorImage;

    private IEnumerator Start()
    {
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
        LogSystem.Log("Development build, waiting for 1 second");
        yield return new WaitForSeconds(1f);
        #endif

        AddressableHandles.instance.initSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.initSceneRef, UnityEngine.SceneManagement.LoadSceneMode.Single);

        if (AddressableHandles.instance.initSceneHandle.OperationException != null)
        {
            Debug.LogError("Init scene failed to load.");
            
            ErrorImage.SetActive(true);

            yield break;
        }

        while (!AddressableHandles.instance.initSceneHandle.IsDone)
        {
            ProgressBar.fillAmount = Mathf.Clamp01(AddressableHandles.instance.initSceneHandle.PercentComplete);
            yield return null;
        }
    }
}