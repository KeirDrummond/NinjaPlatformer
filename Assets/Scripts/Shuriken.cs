using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour {

    public float lifeSpan = 3;
    public float shurikenSpeed = 1;

    bool enemyHit = false;

    GameObject player;
    float direction;

    GameObject enemy;
    Enemy enemyScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        direction = player.transform.localScale.x;
        enemyHit = false;

        Destroy(gameObject, lifeSpan);        
    }

    void Update()   
    {
        if (direction > 0)
        {
            Vector2 moveShuriken = new Vector2(1f * shurikenSpeed * Time.deltaTime, 0);
            transform.position += (Vector3) moveShuriken;
        }
        else if (direction < 0)
        {
            Vector2 moveShuriken = new Vector2(1f * shurikenSpeed * Time.deltaTime, 0);
            transform.position -= (Vector3) moveShuriken;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.tag == "Enemy")
        {
            if (!enemyHit)
            {
                enemyHit = true;
                enemy = other.gameObject;
                enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.ReduceHealth();
            }
        }

        Destroy(gameObject);
    }

}
