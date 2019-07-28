using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject enemy;

    bool activated = false;
    public int startDirection = 1;
    public bool canMove;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "GameController" && !activated)
        {
            GameObject newObject = Instantiate(enemy, transform.position, Quaternion.identity) as GameObject;
            Enemy enemyScript = newObject.GetComponent<Enemy>();
            enemyScript.canMove = canMove;
            newObject.transform.localScale = new Vector3(startDirection, 1, 1);
            activated = true;
        }
    }

}
