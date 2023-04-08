using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSoundController : MonoBehaviour
{
    public AudioClip bump;
    public AudioClip bash;
    public AudioClip destroyed;

    public AudioSource sound;

    void Start()
    {
        sound = gameObject.transform.GetComponent<AudioSource>();
    }

    public void PlayBump()
    {
        sound.clip = bump;
        sound.Play();
    }

    public void PlayDestroyed()
    {
        sound.clip = destroyed;
        sound.Play();
    }
}
