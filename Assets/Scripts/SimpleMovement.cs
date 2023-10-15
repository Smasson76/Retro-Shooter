using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed;
    private float Move;
    private Color newCol;
    private Color oldCol;
    public Vector3 firing_point_offset = new Vector3(0, 0.5f, 0);
    public float bullet_speed = 1f;
    private float lastShotTime = -1f;
    public float cooldownTime = .5f;

    private bool ocOn = true;
    private float overchargeTime;
    private float ocStart;


    public Bullet bulletPrefab;

    void Fire()
	{
        if((Time.time - lastShotTime) > cooldownTime){
            Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
            bullet.send_off(Vector2.up, bullet_speed);

            lastShotTime = Time.time;
        }
	}

    private Rigidbody2D rb;
    private SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        
        oldCol = spr.material.GetColor("_Color");
        newCol = new Color(0.4f, 0.9f, 0.7f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        if(ocOn == true){
            overchargeTime -= Time.deltaTime;
        }

        if (Input.GetKey("r") == true){
            ocOn = true;
            overchargeTime = 4;
        }

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetKey("space") == true)
        {
            spr.material.SetColor("_Color", newCol);
            if(ocOn == true){
                //overchargeTime -= Time.deltaTime;
                if(overchargeTime > 0.0f){
                    cooldownTime = .0005f;
                } else {
                    cooldownTime = .5f;
                    ocOn = false;
                }
            }
            Fire();
        }
        if (Input.GetKey("space") == false)
        {
            spr.material.SetColor("_Color", oldCol);
        }
    }
}
