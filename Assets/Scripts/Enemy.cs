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
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
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
            if (randomValue < chance) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab, transform.position, transform.rotation) as Rigidbody2D;
            } else if (randomValue < chance2) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab2, transform.position, transform.rotation) as Rigidbody2D;
            } else if (randomValue < chance3) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab3, transform.position, transform.rotation) as Rigidbody2D;
            }
            Death();
        }
    }
    
    void Death()
    {
        collider.enabled = false;
        anim.SetTrigger("Death");
        Destroy(this.gameObject, 0.8f); 
    }
}
