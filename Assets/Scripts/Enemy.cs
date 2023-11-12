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
    public Rigidbody2D powerUpPrefab2;
    public Rigidbody2D powerUpPrefab3;
    public Rigidbody2D powerUpPrefab4;
    public float chance = 1.5f;
    public float chance2 = 3f;
    public float chance3 = 5f;
    public float chance4 = 7f;

	public Vector2 firing_window = new Vector2(1.5f, 2.5f);
	public BloodBurstParticleEffect bloodBurstEffect;

    public AudioClip enemy_death;
    Vector2 origin_position;
    public float theta=0f;
    private float safety = 1.80f;
    public Vector2 translational_velocity = new Vector2(0f,0f);
    public float TimeSpawned;
    private float timeStamp=5f;
    bool flag;
    private List<Rigidbody2D> powerUpPrefabClones = new List<Rigidbody2D>();
    private int cloneCount =0;
    //public IEnumerator coroutine;

    public bool isDead = false;
    

	public virtual float calculate_next_fire_time(){
		float next_firing_time = Random.Range(firing_window.x, firing_window.y)+safety;
		return next_firing_time;
	}
    void Awake()
    {
        firing_window = new Vector2(transform.localScale.x + safety,transform.localScale.y + safety);
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        origin_position = transform.position;
		timeLeft = calculate_next_fire_time();
        
    }
    public void setSpawnTime(float arg){
        TimeSpawned = arg;
    }
    public float getSpawnTime(){
        return TimeSpawned;
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
            GameObject obj = GameObject.FindWithTag("MultiShotPowerup");
            if(obj){
                Debug.Log("Powerup found! " + obj.tag);
                timeStamp -= Time.deltaTime;
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(0f,-10f);
                rb.position = obj.transform.forward * Time.deltaTime;//new Vector2(obj.transform.forward.x*rb.velocity.x,obj.transform.forward.y*rb.velocity.y);
                rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
                //obj.transform.position = new Vector2(rb.position.x + rb.velocity.x * Time.deltaTime,rb.position.y+rb.velocity.y*Time.deltaTime);
                Debug.Log("del = " + timeStamp);
                if(timeStamp < 0f){
                Debug.Log("Powerup destroyed!");
                Destroy(obj);
                }
            }
    }
    
    public virtual void fire(){
        Bullet new_bullet = Instantiate(bullet, PointOfFireObject.transform.position, Quaternion.identity);
        //new_bullet.transform.SetParent(this.transform,true);
        new_bullet.send_off(Vector2.down, bullet_speed, false);
        Destroy(new_bullet, 3f);
    }

    public void set_can_shoot(bool val){
        this.can_shoot = val;
    }
    public bool get_can_shoot(){
        if(can_shoot){
            return true;
        }
        else{return false;}
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
            musicManager.Instance.playSound("entity_hit");
            GameManager.instance.enemyCount -= 1;
            GameManager.instance.RewardPoint();
            musicManager.Instance.playSound("enemy_die");
            
            Destroy(this.gameObject, 0.8f);
            isDead = true; 
            BloodBurstParticleEffect bloodBurst = Instantiate(bloodBurstEffect, transform.position, Quaternion.identity);
        }
		
    }
    public virtual void moveCircles(float x, float y, float radius){
        //radius will be the perp dist to center of circular path (how big is circle)
        //x and y will be used to set the vertex position
        Vector2 _vertex = new Vector2(x+(translational_velocity.x*Time.deltaTime),y+(translational_velocity.y*Time.deltaTime));
        float omega = 1f; //angular velocity
        theta += omega * Time.deltaTime;
        var cirpos = radius * (new Vector2(Mathf.Cos(theta),Mathf.Sin(theta)));
        this.transform.position = cirpos + _vertex;
    }
}
