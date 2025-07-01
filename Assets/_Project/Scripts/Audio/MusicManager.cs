using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioClip[] musics;
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

            musicSource.clip = musics[musicIndex];
            musicSource.Play();
            StartCoroutine(MusicLoop());
        }
    }
    
    public void PlaySong(int index)
    {
        overrideSong = true;
        overrideIndex = index;
        
        musicSource.Stop();

        isPlaying = false;
    }

    private IEnumerator MusicLoop()
    {
        while (musicSource.isPlaying)
        {
            yield return null;
        }

        isPlaying = false;
    }
}