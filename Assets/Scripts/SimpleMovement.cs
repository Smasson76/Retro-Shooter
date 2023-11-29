using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public bool Iframes= false;
    public float speed;
    private float Move;
    public Vector3 firing_point_offset = new Vector3(0, 0.5f, 0);
    public float bullet_speed = 1f;
    private float lastShotTime = -1f;
    public float cooldownTime = .5f;

    public float bulletPowerUpTime;
    public float ocStart;

    public Bullet bulletPrefab;
    public Color mycol;
    public Color original;
    public float IframeCD = 2.5f;

	private Animator animator;
	private bool shipDisabled = false;
    public string Anim_state;

	public string ship_color = "blue";

    void Fire()
	{
        if((Time.time - lastShotTime) > cooldownTime) {
            if(GameManager.instance.multishot == true) {
                if(bulletPowerUpTime > 0f) {
                    Bullet bullet1 = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    bullet1.transform.SetParent(this.transform);

                    Bullet bullet2 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(-1.5f, 0.5f, 0), Quaternion.identity);
                    bullet2.transform.SetParent(this.transform);

                    Bullet bullet3 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(1.5f, 0.5f, 0), Quaternion.identity);
                    bullet3.transform.SetParent(this.transform);

                    bullet1.send_off(Vector2.up, bullet_speed, false);
                    bullet2.send_off(Vector2.up, bullet_speed, false);
                    bullet3.send_off(Vector2.up, bullet_speed, false);

                    lastShotTime = Time.time;
                } else {
                    GameManager.instance.multishot = false;
                    bulletPowerUpTime = 0;
                    Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    bullet.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);

                    lastShotTime = Time.time;
                }
            } else {
                Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                bullet.transform.SetParent(this.transform);
                bullet.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);

                lastShotTime = Time.time;
            }
        }
	}

    private Rigidbody2D rb;

	public void startDeathAnim(){
		animator.Play("Destruction");
	}


    void Start()
    {
        Anim_state = "";
		animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        original = GameManager.instance.PlayerInstance.GetComponentInChildren<SpriteRenderer>().material.GetColor("_Color");
        mycol = Color.red;
        StartCoroutine("flashChar");
    }

    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        if(GameManager.instance.multishot == true || GameManager.instance.ocOn == true || GameManager.instance.xpl == true){
            bulletPowerUpTime -= Time.deltaTime;
            if(GameManager.instance.ocOn == true){
                cooldownTime = .25f;
            }
            if (bulletPowerUpTime <= 0f)
            {
                if(GameManager.instance.multishot == true){
                    GameManager.instance.multishot = false;
                    GameManager.instance.MultiShotPowerUpImage.SetActive(false);
                    bulletPowerUpTime = 0;
                }
                if(GameManager.instance.ocOn == true) {
                    GameManager.instance.ocOn = false;
                    GameManager.instance.OverchargePowerUpImage.SetActive(false);
                    cooldownTime = .5f;
                    bulletPowerUpTime = 0;
                }
                if(GameManager.instance.xpl == true){
                    GameManager.instance.xpl = false;
                    GameManager.instance.ExplosivePowerUpImage.SetActive(false);
                    bulletPowerUpTime = 0;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Escape was pressed -> call main menu");
            GameManager.instance.ParallaxBackgroundInstance.stopMotion();
            GameManager.instance.PlayerInstance.GetComponent<SimpleMovement>().disableShip();
			GameManager.instance.PlayerDeath();
            GameManager.instance.reset_main_menu();
        }
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);
        if (Input.GetKey("space") == true)
        {
            if(!Iframes){
                Fire();
            }
            else{
                //StartCoroutine("flashChar");
                //gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",mycol);
                //Debug.Log("Cannot shoot while invulnerable! \nColor is " + gameObject.GetComponentInChildren<SpriteRenderer>().material.GetColor("_Color"));
            }
        }
        if(Iframes){
            StartCoroutine("Iframes_timer");
        }
        cooldownTime = 0.5f;
		if (shipDisabled == false){
			rb.velocity = new Vector2(Move * speed, rb.velocity.y);
			if (Input.GetKey("space") == true)
			{
				if(!Iframes){
					if(GameManager.instance.ocOn == true){
						if(bulletPowerUpTime > 0.0f){
							cooldownTime = .25f;
						} else {
							cooldownTime = .5f;
							GameManager.instance.ocOn = false;
						}
					}
					Fire();
				}
			}
			if(Iframes){
				StartCoroutine("Iframes_timer");
			}
		}
    }

	public void disableShip(){
		shipDisabled = true;
	}

	public bool hasFinishedExploding(){
		animator = GetComponentInChildren<Animator>();
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Empty")){
			Destroy(GetComponentInChildren<ParticleSystem>());
			return true;
		} else {
			return false;
		}
	}

    public bool getIframes(){
        return Iframes;
    }

    public void setIframes(){
        Iframes = !Iframes;
    }
    public void set_state(string arg)
    {
        Anim_state = arg;
    }
    public string get_state()
    {
        return Anim_state;
    }

    IEnumerator flashChar(){
        while(Iframes){
            this.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",mycol);
            yield return new WaitForSeconds(0.25f);

            this.gameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color",original);
            yield return new WaitForSeconds(0.25f);
        }
    }
     IEnumerator Iframes_timer(){
        yield return new WaitForSeconds(IframeCD);
        if(Iframes){
			setIframes();
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("MultiShotPowerup"))
        {
            GameManager.instance.PowerUpHit(1);
            musicManager.Instance.playSound("trip_laser");
            Destroy(other.gameObject);
        } else if (other.gameObject.CompareTag("OverchargePowerup"))
        {
            GameManager.instance.PowerUpHit(2);
            Destroy(other.gameObject);
        } else if (other.gameObject.CompareTag("ExplosivePowerup"))
        {
            GameManager.instance.PowerUpHit(3);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("ShieldPowerup"))
        {
            GameManager.instance.PowerUpHit(4);
            Destroy(other.gameObject);
        }
    }

	public void player_enter(){
		animator.Play("PlayerEntrance");
	}

	public void on_player_entered(){
		GameManager.instance.on_player_entered();
	}

	public void on_destruction_finished(){
		GameManager.instance.on_player_destroyed();
	}

	public void change_color(string color_str, string prev_color){
		animator.ResetTrigger(prev_color);
        animator.SetTrigger(color_str);
		animator.Play("ColorSelector");
	}
}
