using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    public float size_scale = 1.25f;
    public float movement_radius = 0.25f;
    private int Count=0;
    private Vector2 original_position;
    private float curr_time;
    private Rigidbody2D rb;
    private float cooldown = 2f;
    private bool trigger_flag;
<<<<<<< HEAD
<<<<<<< HEAD
=======
    public float Dive_timer;
    //private bool isDead = false;
>>>>>>> 8ce42ec (Tried a bunch of solutions to fix skin bug -- didn't ;however, I did change tank divebomb to be random time intervals as opposed to uniform)
=======
    public float Dive_timer;
    //private bool isDead = false;
>>>>>>> d16dc14 (preparing final PR)

    void Start()
    {
        Dive_timer = UnityEngine.Random.Range(0f,10f);
        //maybe give a velocity to move toward player with 
        this.gameObject.name = "Tank" + Count + 1;
        Count++;
        this.gameObject.tag = "Enemy";
        transform.localScale = new Vector3(size_scale,size_scale,size_scale);
        original_position = new Vector2(transform.position.x,transform.position.y);
        translational_velocity = new Vector2(0f,-1f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        cooldown = base.calculate_next_fire_time();
    }
    void Update(){
        translational_velocity = new Vector2(translational_velocity.x * (Time.deltaTime),translational_velocity.y *(Time.deltaTime));

        rb.velocity = new Vector2(0f,0f);
        cooldown -= Time.deltaTime;
        if ((cooldown < 0) && trigger_flag)
        {
            base.fire();
            cooldown = base.calculate_next_fire_time();
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
		    if (hit.collider != null) {
			    if (hit.collider.CompareTag("Enemy")){
				    trigger_flag = false;	
			    } else {
				    trigger_flag = true;
			    }
		    } else if (hit.collider == null) {
			      trigger_flag = true;
		    }
        StartCoroutine("DiveBomb");
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(isDead == false){
            if(obj.gameObject.CompareTag("Bullet")){
                if (obj.gameObject.transform.parent is null) return;
                if(obj.gameObject.transform.parent.CompareTag("Player")){
                    Die();
                    isDead = true;
                }
            }
            if(obj.gameObject.CompareTag("Player")){
                if(obj.GetComponent<SimpleMovement>().getIframes()){
                }
                else{
                    obj.GetComponent<SimpleMovement>().setIframes();
                    GameManager.instance.PlayerHit();
                    Die();
                    isDead = true;
                }
            }
            if(obj.gameObject.CompareTag("ScreenDeath")){
                GameManager.instance.enemyCount -= 1;
                GameManager.instance.RewardPoint();
                musicManager.Instance.playSound("enemy_die");
                
                Destroy(this.gameObject, 0.8f);
                isDead = true;
            }
        }
        
    }
    public override float calculate_next_fire_time(){
        return base.calculate_next_fire_time();
    }
    public override void fire(){
        base.fire();
    }
    IEnumerator DiveBomb(){
        curr_time = gameObject.GetComponent<Enemy>().getSpawnTime();
        if(Time.time - curr_time < Dive_timer){
            moveCircles(original_position.x,original_position.y,movement_radius);
        }
        else{
            trigger_flag = false;
            rb.velocity = new Vector2(0,-2*Time.deltaTime);
            transform.position = new Vector2(transform.position.x+rb.velocity.x,transform.position.y+rb.velocity.y);
        }
        yield return null;
    }
}