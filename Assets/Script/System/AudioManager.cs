using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource aS_music, aS_content, aS_camerasnap, aS_choose, aS_dialogue, aS_like, aS_notif, aS_send, aS_hover, aS_event;

    public List<AudioClip> audioClipsContent;
    void Start()
    {
        Instance = this;
    }

    public void SetClip(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void SetMute(AudioSource audioSource, bool isMute = true)
    {
        audioSource.mute = isMute;
    }

    public void PlayHoverAudio()
    {
        StopAudio(aS_hover);
        SetPitchRand(aS_hover);
        aS_hover.Play();
    }

    public void PlaySelectAudio()
    {
        StopAudio(aS_choose);
        SetPitchRand(aS_choose);
        aS_choose.Play();
    }

    public void PlaySnapAudio()
    {
        StopAudio(aS_camerasnap);
        SetPitchRand(aS_camerasnap);
        aS_camerasnap.Play();
    }

    public void PlayDialogAudio()
    {
        StopAudio(aS_dialogue);
        aS_dialogue.Play();
    }

    public void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void SetPitchRand(AudioSource audioSource, float lo = 0.9f, float hi = 1.1f)
    {
        audioSource.pitch = Random.Range(lo, hi);
    }
}
