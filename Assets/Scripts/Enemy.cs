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
    public Vector2 firing_window = new Vector2(1.5f, 2.5f);

    public AudioClip enemy_death;
    Vector2 origin_position;
    private float theta=0f;
    public bool isDead = false;
    

	float calculate_next_fire_time(){
		float next_firing_time = Random.Range(firing_window.x, firing_window.y);
		return next_firing_time;
	}
    public Rigidbody2D powerUpPrefab;
    public Rigidbody2D powerUpPrefab2;
    public Rigidbody2D powerUpPrefab3;
    //public Rigidbody2D powerUpPrefab4;
    public float chance = 3f;
    public float chance2 = 5f;
    public float chance3 = 8f;
    //public float chance4 = 10f;
    
    void Awake()
    {
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
			    if (hit.collider.CompareTag("Enemy")){
				    //Debug.Log("This is an enemy, I can't shoot");
				    can_shoot = false;	
			    } else {
				    //Debug.Log("!!!" + hit.collider.tag);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            
            Die();
        }
    }
    
    public void Die()
    {
        if(isDead == false){
            collider.enabled = false;
            anim.SetTrigger("Death");
            GameManager.instance.enemyCount -= 1;
            GameManager.instance.RewardPoint();
            musicManager.Instance.playSound("enemy_die");
            Debug.Log("Dies");
            Destroy(this.gameObject, 0.8f);
            isDead = true; 
        }
    }
    void moveCircles(float x, float y, float radius){
        //radius will be the perp dist to center of circular path (how big is circle)
        //x and y will be used to set the vertex position
        Vector2 _vertex = new Vector2(x,y);
        float omega = 1f; //angular velocity
        theta += omega * Time.deltaTime;
        var cirpos = radius * (new Vector2(Mathf.Cos(theta),Mathf.Sin(theta)));
        this.transform.position = cirpos + _vertex;
    }
}
