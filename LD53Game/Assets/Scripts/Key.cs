using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public PlayerController player;
    public GameObject keyGetParticles;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            Instantiate(keyGetParticles, transform.position, keyGetParticles.transform.rotation);
            AudioHandler.Instance.PlaySound(AudioHandler.Instance.KeyGet, 0.8f, 1.2f);
            player.hasKey = true;
            player.hasKeyUIObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
