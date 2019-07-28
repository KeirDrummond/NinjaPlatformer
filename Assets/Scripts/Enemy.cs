using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject PickUp;
    public GameObject gameController;
    public GameController gameControllerScript;

    public int health = 3;

    public float walkSpeed = 1;

    public float attackInterval = 1;
    public float attackCD = 0;
    public bool canMove;
    public bool alive = true;
    public bool hitStun = false;
    float hitStunCD;

    int direction = 1;

    public Animator animator;
    SpriteRenderer spriteColour;
    Rigidbody2D rb;

    public LayerMask whatIsGround;
    public Transform groundDetector;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteColour = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();
    }

    void Update()
    {
        Cooldowns();
        AttackCheck();
        ChangeDirection();
        GetDirection();
        EnemyMove();
    }

    void EnemyMove()
    {
        if (attackCD == 0 && alive & canMove && !hitStun)
        {
            Vector2 movement = new Vector2(1f * direction, 0);
            transform.position += (Vector3)movement * walkSpeed * Time.deltaTime;
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    void GetDirection()
    {
        direction = (int)transform.localScale.x;
    }

    void ChangeDirection()
    {
        Vector2 groundDetection = new Vector2(groundDetector.position.x, groundDetector.position.y);
        Vector2 wallDetection = new Vector2(groundDetector.position.x, groundDetector.position.y + 1);
        if ((!Physics2D.OverlapPoint(groundDetection, whatIsGround) || Physics2D.OverlapPoint(wallDetection, whatIsGround)) && canMove)
        {
            Vector3 turnAround = transform.localScale;
            turnAround.x *= -1;
            transform.localScale = turnAround;
        }
    }

    public void ReduceHealth()
    {
        if (health > 0)
        {
            health -= 1;
            StartCoroutine(DamageFlash());
            if (health <= 0)
            {
                gameControllerScript.score += 10;
                rb.isKinematic = true;
                Destroy(GetComponent<BoxCollider2D>());
                alive = false;
                animator.SetTrigger("Death");
                Invoke("Death", 0.667f);
            }
            else if (health > 0)
            {
                animator.SetTrigger("Hurt");
                hitStunCD = 0.4f;
                hitStun = true;
                attackCD = 0;
            }
        }
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteColour.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteColour.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Death()
    {
        Vector2 pickUpPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Instantiate(PickUp, pickUpPosition, Quaternion.identity);
        Destroy(gameObject);
    }

    void Cooldowns()
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                attackCD = 0;
            }
        }

        if (hitStun)
        {
            hitStunCD -= Time.deltaTime;
            if (hitStunCD <= 0)
            {
                hitStunCD = 0;
                hitStun = false;
            }
        }
    }

    void AttackCheck()
    {
        if (alive)
        {
            if (attackCD <= 0)
            {
                attackCD = 0;
            }
        }
    }
}
    