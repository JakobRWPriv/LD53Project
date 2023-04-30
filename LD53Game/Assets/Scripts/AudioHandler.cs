using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour {

    [Header("Audio sources & Audio object")]
    public GameObject audioObj;
    public Transform audioObjParent;

    [Space]
    [Header("Player")]
    public AudioClip Grab;
    public AudioClip GrabSuccess;
    public AudioClip Throw;
    public AudioClip Dash;
    public AudioClip IntoWheelbarrow;

    // AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD 
    // AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD 
    // AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD 
    // AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD AUDIO OBJ DÄR NERE UTKOMMENTERAD 

    [Space]
    [Header("Farmer")]
    public AudioClip FarmerStep;

    [Space]
    [Header("Enemy")]
    public AudioClip Activate;
    public AudioClip Shoot;
    public AudioClip DropHit;
    public AudioClip Die;
    public AudioClip Die2;

    
    #region Singleton
    private static AudioHandler _instance;

    public static AudioHandler Instance { get { return _instance; } }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        AudioListener.pause = false;

    }
    void OnDestroy()
    {
        if (this == _instance)
        { _instance = null; }
    }
    #endregion

    public void PlaySound(AudioClip sound, float volume = 1, float pitch = 1, float delay = 0)
    {
        GameObject InstantiatedAudioObj = Instantiate(audioObj);
        InstantiatedAudioObj.transform.SetParent(audioObjParent);
        if (delay == 0) {
            //InstantiatedAudioObj.GetComponent<AudioObj>().PlaySound(sound, volume, pitch);
        } else {
            //InstantiatedAudioObj.GetComponent<AudioObj>().PlaySoundDelayed(delay, sound, volume, pitch);
        }
    }
}
