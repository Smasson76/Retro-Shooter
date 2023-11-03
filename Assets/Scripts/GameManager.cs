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

    [Header("-- GAME PROPERTIES --")]
    public int Score = 0;
    public int HighScore;
    public int livesCount = 3;

    [Header("-- GAME OBJECTS --")]
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public GameObject EnemySpawnerObject;
    public GameObject EnemySpawnerInstance;

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
        SimpleMovement simpleMovement = PlayerInstance.GetComponent<SimpleMovement>();
        if (livesCount > 0)
        {
            print(simpleMovement.shield);
            if(simpleMovement.shield == true)
            {
                simpleMovement.shield = false;
            }
            else
            {
                Destroy(PlayerInstance);
                livesCount--;
                UpdateLifeUI();
                SpawnPlayer();
            }
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
