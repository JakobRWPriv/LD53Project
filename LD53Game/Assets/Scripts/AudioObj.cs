using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObj : MonoBehaviour {

    AudioSource source;
    public bool isMenuSound;
    public float destroyCountdown;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound, float volume = 1, float pitch = 1)
    {
        if (isMenuSound)
            source.ignoreListenerPause = true;
        source.volume = volume;
        source.pitch = pitch;
        source.clip = sound;
        source.Play();
        destroyCountdown = sound.length*100;
    }

    public void PlaySoundDelayed(float delay, AudioClip sound, float volume = 1, float pitch = 1) {
        StartCoroutine(PlaySoundDelayedCo(delay, sound, volume, pitch));
    }

    public IEnumerator PlaySoundDelayedCo(float delay, AudioClip sound, float volume = 1, float pitch = 1) {
        if (isMenuSound)
            source.ignoreListenerPause = true;
        source.volume = volume;
        source.pitch = pitch;
        source.clip = sound;

        yield return new WaitForSeconds(delay);

        source.Play();
        destroyCountdown = sound.length*100;
    }

    private void FixedUpdate()
    {
        if (!AudioListener.pause && !isMenuSound)
            destroyCountdown--;
        else if (isMenuSound)
            Destroy(gameObject);

        if (destroyCountdown < 1)
            Destroy(gameObject);
    }
}
