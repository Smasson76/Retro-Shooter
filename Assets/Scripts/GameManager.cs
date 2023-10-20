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

    [Header("-- GAME PROPERTIES --")]
    public int Score = 0;
    public bool GameInPlay;

    [Header("-- GAME OBJECTS --")]
    public GameObject PlayerObject;
    public GameObject EnemySpawnerObject;

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

        StartGameScreen.SetActive(true);
        GameMenu.SetActive(false);
        GameInPlay = false;
        ScoreText.text = "" + Score;
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
        /*ScoreText.text = Score + "pts";
		int highScore = PlayerPrefs.GetInt("HighScore", 0);
		if (Score > highScore) 
        {
			PlayerPrefs.SetInt("HighScore", Score);
			PlayerPrefs.Save();
			HighScoreText.text = "" + highScore;
        }*/
    }

    public void RewardPoint()
    {
        Score++;
        ScoreText.text = "" + Score;
        CameraShake.instance.shakeDuration = 0.04f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartOnePlayer()
    {
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(true);
        GameInPlay = true;

        //Spawn enemy and player
        Instantiate(EnemySpawnerObject, new Vector2(-0.1f, 3), Quaternion.identity);
        Instantiate(PlayerObject, new Vector2(0, -3f), Quaternion.identity);
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
