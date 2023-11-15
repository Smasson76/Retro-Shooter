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
    public GameObject CreditsScreen;
    public GameObject GameMenu;
    public GameObject[] livesUICounter;
    public GameObject OverchargePowerUpImage;
    public GameObject ExplosivePowerUpImage;
    public GameObject MultiShotPowerUpImage;

    [Header("-- GAME PROPERTIES --")]
    public int Score = 0;
    public int HighScore;
    public int livesCount = 3;
    public int enemyCount = 0;
    public int powerUpCount = 0;
    

    [Header("-- GAME OBJECTS --")]
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public GameObject EnemySpawnerObject;
    public GameObject EnemySpawnerInstance;
    public Rigidbody2D OverChargePrefab;
    public Rigidbody2D ExplosivePrefab;
    public Rigidbody2D MultishotPrefab;
    public Rigidbody2D powerUpPrefabClone;
    public Rigidbody2D powerUpPrefabClone2;
    public Rigidbody2D powerUpPrefabClone3;
	  public Parallax ParallaxBackgroundObject;
	  public Parallax ParallaxBackgroundInstance;

    [Header("-- PLAYER PROPERTIES --")]
    public bool ocOn;
    public bool multishot;
    public bool xpl;

	void Start()
	{
	}

    void Awake() 
    {
        ParallaxBackgroundInstance = Instantiate(
			ParallaxBackgroundObject,
			new Vector2(0, 0),
			Quaternion.identity
		);

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
        Cursor.visible = true;
        StartGameScreen.SetActive(true);
        GameMenu.SetActive(false);
        GameOverScreen.SetActive(false);
        CreditsScreen.SetActive(false);
		ParallaxBackgroundInstance.goSlow();
    }

    public void CreditsMenu()
    {
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(false);
        GameOverScreen.SetActive(false);
        CreditsScreen.SetActive(true);
    }

    public void PlayerHit()
    {
        if (livesCount > 1)
        {
			musicManager.Instance.playSound("entity_hit");
            Destroy(PlayerInstance);
            livesCount--;
            UpdateLifeUI();
            SpawnPlayer();
            PlayerInstance.GetComponent<SimpleMovement>().setIframes();  
        } else if (livesCount <= 1){
            livesCount--;
            UpdateLifeUI();
			ParallaxBackgroundInstance.stopMotion();
            PlayerInstance.GetComponent<SimpleMovement>().setIframes();  
			PlayerInstance.GetComponentInChildren<Animator>().Play("Destruction");
			PlayerInstance.GetComponent<SimpleMovement>().disableShip();
            StartCoroutine(PlayerDeath());
		}
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

    IEnumerator PlayerDeath()
    {
        GameOverScreen.SetActive(true);
        enemyCount = 0;
        musicManager.Instance.playSound("player_death");
        Destroy(EnemySpawnerInstance);
        PlayerInstance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

		while (!PlayerInstance.GetComponent<SimpleMovement>().hasFinishedExploding()) {
	        yield return null;
    	}

        Destroy(PlayerInstance);
        Destroy(powerUpPrefabClone);
        Destroy(powerUpPrefabClone2);
        Destroy(powerUpPrefabClone3);

        yield return new WaitForSeconds(1.5f);

        Application.LoadLevel(Application.loadedLevel);
        MultiShotPowerUpImage.SetActive(false);
        OverchargePowerUpImage.SetActive(false);
        ExplosivePowerUpImage.SetActive(false);
        multishot = false;
        ocOn = false;
        xpl = false;
        enemyCount = 0;
        MainMenu();
    }

    public void PowerUpHit(int powerUpIndex)
    {
        SimpleMovement simpleMovementScript = PlayerObject.GetComponent<SimpleMovement>();
        musicManager.Instance.playSound("pickup_powerup");
        switch (powerUpIndex) 
        {
            case 1:
                if(ocOn == true){
                    ocOn = false;
                    OverchargePowerUpImage.SetActive(false);
                    simpleMovementScript.cooldownTime = .5f;
                }
                if(xpl == true){
                    xpl = false;
                    ExplosivePowerUpImage.SetActive(false);
                }
                simpleMovementScript.bulletPowerUpTime += 10;
                MultiShotPowerUpImage.SetActive(true);
                multishot = true;
                Debug.Log("multishot on");
                
                break;
            case 2:
                if(multishot == true){
                    multishot = false;
                    MultiShotPowerUpImage.SetActive(false);
                }
                if(xpl == true){
                    xpl = false;
                    ExplosivePowerUpImage.SetActive(false);
                    
                }
                simpleMovementScript.bulletPowerUpTime += 10;
                OverchargePowerUpImage.SetActive(true);
                ocOn=true;
                Debug.Log("OverCharge on");
                
                break;
            case 3:
                if(ocOn == true){
                    ocOn = false;
                    OverchargePowerUpImage.SetActive(false);
                    simpleMovementScript.cooldownTime = .5f;
                }
                if(multishot == true){
                    multishot = false;
                    MultiShotPowerUpImage.SetActive(false);
                }
                
                simpleMovementScript.bulletPowerUpTime += 10;
                ExplosivePowerUpImage.SetActive(true);
                xpl = true;
                Debug.Log("xpl on");
                
                break;
            default:
                break;
        }
    }

    public void RewardPoint(Vector3 spot)
    {
        float randomValue = Random.Range(1f,20f);
        if (randomValue < 3f && ocOn != true) {
            //Rigidbody2D powerUpPrefabClone;
            powerUpPrefabClone = Instantiate(OverChargePrefab, spot, transform.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        } else if (randomValue < 5f && xpl != true) {
            //Rigidbody2D powerUpPrefabClone2;
            powerUpPrefabClone2 = Instantiate(ExplosivePrefab, spot, transform.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        } else if (randomValue < 8f && multishot != true) {
            //Rigidbody2D powerUpPrefabClone3;
            powerUpPrefabClone3 = Instantiate(MultishotPrefab, spot, transform.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        }
        Score++;
        ScoreText.text = "" + Score;
        CameraShake.instance.shakeDuration = 0.04f;
    }

    public void StartOnePlayer()
    {
		ParallaxBackgroundInstance.goFast();

		if (musicManager.Instance.getCurrentTrack() != "GameTheme"){
			musicManager.Instance.playMusic("GameTheme");
		}

        Score = 0;
        livesCount = 3;
        ScoreText.text = "" + Score;
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(true);
        UpdateLifeUI();
        SpawnPlayer();
        SpawnEnemy();
        Cursor.visible = false;
    }

    public void SpawnEnemy()
    {
        //Debug.Log("Calling Spawn Enemy with " + enemyCount + " remaining!");
        EnemySpawnerInstance = Instantiate(EnemySpawnerObject, new Vector2(0, 2f), Quaternion.identity);
    }

    public void SpawnPlayer()
    {
        PlayerInstance = Instantiate(PlayerObject, new Vector2(0, -5f), Quaternion.identity);
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
