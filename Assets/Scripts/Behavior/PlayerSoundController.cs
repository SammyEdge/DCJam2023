using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioClip walk;
    public AudioClip turn;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip hit;
    public AudioClip key;
    public AudioClip bump;
    public AudioClip shift1;
    public AudioClip shift2;
    public AudioClip cooldown;
    public AudioClip grab;
    public AudioClip killBoner;

    public AudioSource sound;

    public AudioClip RandomHit()
    {
        int num = new System.Random().Next(1, 3);

        switch (num)
        {
            case 2:
                return attack2;
            case 3:
                return attack3;
            default:
                return attack1;
        }
    }

    void Start()
    {
        sound = gameObject.transform.GetComponent<AudioSource>();
    }

    public void AttackSound()
    {
        sound.clip = RandomHit();
        sound.Play();
    }

    public void HitWallSound()
    {
        sound.clip = attack3;
        sound.Play();
    }

    public void DamageSound()
    {
        sound.clip = hit;
        sound.Play();
    }

    public void DropKeySound()
    {
        sound.clip = key;
        sound.Play();
    }

    public void BumpSound()
    {
        sound.clip = bump;
        sound.Play();
    }

    public void ShiftSound()
    {
        sound.clip = shift1;
        sound.Play();
    }

    public void UnShiftSound()
    {
        sound.clip = shift2;
        sound.Play();
    }

    public void CooldownSound()
    {
        sound.clip = cooldown;
        sound.Play();
    }

    public void GrabSound()
    {
        sound.clip = grab;
        sound.Play();
    }

    public void KillBonerSound()
    {
        sound.clip = killBoner;
        sound.Play();
    }
}
