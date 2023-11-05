using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public bool can_shoot = false;
    public Bullet bullet;
    public float bullet_speed = 1f;
    float timeLeft = 2.0f;
    public GameObject PointOfFireObject;
    public Animator anim;
    public BoxCollider2D collider;
    public Rigidbody2D powerUpPrefab;
    public float chance = 5f;
	public Vector2 firing_window; 
    public float safety_buffer;
    public AudioClip enemy_death;
    Vector2 origin_position;
    float theta = 0f;
    

	float calculate_next_fire_time(){
		float next_firing_time = Random.Range(firing_window.x, firing_window.y)+safety_buffer;
		return next_firing_time;
	}
    
    void Awake()
    {
        safety_buffer  = (float) UnityEngine.Random.Range(1,5)/10;
        firing_window = new Vector2(transform.localScale.x-safety_buffer,2*transform.localScale.y+safety_buffer);
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        origin_position = transform.position;
		timeLeft = calculate_next_fire_time();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if ((timeLeft < 0) && can_shoot)
        {
            fire();
            timeLeft = calculate_next_fire_time();
        }
        moveCircles(origin_position.x,origin_position.y,0.25f);
         RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
		    if (hit.collider != null) {
			    if (hit.collider.CompareTag("Enemy")||hit.collider.CompareTag("Tank")){
				    //Debug.Log("This is an enemy, I can't shoot");
				    can_shoot = false;	
			    } else {
				    //Debug.Log(this.gameObject.tag + "!!!" + hit.collider.tag);
				    can_shoot = true;
			    }
		    } else if (hit.collider == null) {
			      //Debug.Log("Didnt get a hit");
			      can_shoot = true;
		    }
    }

    void fire(){
        Bullet new_bullet = Instantiate(bullet, PointOfFireObject.transform.position, Quaternion.identity);
        new_bullet.send_off(Vector2.down, bullet_speed, false);
        Destroy(new_bullet, 3f);
    }

    public void set_can_shoot(bool val){
        this.can_shoot = val;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Player"))
        {
            float randomValue = Random.value;
            if (randomValue < chance) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab, transform.position, transform.rotation) as Rigidbody2D;
            }
            if(other.gameObject.CompareTag("Player")){
                GameManager.instance.PlayerHit();
            }
            Die();
        }
        if(other.gameObject.CompareTag("ScreenDeath")){
            //obj left screen, kill it and decrease enemy counter
            int prev = GameManager.instance.enemyCount;
            GameManager.instance.enemyCount--;
            Debug.Log("Boom!!" + this.gameObject + " is destroyed! and enemycount decreased from " + prev + " -> " + GameManager.instance.enemyCount);
            Die();
            //Destroy(other.gameObject,3f);
        }
    }
    
    public void Die()
    {
        collider.enabled = false;
        anim.SetTrigger("Death");
        musicManager.Instance.playSound("entity_hit");
        Destroy(this.gameObject, 0.8f); 
    }
    public void moveCircles(float x,float y,float radius){
    //radius will be the perp dist to center of circular path (how big is circle)
    //x and y will be used to set the vertex postion
    Vector2 _vertex = new Vector2(x,y);
    float omega = 1f;//angular velocity
    theta += omega * Time.deltaTime;
    var cirpos = radius * (new Vector2(Mathf.Cos(theta),Mathf.Sin(theta)));
    this.transform.position = cirpos + _vertex;
    }
}
