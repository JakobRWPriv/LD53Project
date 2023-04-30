using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectMethods : MonoBehaviour
{
    public AudioClip stepSound;

    public void PlayStepSound() {
        AudioHandler.Instance.PlaySound(stepSound, 0.4f, Random.Range(1.2f, 1.3f));
    }
}
