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
    Vector2 origin_position;
    float theta = 0f;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        origin_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if ((timeLeft < 0) && can_shoot)
        {
            timeLeft = 2.0f;
            fire();
        }
        moveCircles(origin_position.x,origin_position.y,0.25f);
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
        if (other.gameObject.CompareTag("Bullet"))
        {
            float randomValue = Random.value;
            if (randomValue < chance) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab, transform.position, transform.rotation) as Rigidbody2D;
            }
            Die();
        }
    }
    
    public void Die()
    {
        collider.enabled = false;
        anim.SetTrigger("Death");
        musicManager.Instance.playSound("enemy_die");
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
