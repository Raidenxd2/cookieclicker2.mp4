using UnityEngine;
public class SoundManager : MonoBehaviour
{
	public AudioSource EffectsSource;

	public static SoundManager Instance = null;
	
	// Initialize the singleton instance.
	private void Awake()
	{
		//If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
		 	Instance = this;
		}

		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
		 	Destroy(gameObject);
		}
	}

	public void Play(AudioClip clip)
	{
		EffectsSource.clip = clip;
		EffectsSource.Play();
	}
}