using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("-- UI OBJECTS --")]
    public Text ScoreText;
    public Text HighScoreText;
    public GameObject StartGameScreen;
    public GameObject GameOverScreen;
    public GameObject GameMenu;
    public GameObject[] livesUICounter;
    public GameObject MultiShotPowerUpImage;
    public GameObject OverchargePowerUpImage;
    public GameObject ExplosivePowerUpImage;

    [Header("-- GAME PROPERTIES --")]
    public int Score = 0;
    public int HighScore;
    public int livesCount = 3;

    [Header("-- GAME OBJECTS --")]
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public GameObject EnemySpawnerObject;
    public GameObject EnemySpawnerInstance;

    [Header("-- PLAYER PROPERTIES --")]
    public bool ocOn;
    public bool multishot;
    public bool xpl;

    void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }

        MainMenu();
        ScoreText.text = "" + Score;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = "" + highScore;
        MultiShotPowerUpImage.SetActive(false);
        OverchargePowerUpImage.SetActive(false);
        ExplosivePowerUpImage.SetActive(false);
        ocOn = false;
        multishot = false;
        xpl = false;
    }

    /*IEnumerator Start() 
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = "" + highScore;
        while (enabled) 
        { 
            yield return new WaitForSeconds(4);
            for (int i = 0; i < 10; i++) 
            {
                int nextEnemyPoint = Random.Range(0, 4);
                Vector3[] points = new Vector3[] 
                {
                    new Vector3(25, 0, 25),
                    new Vector3(25, 0, -25),
                    new Vector3(-25, 0, 25),
                    new Vector3(-25, 0, -25)
                };
                //Spawn enemy here
            }
        }
    }*/

    void Update () 
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
		if (Score > highScore) {
			PlayerPrefs.SetInt("HighScore", Score);
			PlayerPrefs.Save();
			HighScoreText.text = "" + highScore;
        }
    }

    public void MainMenu()
    {
        StartGameScreen.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void PlayerHit()
    {
        if (livesCount > 0)
        {
            Destroy(PlayerInstance);
            livesCount--;
            UpdateLifeUI();
            SpawnPlayer();
        }

        if (livesCount <= 0)
            PlayerDeath();
    }

    private void UpdateLifeUI()
    {
        for (int i = 0; i < livesUICounter.Length; i++)
        {
            if (i < livesCount)
                livesUICounter[i].gameObject.SetActive(true);
            else
                livesUICounter[i].gameObject.SetActive(false);
        }
    }

    public void PlayerDeath()
    {
        Destroy(PlayerInstance);
        Destroy(EnemySpawnerInstance);
        MainMenu();
    }

    public void PowerUpHit(int powerUpIndex)
    {
        SimpleMovement simpleMovementScript = PlayerObject.GetComponent<SimpleMovement>();
        switch (powerUpIndex) 
        {
            case 1:
                simpleMovementScript.multishotTime = 4;
                MultiShotPowerUpImage.SetActive(true);
                multishot = true;
                break;
            case 2:
                OverchargePowerUpImage.SetActive(true);
                break;
            case 3:
                ExplosivePowerUpImage.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void RewardPoint()
    {
        Score++;
        ScoreText.text = "" + Score;
        CameraShake.instance.shakeDuration = 0.04f;
    }

    public void StartOnePlayer()
    {
        Score = 0;
        livesCount = 3;
        ScoreText.text = "" + Score;
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(true);
        UpdateLifeUI();

        SpawnPlayer();
        EnemySpawnerInstance = Instantiate(EnemySpawnerObject, new Vector2(0, 1.5f), Quaternion.identity);
    }

    public void SpawnPlayer()
    {
        PlayerInstance = Instantiate(PlayerObject, new Vector2(0, -3f), Quaternion.identity);
    }

    public void StartTwoPlayer()
    {
        //Future scenario where we will allow for 2 players
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
