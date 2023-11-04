using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb2D;
    private Vector2 velocity = Vector2.up;
    private bool explodes = false;
    private float xplRange = 1;
    AudioClip pewpew;
    private float hitTime=0;
    //private float IframeCD = 4f;
    //private IEnumerator coroutine;
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //coroutine = Iframe_timer(gameObject.GetComponent<SimpleMovement>().getIframes());
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        //StartCoroutine(coroutine);
    }

    public void send_off(Vector2 direction, float speed_multiplier, bool xpl)
    {
        explodes = xpl;
        velocity = speed * direction * speed_multiplier;
        musicManager.Instance.playSound("shoot");
        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(explodes == true){
            //does explode
            //missileCircle();
            missileCircle();
        } else {
            //does not explode
            if (other.gameObject.CompareTag("Enemy"))
            {
                //GameManager.instance.enemyCount -= 1;
                //GameManager.instance.RewardPoint();
                Debug.Log("Enemy Count:" + GameManager.instance.enemyCount);
            } else if (other.gameObject.CompareTag("Player"))
            {
                if(other.GetComponent<SimpleMovement>().getIframes()){
                Debug.Log("Iframes active on " + other.gameObject.tag);
                /*if(Time.time - hitTime >= 4f){
                        other.GetComponent<SimpleMovement>().setIframes();
                    }*/
                }
                else{
                    Debug.Log("Player hit!");
                    hitTime = Time.time;
                    other.GetComponent<SimpleMovement>().setIframes();
                    //StartCoroutine("Iframe_timer");
                    Debug.Log("TimeStamp = " + hitTime);
                    //other.GetComponent<SimpleMovement>().setIframes();
                    GameManager.instance.PlayerHit();
                }
            }
            
            if (other.gameObject.CompareTag("MultiShotPowerup"))
            {
                GameManager.instance.PowerUpHit(1);
                musicManager.Instance.playSound("trip_laser");
                Destroy(other.gameObject);
                Debug.Log("PowerUp Count:" + GameManager.instance.powerUpCount);
            } else if (other.gameObject.CompareTag("OverchargePowerup"))
            {
                GameManager.instance.PowerUpHit(2);
                Destroy(other.gameObject);
                Debug.Log("PowerUp Count:" + GameManager.instance.powerUpCount);
            } else if (other.gameObject.CompareTag("ExplosivePowerup"))
            {
                GameManager.instance.PowerUpHit(3);
                Destroy(other.gameObject);
                Debug.Log("PowerUp Count:" + GameManager.instance.powerUpCount);
            }

            Destroy(this.gameObject);
        }
    }

    private void missileCircle(){
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, xplRange);
        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(this.gameObject, exp.main.duration);
        Debug.Log("exploded objects: "+hitColliders.Length);
        int temphit = 0;
        foreach (Collider2D hit in hitColliders)
        {
            Enemy hit_Enemy = hit.GetComponent<Enemy>();
            
            if(hit_Enemy != null){
                //Debug.Log("xpl");
                temphit++;
                Debug.Log("exploded enemys: "+temphit);
                hit_Enemy.Die();
            }
        }
    }
    private void missileBox(){
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 6), 0);
        foreach (Collider2D hit in hitColliders)
        {
            Enemy hit_Enemy = hit.GetComponent<Enemy>();
            if(hit_Enemy != null){
                //Debug.Log("xpl");
                ParticleSystem exp = GetComponent<ParticleSystem>();
                exp.Play();
                GameManager.instance.RewardPoint();
                Destroy(this.gameObject, exp.main.duration);
                Destroy(hit_Enemy.gameObject);
            }
        }
    }
}