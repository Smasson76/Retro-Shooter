using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb2D;
    private Vector2 velocity = Vector2.up;
    private bool explodes = false;
    private float xplRange = 1.5f;
    AudioClip pewpew;
    private float hitTime=0;
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
    }

    public void send_off(Vector2 direction, float speed_multiplier, bool xpl)
    {
        explodes = xpl;
        velocity = speed * direction * speed_multiplier;
        musicManager.Instance.playSound("shoot");
        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other){
        
        if (other.gameObject.CompareTag("Enemy")){
            if(explodes == true){
                missileCircle(other);
            } else {
                Destroy(this.gameObject);
            }

            
        } else if (other.gameObject.CompareTag("Player")){
            if(other.GetComponent<SimpleMovement>().getIframes()){
                //do nothing - invinsible
            }
            else{
                hitTime = Time.time;
                other.GetComponent<SimpleMovement>().setIframes();
                GameManager.instance.PlayerHit();
                Destroy(this.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("MultiShotPowerup"))
        {
            
            GameManager.instance.PowerUpHit(1);
            musicManager.Instance.playSound("trip_laser");
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
        } else if (other.gameObject.CompareTag("OverchargePowerup"))
        {
            
            GameManager.instance.PowerUpHit(2);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
        } else if (other.gameObject.CompareTag("ExplosivePowerup"))
        {
            
            GameManager.instance.PowerUpHit(3);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
        }

        else if (other.gameObject.CompareTag("ShieldPowerup"))
        {
            GameManager.instance.PowerUpHit(4);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }

    private void missileCircle(Collider2D other){
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, xplRange);
        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(this.gameObject, exp.main.duration);
        foreach (Collider2D hit in hitColliders)
        {
            
            if(hit.gameObject.CompareTag("Enemy") == true && hit != other){
                hit.gameObject.GetComponent<Enemy>().Die();
            }
        }
    }
    private void missileBox(Collider2D other){
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 12), 0);
        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(this.gameObject, exp.main.duration);
        foreach (Collider2D hit in hitColliders)
        {
            if(hit.gameObject.CompareTag("Enemy") == true && hit != other){
                hit.gameObject.GetComponent<Enemy>().Die();
            }
        }
        
    }
} 

