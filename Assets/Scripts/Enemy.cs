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
    public float chance = 2.5f;
	public Vector2 firing_window = new Vector2(1.5f, 2.5f);
	public BloodBurstParticleEffect bloodBurstEffect;

    public AudioClip enemy_death;
    Vector2 origin_position;
    public float theta=0f;
    private float safety = 0.25f;
    public Vector2 translational_velocity = new Vector2(0f,0f);
    public float TimeSpawned;

    

	float calculate_next_fire_time(){
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
				    can_shoot = false;	
			    } else {
				    can_shoot = true;
			    }
		    } else if (hit.collider == null) {
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
            float randomValue = Random.value;
            if (GameManager.instance.multishot == false)
            {
                if (randomValue < chance) {
                    Rigidbody2D powerUpPrefabClone;
                    powerUpPrefabClone = Instantiate(powerUpPrefab, transform.position, transform.rotation) as Rigidbody2D;
                }
            }
            Die();
        }
    }
    
    void Die()
    {
        collider.enabled = false;
        anim.SetTrigger("Death");
        musicManager.Instance.playSound("entity_hit");
        Destroy(this.gameObject, 0.8f); 
		BloodBurstParticleEffect bloodBurst = Instantiate(bloodBurstEffect, transform.position, Quaternion.identity);
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
