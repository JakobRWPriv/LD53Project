using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : Enemy
{
    public PlayerController player;
    public bool isTutorialEnemy;
    public GameObject tutorialText;
    public bool playerInFront;
    public Collider2D frontCollider;
    public Animator animator;
    public GameObject deathParticles;

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
            TakeDamage();
        }
    }

    public override void TakeDamage() {
        base.TakeDamage();
        player.DashAttackHit();

        if (isTutorialEnemy) {
            tutorialText.SetActive(true);
        }

        AudioHandler.Instance.PlaySound(AudioHandler.Instance.EnemyDie, 0.8f, 1.2f);
        AudioHandler.Instance.PlaySound(AudioHandler.Instance.EnemyDie, 0.8f, 0.8f);
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        Destroy(gameObject);
    }
}
