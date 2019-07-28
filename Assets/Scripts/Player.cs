using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int moveSpeed = 1;
    public int jumpHeight = 1;
    public int noOfShurikens = 0;

    public int playerHealth = 3;
    public bool alive = true;

    public GameObject shurikenPrefab;

    bool grounded = true;

    bool airJump = false;
    bool canMove = true;
    bool touchedGround = true;
    bool inControl = true;

    bool groundJump;
    float groundJumpTimer;
    public float groundJumpTimerBase = 0.1f;

    bool canAttack = true;
    int comboCounter = 0;
    float attackCD = 0;
    float attackWindow = 0;
    bool reduceWindow = false;

    public bool swordActive = false;
    float attackDuration = 0.5f;
    float remainingAttack;

    public bool enemyHit = false;
    int knockBackDirection = 1;
    bool hitStun;
    float hitStunCD;
    public bool invincibile = false;

    public Transform feet;
    public LayerMask whatIsGround;

    public Transform throwFrom;

    Animator animator;
    Rigidbody2D rb;
    Collider2D playerCollider;
    SpriteRenderer spriteColour;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteColour = GetComponent<SpriteRenderer>();
        animator.SetBool("Alive", true);
    }

    void Update()
    {
        if (transform.position.x > 116)
        {
            if (transform.position.x < 128)
            {
                Vector3 turnRight = transform.localScale;
                turnRight.x = 1;
                transform.localScale = turnRight;
                Vector2 moveRight = new Vector2(moveSpeed, rb.velocity.y);
                rb.velocity = moveRight;
            }

            if (grounded)
            {
                animator.SetBool("Running", true);
            }
        }
        else
        {
            if (hitStun)
            {
                if (grounded && playerHealth > 0 & hitStunCD == 0)
                {
                    inControl = true;
                    hitStun = false;
                }
            }

            if (alive && inControl)
            {
                Controls();
            }

            if (rb.velocity.y < -15)
            {
                rb.velocity = new Vector2(rb.velocity.x, -15f);
            }
        }
        AttackCheck();
        Cooldowns();
    }

    private void LateUpdate()
    {
        CheckGrounded();

        if (grounded)
        {
            groundJump = true;
            groundJumpTimer = groundJumpTimerBase;
            airJump = true;
            touchedGround = true;
            if (hitStun)
            {
                rb.velocity = Vector2.zero;
            }
            animator.SetBool("Grounded", true);            
        }
        else
        {
            if (groundJumpTimer != 0)
            {
                groundJump = true;
            }
            else if (groundJumpTimer == 0)
            {
                groundJump = false;
            }

            animator.SetBool("Grounded", false);
        }
    }

    private void CheckGrounded()
    {
        grounded = false;
        if (rb.velocity.y == 0)
        {
            float playerWidth = playerCollider.bounds.size.x;

            Vector2 leftSide = new Vector2(playerCollider.transform.position.x - playerWidth / 2, feet.transform.position.y);
            Vector2 rightSide = new Vector2(playerCollider.transform.position.x + playerWidth / 2, feet.transform.position.y);
            grounded = Physics2D.OverlapArea(leftSide, rightSide, whatIsGround);
        }
    }

    void Controls()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (canMove)
            {
                Vector3 turnRight = transform.localScale;
                turnRight.x = 1;
                transform.localScale = turnRight;
                Vector2 moveRight = new Vector2(moveSpeed, rb.velocity.y);
                rb.velocity = moveRight;

                if (grounded)
                {
                    animator.SetBool("Running", true);
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (canMove)
            {
                Vector3 turnLeft = transform.localScale;
                turnLeft.x = -1;
                transform.localScale = turnLeft;
                Vector2 moveLeft = new Vector2(-moveSpeed, rb.velocity.y);
                rb.velocity = moveLeft;

                if (grounded)
                {
                    animator.SetBool("Running", true);
                }
            }
        }
        else
        {
            if (canMove)
            {

                Vector2 dontMove = new Vector2(0, rb.velocity.y);
                rb.velocity = dontMove;

                if (grounded)
                {
                    animator.SetBool("Running", false);
                }
            }

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (groundJump || airJump)
            {
                if (!groundJump)
                {
                    airJump = false;
                    touchedGround = true;
                }

                groundJumpTimer = 0;
                attackCD = 0;
                comboCounter = 0;
                attackWindow = 0.5f;
                reduceWindow = false;

                Vector2 jump = new Vector2(0, jumpHeight);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(jump, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                canMove = true;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            }
        }

        if (Input.GetButtonDown("Attack") && canAttack)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                Vector3 turnRight = transform.localScale;
                turnRight.x = 1;
                transform.localScale = turnRight;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                Vector3 turnLeft = transform.localScale;
                turnLeft.x = -1;
                transform.localScale = turnLeft;
            }
            comboCounter++;
            attackWindow = 0.5f;
            reduceWindow = true;

            remainingAttack = attackDuration;
            swordActive = true;
            enemyHit = false;
            if (comboCounter == 1)
            {
                animator.SetTrigger("NewCombo");
            }
            animator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Throw"))
        {
            if (noOfShurikens > 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    Vector3 turnRight = transform.localScale;
                    turnRight.x = 1;
                    transform.localScale = turnRight;
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    Vector3 turnLeft = transform.localScale;
                    turnLeft.x = -1;
                    transform.localScale = turnLeft;
                }

                comboCounter = 3;

                Vector2 shurikenSpawn = throwFrom.position;
                Instantiate(shurikenPrefab, shurikenSpawn, Quaternion.identity);
                noOfShurikens -= 1;

                attackWindow = 0.5f;
                reduceWindow = true;
                animator.SetTrigger("Throw");
            }
        }
    }

    public void ReduceHealth(float enemyPosition)
    {
        if (playerHealth > 0 && !invincibile)
        {
            playerHealth -= 1;
            animator.SetTrigger("Hurt");
            StartCoroutine(DamageFlash());
            inControl = false;

            attackCD = 0;
            comboCounter = 0;
            attackWindow = 0.5f;
            reduceWindow = false;
            canMove = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            knockBackDirection = 1;
            if (transform.position.x < enemyPosition)
            {
                knockBackDirection = -1;
            }

            hitStun = true;
            hitStunCD = 0.5f;
            Vector3 direction = transform.localScale;
            direction.x = knockBackDirection * -1;
            transform.localScale = direction;
            Vector2 knockUp = new Vector2(0, 15f);
            rb.velocity = Vector2.zero;
            rb.AddForce(knockUp, ForceMode2D.Impulse);
            Vector2 knockBack = new Vector2(moveSpeed * knockBackDirection, rb.velocity.y);
            rb.velocity = knockBack;
            transform.position += new Vector3(0, 0.1f);
            grounded = false;
        }
        else if (playerHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator DamageFlash()
    {
        invincibile = true;
        if (playerHealth <= 0)
        {
            animator.SetBool("Alive", false);
        }

        for (int i = 0; i < 5; i++)
        {
            spriteColour.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteColour.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        invincibile = false;
        if (playerHealth <= 0)
        {
            alive = false;
            gameObject.SetActive(false);
        }
    }

    void Cooldowns()
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                comboCounter = 0;
                attackCD = 0;
            }
        }

        if (attackWindow > 0 && reduceWindow)
        {
            attackWindow -= Time.deltaTime;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            canMove = false;
        }
        else if (attackWindow <= 0 && reduceWindow)
        {
            attackCD = 0.2f;
            touchedGround = false;
            attackWindow = 0.5f;
            reduceWindow = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canMove = true;
        }

        if (swordActive)
        {
            remainingAttack -= Time.deltaTime;
            if (remainingAttack <= 0)
            {
                remainingAttack = 0;
                swordActive = false;
            }
        }

        if (hitStun)
        {
            hitStunCD -= Time.deltaTime;
            if (hitStunCD <= 0)
            {
                hitStunCD = 0;
            }
        }
        if (!grounded && groundJumpTimer != 0)
        {
            groundJumpTimer -= Time.deltaTime;
            if (groundJumpTimer <= 0)
            {
                groundJumpTimer = 0;
                groundJump = false;
            }
        }
    }

    void AttackCheck()
    {
        canAttack = true;

        if (attackCD != 0)
        {
            canAttack = false;
        }
        else if (comboCounter == 3)
        {
            canAttack = false;
        }
        else if (!touchedGround)
        {
            canAttack = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallOff")
        {
            playerHealth = 0;
            ReduceHealth(transform.position.x);
        }
    }

}

