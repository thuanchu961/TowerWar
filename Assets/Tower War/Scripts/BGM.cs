using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioClips = Resources.LoadAll<AudioClip>("BGM");
        
    }

    private AudioClip GetAudioClips()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetAudioClips();
            audioSource.Play();
        }
    }
}
