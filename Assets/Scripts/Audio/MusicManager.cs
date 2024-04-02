using System.Collections;
using LoggerSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AssetReferenceT<AudioClip>[] musics;
    private AsyncOperationHandle<AudioClip> handle;
    private float musicDuration;
    private bool overrideSong;
    private int overrideIndex;

    private AudioSource musicSource;

    private bool isPlaying;

    private void Awake()
    {
        instance = this;

        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            
            int musicIndex = 0;

            if (overrideSong)
            {
                musicIndex = overrideIndex;
            }
            else
            {
                musicIndex = Random.Range(0, musics.Length);
            }

            handle = Addressables.LoadAssetAsync<AudioClip>(musics[musicIndex]);
            handle.Completed += Handle_Completed;
        }
    }

    private void Handle_Completed(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            musicSource.clip = obj.Result;
            musicDuration = obj.Result.length;
            musicSource.Play();
            StartCoroutine(MusicLoop());
        }
        else
        {
            LogSystem.Log("AssetReference failed to load.", LogTypes.Error);
        }
    }

    public void PlaySong(int index)
    {
        overrideSong = true;
        overrideIndex = index;

        Addressables.Release(handle);
        
        musicSource.Stop();

        isPlaying = false;
    }

    private IEnumerator MusicLoop()
    {
        while (musicSource.isPlaying)
        {
            yield return null;
        }
        
        Addressables.Release(handle);
        isPlaying = false;
    }
}