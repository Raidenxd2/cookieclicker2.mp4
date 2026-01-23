using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AssetReferenceT<AudioClip>[] musics;
    [SerializeField] private AssetReferenceT<AudioClip>[] musicsOnline;
    private AsyncOperationHandle<AudioClip> musicHandle;
    private bool overrideSong;
    private int overrideIndex;

    public bool useOnlineMusic;

    private bool stopPlaying;

    private AudioSource musicSource;

    private void Awake()
    {
        instance = this;

        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();

        PlayRandomSongAsync().Forget();
    }

    public void PlayRandomSong()
    {
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

        int musicIndex;

        if (overrideSong)
        {
            musicIndex = overrideIndex;
        }
        else
        {
            if (useOnlineMusic)
            {
                musicIndex = Random.Range(0, musicsOnline.Length);
            }
            else
            {
                musicIndex = Random.Range(0, musics.Length);
            }
        }

        if (useOnlineMusic)
        {
            musicHandle = Addressables.LoadAssetAsync<AudioClip>(musicsOnline[musicIndex]);
        }
        else
        {
            musicHandle = Addressables.LoadAssetAsync<AudioClip>(musics[musicIndex]);
        }
        
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