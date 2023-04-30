using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public PlayerController player;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            player.hasKey = true;
            player.hasKeyUIObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
