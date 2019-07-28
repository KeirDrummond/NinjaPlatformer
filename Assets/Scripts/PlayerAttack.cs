using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    GameObject player;
    Player playerScript;

    GameObject enemy;
    Enemy enemyScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (playerScript.swordActive)
        {
            if (collision.tag == "Enemy")
            {
                if (!playerScript.enemyHit)
                {
                    playerScript.enemyHit = true;
                    enemy = collision.gameObject;
                    enemyScript = enemy.GetComponent<Enemy>();

                    enemyScript.ReduceHealth();
                }
            }
        }
        
    }

}
