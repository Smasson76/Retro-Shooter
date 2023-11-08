using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    public float size_scale = 1.25f;
    public float movement_radius = 0.25f;
    private int Count=0;
    //private Vector2 velocity = new Vector2(0,-1);
    private Vector2 original_position;
    private float curr_time;
    private Rigidbody2D rb;
    private float cooldown = 2f;
    private bool trigger_flag;
    void Start()
    {
        //maybe give a velocity to move toward player with
        this.gameObject.name = "Tank" + Count + 1;
        Count++;
        this.gameObject.tag = "Enemy";
        transform.localScale = new Vector3(size_scale,size_scale,size_scale);
        original_position = new Vector2(transform.position.x,transform.position.y);
        translational_velocity = new Vector2(0f,-1f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        cooldown = base.calculate_next_fire_time();
        //curr_time = Time.time;
        //theta = 2f;
    }
    void Update(){
        translational_velocity = new Vector2(translational_velocity.x * (Time.deltaTime),translational_velocity.y *(Time.deltaTime));
        
        //transform.position = new Vector2(transform.position.x + translational_velocity.x,transform.position.y + translational_velocity.y);
        rb.velocity = new Vector2(0f,0f);
        //gameObject.GetComponent<Enemy>().fire();
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
        if(obj.gameObject.CompareTag("Bullet")){
            if(obj.gameObject.transform.parent.CompareTag("Player")){
                 Debug.Log("Enemy " + this + " collided with " + obj.gameObject.tag);
                GameManager.instance.enemyCount--;
                GameManager.instance.RewardPoint();
                Destroy(this.gameObject);
            }
            else{
                //do nothing
            }
        }
        if(obj.gameObject.CompareTag("Player")){
             if(obj.GetComponent<SimpleMovement>().getIframes()){
                Debug.Log("Iframes active on " + obj.gameObject.tag);
                /*if(Time.time - hitTime >= 4f){
                        other.GetComponent<SimpleMovement>().setIframes();
                    }*/
                }
                else{
                    Debug.Log("Player hit!");
                    //hitTime = Time.time;
                    obj.GetComponent<SimpleMovement>().setIframes();
                    //StartCoroutine("Iframe_timer");
                    //Debug.Log("TimeStamp = " + hitTime);
                    //other.GetComponent<SimpleMovement>().setIframes();
                    GameManager.instance.PlayerHit();
                    GameManager.instance.enemyCount--;
                    GameManager.instance.RewardPoint();
                }
        }
        if(obj.gameObject.CompareTag("ScreenDeath")){
            Debug.Log("Enemy " + this + " collided with " + obj.gameObject.tag);
            GameManager.instance.enemyCount--;
            GameManager.instance.RewardPoint();
            Destroy(this.gameObject);
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
        if(Time.time - curr_time < 3f){
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