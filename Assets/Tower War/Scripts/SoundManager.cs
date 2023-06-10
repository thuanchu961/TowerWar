using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public GameObject prefab;

    public AudioClip[] audioClips;
    private AudioSource[] audioSources;

    private void Awake()
    {
        instance = this;
        audioSources = new AudioSource[6];
    }

    public void PlaySound(TypeSound typeSound, float volume, bool isLoopback)
    {
        if (PlayerPrefs.GetInt("AllowSound") == 0)
        {
            Play(audioClips[(int)typeSound], ref audioSources[(int)typeSound], volume, isLoopback);
        }
    }

    private void Play(AudioClip clip, ref AudioSource audioSource, float volume, bool isLoopback)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            return;
        }
        audioSource = Instantiate(instance.prefab).GetComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.loop = isLoopback;
        audioSource.clip = clip;
        audioSource.Play();

        if (!isLoopback)
        {
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
    }

    public void StopSound(TypeSound typeSound)
    {
        if (audioSources[(int)typeSound] != null)
        {
            audioSources[(int)typeSound].Stop();
        }
    }
}

public enum TypeSound
{
    Attack = 0,
    Background = 1,
    DieBoss = 2,
    DieChar = 3,
    Goldcoin = 4,
    Youlost = 5
}
