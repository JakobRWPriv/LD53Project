using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject unlockParticles;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            if (playerController.hasKey) {
                Instantiate(unlockParticles, transform.position, unlockParticles.transform.rotation);
                AudioHandler.Instance.PlaySound(AudioHandler.Instance.KeyUnlock, 0.7f, 1.3f);
                playerController.hasKey = false;
                playerController.hasKeyUIObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
