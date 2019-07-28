using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject player;
    Player playerScript;

    bool isPlayerAlive = true;
    float restartDelay = 2f;
    public int score;
    float currentTime;

    int timeBonus;
    int endScore;

    Camera mainCamera;

    BoxCollider2D triggerBox;

    public Text playerHP;
    public Text playerAmmo;
    public Text timer;
    public Text scoreText;
    public Text endScoreText;
    public Text restartText;
    public Text rankingText;

    void Awake()
    {
        Instantiate(player, new Vector2(-2, -4.041319f), Quaternion.identity);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        mainCamera = Camera.main;
        triggerBox = gameObject.GetComponent<BoxCollider2D>();
        score = 0;
        currentTime = 0;

        endScoreText.enabled = false;
        restartText.enabled = false;
        rankingText.enabled = false;
    }

    void FixedUpdate()
    {
        if (!player.activeSelf)
        {
            isPlayerAlive = false;
        }
        if (Input.GetButtonDown("Submit"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
        DisplayText();
        GetCameraArea();
        RestartDelay();
    }

    void DisplayText()
    {
        playerHP.text = "Health points: " + playerScript.playerHealth;
        playerAmmo.text = "Shurikens: " + playerScript.noOfShurikens;

        if (player.transform.position.x < 116)
        {
            currentTime += Time.deltaTime;
            float mins = currentTime / 60;
            float secs = currentTime % 60;
            string timerString = string.Format("{0}:{1}", mins.ToString("00"), secs.ToString("00.00"));
            timer.text = "Time: " + timerString;
            scoreText.text = "Score: " + score.ToString("0000");
        }
        else
        {
            endScoreText.enabled = true;
            restartText.enabled = true;
            if (currentTime < 300)
            {
                timeBonus = 300 - (int)currentTime;
            }
            else
            {
                timeBonus = 0;
            }
            endScore = timeBonus + score;
            endScoreText.text = "Final Score: " + endScore;
            rankingText.enabled = true;
            if (endScore >= 355)
            {
                rankingText.text = "Platinum";
            }
            else if (endScore >= 340)
            {
                rankingText.text = "Gold";
            }
            else if (endScore >= 300)
            {
                rankingText.text = "Silver";
            }
            else
            {
                rankingText.text = "Bronze";
            }            
        }
    }

    void GetCameraArea()
    {
        triggerBox.transform.position = mainCamera.transform.position;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        Vector2 cameraSize = new Vector2(cameraWidth * 1.2f, cameraHeight);
        triggerBox.size = cameraSize;
    }

    void RestartDelay()
    {
        if (!isPlayerAlive)
        {
            restartDelay -= Time.deltaTime;
            if (restartDelay <= 0)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentScene);
            }
        }
    }
}
