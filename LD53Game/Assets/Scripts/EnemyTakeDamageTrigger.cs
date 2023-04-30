using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageTrigger : MonoBehaviour
{
    public Enemy enemyParent;
    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "PlayerAttack") {
            enemyParent.TakeDamage();
        }
    }
}
