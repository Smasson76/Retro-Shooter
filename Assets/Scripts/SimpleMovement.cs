using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed;
    private float Move;
    public Vector3 firing_point_offset = new Vector3(0, 0.5f, 0);
    public float bullet_speed = 1f;
    private float lastShotTime = -1f;
    public float cooldownTime = .5f;

    public float shieldCoolDownTime = 10f;
    public float shieldUseTime = -11f;
    public bool shield = false;

    public Bullet bulletPrefab;

    void Fire()
	{
        if((Time.time - lastShotTime) > cooldownTime){
            Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
            bullet.send_off(Vector2.up, bullet_speed);

            lastShotTime = Time.time;
        }
	}

    //public String powerType;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (((Time.time - shieldUseTime) > shieldCoolDownTime) && shield)
        {
            print("sets shield to false");
            shield = false;
        }

        if (Input.GetKey(KeyCode.S) == true && ((Time.time - shieldUseTime) > shieldCoolDownTime))
        {
            print("Something is happening here");
            shield = true;
            shieldUseTime = Time.time;
        }

        if (Input.GetKey("space") == true)
        {
            Fire();
        }
    }
}
