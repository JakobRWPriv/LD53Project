using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public PlayerController playerController;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            if (playerController.hasKey) {
                playerController.hasKey = false;
                playerController.hasKeyUIObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
