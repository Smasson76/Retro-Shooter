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

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
		if (hit.collider != null) {
			if (hit.collider.CompareTag("Enemy")){
				Debug.Log("This is an enemy, I can't shoot");
				can_shoot = false;	
			} else {
				Debug.Log("!!!" + hit.collider.tag);
				can_shoot = true;
			}
		} else if (hit.collider == null) {
			Debug.Log("Didnt get a hit");
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
            if (randomValue < chance) {
                Rigidbody2D powerUpPrefabClone;
                powerUpPrefabClone = Instantiate(powerUpPrefab, transform.position, transform.rotation) as Rigidbody2D;
            }
            Die();
        }
    }
    
    void Die()
    {
        collider.enabled = false;
        anim.SetTrigger("Death");
        Destroy(this.gameObject, 0.8f); 
    }
}
