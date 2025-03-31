using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public List<Audioes> AUDIO = new();
    private bool isBGOn = true;

    // Start is called before the first frame update
    private void Awake()
    {
        isBGOn = true;

        foreach (Audioes audio in AUDIO)
        {
            audio.AudioSource.mute = audio.Mute;
            audio.AudioSource.playOnAwake = audio.PlayOnAwake;
            audio.AudioSource.loop = audio.Loop;
            audio.AudioSource.volume = audio.Valume;
            audio.AudioSource.pitch = audio.Pitch;
        }
    }
    public void PlayWalkingAudio()
    {
        if (!AUDIO[0].AudioSource.isPlaying)
        {
            AUDIO[0].AudioSource.Play();
        }
    }

    public void StopWalkingAudio()
    {
        if (AUDIO[0].AudioSource.isPlaying)
        {
            AUDIO[0].AudioSource.Stop();
        }
    }

    public void PlayRunningAudio()
    {
        if (!AUDIO[1].AudioSource.isPlaying)
        {
            AUDIO[1].AudioSource.Play();
        }
    }

    public void StopRunningAudio()
    {
        if (AUDIO[1].AudioSource.isPlaying)
        {
            AUDIO[1].AudioSource.Stop();
        }
    }

    public void PlayPopUpSound()
    {
        if (!AUDIO[2].AudioSource.isPlaying)
        {
            AUDIO[2].AudioSource.Play(); 
        }
    }

    public void PlayButtonPressAudio()
    {
        if (!AUDIO[3].AudioSource.isPlaying)
        {
            AUDIO[3].AudioSource.Play();
        }
    }

    public void NextButtonPressAudio()
    {
        if (!AUDIO[4].AudioSource.isPlaying)
        {
            AUDIO[4].AudioSource.Play();
        }
    }

    public void ItemCollectionAudio()
    {
        if (!AUDIO[5].AudioSource.isPlaying)
        {
            AUDIO[5].AudioSource.Play();
        }
    }

    public void StarsPopUps()
    {
       AUDIO[6].AudioSource.Play();
    }

    public void LevelBTNClicked()
    {
        AUDIO[7].AudioSource.Play();
    }

    public void MusicOnOff()
    {
        isBGOn = !isBGOn; 
        if(isBGOn)
        {
            // Code!!
        }
    }

    public void ChangeInValume(float valume)
    {
        foreach (Audioes audio in AUDIO)
        {
            audio.Valume = valume;
            audio.AudioSource.volume = audio.Valume;
        }
    }
}
