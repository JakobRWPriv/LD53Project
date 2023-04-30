using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    public Collider2D collider2D;
    public PlayerController player;
    public Animator squishAnimator;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "PlayerAttack") {
            TakeDamage();
        }
    }

    public void TakeDamage() {
        player.DashAttackHit();
        squishAnimator.SetTrigger("Squish");

        collider2D.enabled = false;
        StartCoroutine(ToggleCollider());
    }

    IEnumerator ToggleCollider() {
        yield return new WaitForSeconds(0.5f);
        collider2D.enabled = true;
    }
}
