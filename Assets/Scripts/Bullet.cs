using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb2D;
    private Vector2 velocity = Vector2.up;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);

    }

    public void send_off(Vector2 direction, float speed_multiplier)
    {
        velocity = speed * direction * speed_multiplier;
        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.RewardPoint();
        } else if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerHit();
        }

        Destroy(this.gameObject);
    }
}