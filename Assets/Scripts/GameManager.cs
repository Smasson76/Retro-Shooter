using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    public GameObject ShipSelection;
    protected string animation_string;
    public bool selection_has_been_made =false;
    public GameObject[] livesUICounter;
    public GameObject OverchargePowerUpImage;
    public GameObject ExplosivePowerUpImage;
    public GameObject MultiShotPowerUpImage;
    public GameObject ShieldPowerUpImage;

    [Header("-- GAME PROPERTIES --")]
    public int Score = 0;
    public int HighScore;
    public int livesCount = 3;
    public int enemyCount = 0;
    public int powerUpCount = 0;
    private float timeStamp=6f;
    private float timeStamp2=6f;
    private float timeStamp3=6f;
    private float timeStamp4 = 10f;
    

    [Header("-- GAME OBJECTS --")]
    public GameObject PlayerObject;
    public GameObject PlayerInstance;
    public GameObject EnemySpawnerObject;
    public GameObject EnemySpawnerInstance;
    public Rigidbody2D OverChargePrefab;
    public Rigidbody2D ExplosivePrefab;
    public Rigidbody2D MultishotPrefab;
    public Rigidbody2D ShieldPrefab;
    private Rigidbody2D powerUpPrefabClone;
    private Rigidbody2D powerUpPrefabClone2;
    private Rigidbody2D powerUpPrefabClone3;
    private Rigidbody2D powerUpPrefabClone4;
	  public Parallax ParallaxBackgroundObject;
	  public Parallax ParallaxBackgroundInstance;

    [Header("-- PLAYER PROPERTIES --")]
    public bool ocOn;
    public bool multishot;
    public bool xpl;
    public bool shield;

	private Vector2 player_start_coords = new Vector2(0, -5f);

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
        ShieldPowerUpImage.SetActive(false);
        ocOn = false;
        multishot = false;
        xpl = false;
        shield = false;
    }

    void Update () 
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
		if (Score > highScore) {
			PlayerPrefs.SetInt("HighScore", Score);
			PlayerPrefs.Save();
			HighScoreText.text = "" + highScore;
        }
        GameObject obj = GameObject.FindWithTag("MultiShotPowerup");
        if(obj != null){
            timeStamp -= Time.deltaTime;
            if(timeStamp < 0f){
                Destroy(obj);
                timeStamp=6f;
            }
        }
        GameObject obj2 = GameObject.FindWithTag("OverchargePowerup");
        if(obj2 != null){
            timeStamp2 -= Time.deltaTime;

            if(timeStamp2 < 0f){
                Destroy(obj2);
                timeStamp2=6f;
            }
        }

        GameObject obj3 = GameObject.FindWithTag("ExplosivePowerup");
        if(obj3 != null){
            timeStamp3 -= Time.deltaTime;
            Debug.Log("del = " + timeStamp3);

            if(timeStamp3 < 0f){
                Destroy(obj3);
                timeStamp3=6f;
            }
        }

        GameObject obj4 = GameObject.FindWithTag("ShieldPowerup");
        if(obj4 != null)
        {
            Debug.Log("shield found");
            if(timeStamp4 < 0f)
            {
                Debug.Log("Shield destroyed");
                Destroy(obj4);
                timeStamp4 = 10f;
            }
        }
    }

    public void MainMenu()
    {
        UnityEngine.Cursor.visible = true;
        StartGameScreen.SetActive(true);
        GameMenu.SetActive(false);
        GameOverScreen.SetActive(false);
        CreditsScreen.SetActive(false);
        ShipSelection.SetActive(false);
		ParallaxBackgroundInstance.goSlow();
    }

    public void CreditsMenu()
    {
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(false);
        GameOverScreen.SetActive(false);
        CreditsScreen.SetActive(true);
        ShipSelection.SetActive(false);
    }

    public void PlayerHit()
    {
        if (shield)
        {
            shield = false;
            ShieldPowerUpImage.SetActive(false);
        }
        else if (livesCount > 1)
        {
			musicManager.Instance.playSound("entity_hit");
            string stateAtDeath = PlayerInstance.GetComponent<SimpleMovement>().get_state();
            PlayerObject.GetComponent<SimpleMovement>().set_state(stateAtDeath);
            Destroy(PlayerInstance);
            livesCount--;
            UpdateLifeUI();
            SpawnPlayer();
            PlayerInstance.GetComponent<SimpleMovement>().setIframes();
            ChooseYourShip.instance.ResetSkin(animation_string); //doesn't work yet

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
                
                break;
            case 4:
                shield = true;
                ShieldPowerUpImage.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void SpawnPowerUp(Transform spot){
        float randomValue = Random.Range(1f,20f);
        if (randomValue < 3f && GameManager.instance.ocOn != true && powerUpPrefabClone == null) {
            powerUpPrefabClone = Instantiate(OverChargePrefab, spot.position, spot.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        } else if (randomValue < 5f && GameManager.instance.xpl != true && powerUpPrefabClone2 == null) {
            powerUpPrefabClone2 = Instantiate(ExplosivePrefab, spot.position, spot.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        } else if (randomValue < 8f && GameManager.instance.multishot != true && powerUpPrefabClone3 == null) {
            powerUpPrefabClone3 = Instantiate(MultishotPrefab, spot.position, spot.rotation) as Rigidbody2D;
            GameManager.instance.powerUpCount += 1;
        }
        else if (randomValue < 12f && shield != true && powerUpPrefabClone4 == null)
        {
            Debug.Log("Trying to spawn shield");
            powerUpPrefabClone4 = Instantiate(ShieldPrefab, spot.position, spot.rotation) as Rigidbody2D;
            powerUpCount += 1;
            Debug.Log("Shield spawned");
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
		ParallaxBackgroundInstance.goFast();

		if (musicManager.Instance.getCurrentTrack() != "GameTheme"){
			musicManager.Instance.playMusic("GameTheme");
		}

        Score = 0;
        livesCount = 3;
        ScoreText.text = "" + Score;
        StartGameScreen.SetActive(false);
        GameMenu.SetActive(true);
        GameMenu.SetActive(false);
        ShipSelection.SetActive(true);
        SpawnPlayer();
    }

    public void SelectionMade()
    {
        selection_has_been_made = true;
        PlayerObject.GetComponent<SimpleMovement>().set_state(animation_string);
        UpdateLifeUI();
        ShipSelection.SetActive(false);
        GameMenu.SetActive(true);
		StartGame_stage1();
    }

	private void StartGame_stage1(){
        PlayerInstance.transform.position = player_start_coords;
        PlayerInstance.transform.localScale = new Vector3(2f,2f,0);
		// music stops. only engine sounds.
        PlayerInstance.GetComponent<SimpleMovement>().player_enter();  
        //Cursor.visible = false;

	}

	private void on_stage_one_complete(){
		StartGame_stage2();
	}

	public void on_player_entered(){
		on_stage_one_complete();
	}

	private void StartGame_stage2(){
		// Enemy spawn sound plays
        SpawnEnemy();
	}

	public void on_enemy_spawn_entered(){
		on_stage_two_complete();
	}

	private void on_stage_two_complete(){
		// play soundtrack
	}

    public void SpawnEnemy()
    {
        EnemySpawnerInstance = Instantiate(EnemySpawnerObject, new Vector2(0, 2f), Quaternion.identity);
    }

    public void SpawnPlayer()
    {
        PlayerInstance = Instantiate(PlayerObject, player_start_coords, Quaternion.identity);
        if(PlayerInstance != null)
        {
            Animator myanim = PlayerInstance.GetComponentInChildren<Animator>();
            myanim.SetTrigger(PlayerObject.GetComponent<SimpleMovement>().get_state());
        }
    }

    public void StartTwoPlayer()
    {
        //Future scenario where we will allow for 2 players
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void setAnimation_string(string arg)
    {
        animation_string = arg;
    }
    public string getAnimation_string()
    {
        return animation_string;
    }
}
