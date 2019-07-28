using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    GameObject player;
    Player playerScript;

    GameObject enemy;
    Enemy enemyScript;

    bool detected = false;
    bool attacked;
    float waitTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        enemy = transform.parent.gameObject;
        enemyScript = enemy.GetComponent<Enemy>();
    }

    void Update()
    {
        if (detected)
        {
            if (enemyScript.attackCD <= 0 && !enemyScript.hitStun && !attacked)
            {
                attacked = true;
                waitTime = 0.1f;
                enemyScript.animator.SetTrigger("Attack");
                enemyScript.attackCD = enemyScript.attackInterval;
            }
        }

        if (attacked)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                waitTime = 0;
                if (detected && !enemyScript.hitStun && enemyScript.alive)
                {
                    playerScript.ReduceHealth(enemy.transform.position.x);
                }
                attacked = false;             
            }
        }

        if (enemyScript.hitStun)
        {
            attacked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && enemyScript.alive)
        {
            detected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            detected = false;
        }
    }

}
