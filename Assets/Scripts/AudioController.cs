using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    public static AudioController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();        
    }    

    public void PlayAudioClip(int clipID)
    {
        audioSource.PlayOneShot(audioClips[clipID]);
    }
}
