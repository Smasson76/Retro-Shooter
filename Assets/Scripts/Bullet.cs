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
        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(explodes == true){
            //does explode
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, xplRange);
            foreach (Collider2D hit in hitColliders)
            {
                Enemy hit_Enemy = hit.GetComponent<Enemy>();
                if(hit_Enemy != null){
                    Debug.Log("xpl");
                    ParticleSystem exp = GetComponent<ParticleSystem>();
                    exp.Play();
                    GameManager.instance.RewardPoint();
                    Destroy(this.gameObject, exp.main.duration);
                    Destroy(hit_Enemy.gameObject);
                }
            }
        } else {
            //does not explode
            if (other.gameObject.CompareTag("Enemy"))
            {
                GameManager.instance.enemyCount -= 1;
                GameManager.instance.RewardPoint();
            } else if (other.gameObject.CompareTag("Player"))
            {
                GameManager.instance.PlayerHit();
            } else if (other.gameObject.CompareTag("MultiShotPowerup"))
            {
                GameManager.instance.PowerUpHit(1);
                Destroy(other.gameObject);
            } else if (other.gameObject.CompareTag("OverchargePowerup"))
            {
                GameManager.instance.PowerUpHit(2);
                Destroy(other.gameObject);
            } else if (other.gameObject.CompareTag("ExplosivePowerup"))
            {
                GameManager.instance.PowerUpHit(3);
                Destroy(other.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}