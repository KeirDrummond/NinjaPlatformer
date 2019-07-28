using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    GameObject player;
    Player playerScript;

    public Transform moveRight;
    public Transform moveLeft;

    Vector3 newCameraPos;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
	}
	
	void FixedUpdate ()
    {
	    if (player.transform.position.x > moveRight.position.x && player.transform.position.x < 112)
        {
            Vector2 moveCameraRight = new Vector2(1f, 0);
            transform.position += (Vector3)moveCameraRight * playerScript.moveSpeed * Time.deltaTime;
        }
        else if (player.transform.position.x < moveLeft.position.x)
        {
            Vector2 moveCameraLeft = new Vector2(-1f, 0);
            transform.position += (Vector3)moveCameraLeft * playerScript.moveSpeed * Time.deltaTime;
        }

        
    }
}
