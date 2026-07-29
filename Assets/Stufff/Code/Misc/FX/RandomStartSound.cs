using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartSound : MonoBehaviour
{
    public AudioClip[] clips;
    void Start()
    {
        AudioSource AudioP = GetComponent<AudioSource>();
        AudioClip rnd = clips[UnityEngine.Random.Range(0, clips.Length)];
        AudioP.clip = rnd;

        AudioP.Play();

    }

    private void OnEnable()
    {
        AudioSource AudioP = GetComponent<AudioSource>();
        if (!AudioP.isPlaying)
        {
            AudioClip rnd = clips[UnityEngine.Random.Range(0, clips.Length)];
            AudioP.clip = rnd;
            AudioP.Play();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
