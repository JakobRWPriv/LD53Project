using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTrigger : MonoBehaviour
{
    public MovingPlatform movingPlatform;

    public bool contactWithPlayer;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            contactWithPlayer = true;
            if (movingPlatform.waitsForPlayerBeforeStarting) {
                StartCoroutine(StartDelay());
            }
        }
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            contactWithPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            contactWithPlayer = false;
        }
    }

    IEnumerator StartDelay() {
        yield return new WaitForSeconds(0.1f);
        movingPlatform.waitsForPlayerBeforeStarting = false;
        movingPlatform.canMove = true;
    }
}
