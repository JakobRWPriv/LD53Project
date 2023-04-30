using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public LevelManager levelManager;
    public Transform cameraTransform;
    Vector2 startPos;
    public float acceleration;
    public float maxSpeed;
    public float jumpPower;
    public float jumpPeakTimer;
    public Rigidbody2D rb2d;
    public bool isGrounded;
    public bool isFacingRight;
    public Collider2D groundTrigger;
    bool hasLanded;
    public bool hasDied;

    public bool canDash = true;
    public bool isDashing;
    public bool haltDash;
    Coroutine dashCoroutine;
    public Collider2D dashAttackCollider;
    public bool hasHitDashAttack;

    public SpriteRenderer markSprite;

    public Transform leafTransformLeft;
    public Transform leafTransformRight;

    public Animator animator;
    public Animator squishAnimator;

    public float xDirAndFacing;
    public float yDir;

    public SpriteRenderer[] allSprites;

    bool cannotMove;

    public Animator screenWipeAnimator;
    bool isLoadingLevel;

    void Start() {
        startPos = transform.position;
        //allSprites = spritePos.GetComponentsInChildren<SpriteRenderer>(true);
        isFacingRight = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (!isGrounded && hasLanded) {
            hasLanded = false;
        }

        if (isGrounded && !hasLanded) {
            hasLanded = true;
            canDash = true;
            squishAnimator.SetTrigger("SquishDown");
            //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerLand, 0.6f, Random.Range(0.95f, 1.2f));
        }

        if (!isDashing && !cannotMove) {
            if (transform.localScale.x > 0 && !isFacingRight)
                isFacingRight = true;
            else if (transform.localScale.x < 0 && isFacingRight)
                isFacingRight = false;
        }

        Jump();
        Dash();
        FlipPlayer();
    }

    private void Jump() {
        if (Input.GetKey(KeyCode.U)) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 10);
        }
        if (cannotMove) return;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Y)) {
            if (isGrounded) {
                rb2d.velocity += new Vector2(0, jumpPower);
                squishAnimator.SetTrigger("SquishJump");
                //AudioHandler.Instance.PlaySound(AudioHandler.Instance.playerJump, 0.6f, Random.Range(0.7f, 0.75f));
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Y)) {
            if (rb2d.velocity.y > 0 && !hasHitDashAttack)
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    private void Dash() {
        if (!canDash) return;

        if (Input.GetKeyDown(KeyCode.X)) {
            dashAttackCollider.enabled = true;
            squishAnimator.SetTrigger("SquishDashStart");
            canDash = false;
            isDashing = true;
            dashCoroutine = StartCoroutine(DashCo());
        }
    }

    IEnumerator DashCo() {
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;

        if (isFacingRight) {
            rb2d.velocity = new Vector2(30f, 0);
        } else {
            rb2d.velocity = new Vector2(-30f, 0);
        }

        yield return new WaitForSeconds(0.1f);

        haltDash = true;

        yield return new WaitForSeconds(0.3f);

        haltDash = false;
        rb2d.velocity = Vector2.zero;
        jumpPeakTimer = 0;
        rb2d.gravityScale = 3;
        isDashing = false;
        squishAnimator.SetTrigger("SquishJump");
        dashAttackCollider.enabled = false;
        
        if (isGrounded) {
            yield return new WaitForSeconds(0.2f);
            canDash = true;
        }

        dashCoroutine = null;
    }

    public void DashAttackHit() {
        StopCoroutine(dashCoroutine);
        hasHitDashAttack = true;
        StartCoroutine(DashAttackHitCo());
    }

    IEnumerator DashAttackHitCo() {
        haltDash = false;
        isDashing = false;
        canDash = false;
        rb2d.velocity = Vector2.zero;
        jumpPeakTimer = 0;
        rb2d.gravityScale = 3;
        squishAnimator.SetTrigger("SquishJump");
        dashAttackCollider.enabled = false;

        rb2d.velocity += new Vector2(0, jumpPower);
        yield return new WaitForSeconds(0.1f);
        canDash = true;
        yield return new WaitForSeconds(1f);
        hasHitDashAttack = false;
    }

    private void FlipPlayer() {
        if (isDashing || cannotMove) return;

        if ((Input.GetKey(KeyCode.RightArrow) && transform.localScale.x < 0)) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            markSprite.flipX = true;
            leafTransformLeft.localScale = new Vector3(1, 1, 1);
            leafTransformRight.localScale = new Vector3(0.6f, 0.6f, 1);
        }

        
        if ((Input.GetKey(KeyCode.LeftArrow) && transform.localScale.x > 0)) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            markSprite.flipX = false;
            leafTransformLeft.localScale = new Vector3(0.6f, 0.6f, 1);
            leafTransformRight.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            markSprite.flipX = false;
            leafTransformLeft.localScale = new Vector3(0.6f, 0.6f, 1);
            leafTransformRight.localScale = new Vector3(1, 1, 1);
        }
    }

    void FixedUpdate() {
        AddFriction();
        
        if (cannotMove) return;

        isGrounded = (Physics2D.IsTouchingLayers(groundTrigger, 1 << LayerMask.NameToLayer("Ground")) && rb2d.velocity.y < 0.01f && rb2d.velocity.y > -0.01f);

        if (!isDashing) {
            if (Input.GetKey(KeyCode.LeftArrow) ) {
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
                jumpPeakTimer += Time.fixedDeltaTime;
            }

            if (jumpPeakTimer > 0.3f && !Input.GetKey(KeyCode.U)) {
                rb2d.gravityScale = 1f;
            }

            if (isGrounded || rb2d.velocity.y < 0) {
                jumpPeakTimer = 0;
                rb2d.gravityScale = 3f;
            }
        }
            
        if (haltDash) {
            rb2d.velocity = new Vector2(rb2d.velocity.x * 0.8f, 0);
        }
        
        animator.SetBool("IsRunning", (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)));
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsDashing", isDashing);
    }

    private void AddFriction() {
        if (cannotMove || isDashing) return;

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

    MovingPlatformTrigger mpt;
    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Enemy") {
            if (!isDashing && !hasDied) {
                Die();
            }
        }
        if (otherCollider.tag == "EnemyInstaKill") {
            if (!hasDied) {
                Die();
            }
        }
        if (otherCollider.tag == "MovingPlatformTrigger") {
            mpt = otherCollider.GetComponent<MovingPlatformTrigger>();
            if (isGrounded) {
                transform.parent = mpt.movingPlatform.transform;
            } else {
                transform.parent = null;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) && isGrounded && !isLoadingLevel && otherCollider.tag == "DoorGoal0") {
            isLoadingLevel = true;
            StartCoroutine(LoadLevel(1));
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider) {
        if (otherCollider.tag == "MovingPlatformTrigger") {
            transform.parent = null;
        }
    }

    public void Die() {
        hasDied = true;
        cannotMove = true;
        rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero;
        StartCoroutine(DieCo());
    }

    IEnumerator DieCo() {
        screenWipeAnimator.SetTrigger("ScreenWipeClose");
        yield return new WaitForSeconds(0.8f);
        if (levelManager.checkpoint != null) {
            transform.position = levelManager.checkpoint.transform.position;
            cameraTransform.position = levelManager.checkpoint.transform.position;
        } else {
            transform.position = startPos;
            cameraTransform.position = startPos;
        }
        yield return new WaitForSeconds(0.5f);
        screenWipeAnimator.SetTrigger("ScreenWipeOpen");
        hasDied = false;
        cannotMove = false;
        rb2d.gravityScale = 3;
    }

    IEnumerator LoadLevel(int levelToLoad) {
        screenWipeAnimator.SetTrigger("ScreenWipeClose");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(levelToLoad);
    }
}
