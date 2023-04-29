using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    public PlayerController player;
    public bool playerInFront;
    public Collider2D frontCollider;
    public Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        playerInFront = (Physics2D.IsTouchingLayers(frontCollider, 1 << LayerMask.NameToLayer("PlayerTrigger")));

        if (playerInFront) {
            AttackFront();
        }
    }

    void AttackFront() {
        frontCollider.enabled = false;
        StartCoroutine(AttackFrontCo());
    }

    IEnumerator AttackFrontCo() {
        animator.SetTrigger("AttackForward");

        yield return new WaitForSeconds(4f);
        
        frontCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "PlayerAttack") {
            player.DashAttackHit();
            Destroy(gameObject);
        }
    }
}
