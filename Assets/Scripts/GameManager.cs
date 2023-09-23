using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject StartGameScreen;
    public GameObject GameOverScreen;

    public int Score = 0;
    public bool GameInPlay;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }

        StartGameScreen.SetActive(true);
        GameInPlay = false;
      }

    IEnumerator Start() 
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
    }

    void Update () 
    {
        ScoreText.text = Score + "pts";
		int highScore = PlayerPrefs.GetInt("HighScore", 0);
		if (Score > highScore) 
        {
			PlayerPrefs.SetInt("HighScore", Score);
			PlayerPrefs.Save();
			HighScoreText.text = "" + highScore;
        }
    }

    public void StartGame() 
    {
        StartGameScreen.SetActive(false);
        GameInPlay = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
