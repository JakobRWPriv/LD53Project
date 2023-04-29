using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float jumpPower;
    public float jumpPeakTimer;
    public Rigidbody2D rb2d;
    public bool isGrounded;
    bool isFacingRight;
    public Collider2D groundTrigger;
    bool hasLanded;

    public Animator animator;
    public Animator sizeAnimator;

    public float xDirAndFacing;
    public float yDir;

    public SpriteRenderer[] allSprites;
    public GameObject spritePos;

    public int health = 5;
    bool isTakingDamage;
    bool isBlinking;
    bool cannotMove;
    bool isInvincible;

    void Start() {
        allSprites = spritePos.GetComponentsInChildren<SpriteRenderer>(true);
        isFacingRight = true;
        checkTime = Time.time + 2f;
    }

    float timeCheck = 0;
    float checkTime;
    void Update() {
        timeCheck+=Time.deltaTime;


        if (Time.time >= checkTime) {
            print("Time: " + timeCheck);
            checkTime = 999999;
        }

        isGrounded = (Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground")) && rb2d.velocity.y < 0.01f && rb2d.velocity.y > -0.01f);

        if (!isGrounded && hasLanded) {
            hasLanded = false;
        }

        if (isGrounded && !hasLanded) {
            hasLanded = true;
            sizeAnimator.SetTrigger("BounceDown");
            //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerLand, 0.6f, Random.Range(0.95f, 1.2f));
        }

        if (transform.localScale.x > 0 && !isFacingRight)
            isFacingRight = true;
        else if (transform.localScale.x < 0 && isFacingRight)
            isFacingRight = false;

        //if (jumpPeakTimer > 0.4f) {

        //}

        Jump();
        FlipPlayer();
    }

    private void Jump() {
        if (cannotMove) return;

        if (Input.GetKeyDown(KeyCode.Z)) {
            if (isGrounded) {
                rb2d.velocity += new Vector2(0, jumpPower);
                sizeAnimator.SetTrigger("JumpBounce");
                //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerJump, 0.6f, Random.Range(0.7f, 0.75f));
            }
        }

        if (Input.GetKeyUp(KeyCode.Z)) {
            if (rb2d.velocity.y > 0)
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    private void FlipPlayer() {
        if ((Input.GetKey(KeyCode.RightArrow) && transform.localScale.x < 0) ||
                    (Input.GetKey(KeyCode.LeftArrow) && transform.localScale.x > 0)) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate() {
        AddFriction();
        
        if (cannotMove) return;

        if (Input.GetKey(KeyCode.LeftArrow)) {
            float accel = acceleration;
            if (rb2d.velocity.x > 0) {
                accel = acceleration * 2f;
            }
            if (rb2d.velocity.x > -maxSpeed) {
                rb2d.AddForce(-Vector3.right * accel);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            float accel = acceleration;
            if (rb2d.velocity.x < 0) {
                accel = acceleration * 2f;
            }
            if (rb2d.velocity.x < maxSpeed) {
                rb2d.AddForce(Vector3.right * accel);
            }
        }

        if (!isGrounded && rb2d.velocity.y > 0) {
            jumpPeakTimer += 1 * Time.fixedDeltaTime;
        }

        animator.SetBool("IsRunning", (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)));
        animator.SetBool("IsJumping", !isGrounded);
    }

    private void AddFriction() {
        if (cannotMove) return;

        if (!(Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.LeftArrow))) {
            if (isGrounded)
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.75f, rb2d.velocity.y);
            else 
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.9f, rb2d.velocity.y);

            if (isGrounded) {
                if (isFacingRight && rb2d.velocity.x < 0.1f) {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                } else if (!isFacingRight && rb2d.velocity.x > -0.1f) {
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                }
            }
        } else if ((Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.LeftArrow))) {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 1.2f, rb2d.velocity.y);
        }
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Enemy") {
            if (!isInvincible) {
                //TakeDamage(1, otherCollider.transform.position.x < transform.position.x);
                isInvincible = true;
                //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerDamaged);
            }
        }
    }
}
