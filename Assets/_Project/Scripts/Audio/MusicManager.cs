using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AssetReferenceT<AudioClip>[] musics;
    private AsyncOperationHandle<AudioClip> musicHandle;
    private bool overrideSong;
    private int overrideIndex;

    private bool stopPlaying;

    private AudioSource musicSource;

    private void Awake()
    {
        instance = this;

        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();

        PlayRandomSongAsync().Forget();
    }

    private async UniTaskVoid PlayRandomSongAsync()
    {
        if (stopPlaying)
        {
            return;
        }

        if (musicHandle.IsValid())
        {
            Addressables.Release(musicHandle);
        }

        int musicIndex = 0;

        if (overrideSong)
        {
            musicIndex = overrideIndex;
        }
        else
        {
            musicIndex = Random.Range(0, musics.Length);
        }

        musicHandle = Addressables.LoadAssetAsync<AudioClip>(musics[musicIndex]);
        await musicHandle;

        musicSource.clip = musicHandle.Result;
        musicSource.Play();

        await UniTask.WaitUntil(() => musicSource.isPlaying == false);
        PlayRandomSongAsync().Forget();
    }
    
    public void PlaySong(int index)
    {
        overrideSong = true;
        overrideIndex = index;
        
        musicSource.Stop();
    }

    public void UnloadSong()
    {
        stopPlaying = true;

        musicSource.Stop();

        Addressables.Release(musicHandle);
    }
}