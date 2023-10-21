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

    private bool ocOn = false;
    private bool multishot = false;
    private bool xpl = false;
    private float xplTime;
    private float overchargeTime;
    private float multishotTime;
    private float ocStart;


    public Bullet bulletPrefab;

    void Fire()
	{
        if((Time.time - lastShotTime) > cooldownTime){
            if(multishot == true){
                if(multishotTime > 0f){
                    Bullet bullet1 = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    Bullet bullet2 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(-1f, 0.5f, 0), Quaternion.identity);
                    Bullet bullet3 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(1f, 0.5f, 0), Quaternion.identity);
                    bullet1.send_off(Vector2.up, bullet_speed, xpl);
                    bullet2.send_off(Vector2.up, bullet_speed, xpl);
                    bullet3.send_off(Vector2.up, bullet_speed, xpl);

                    lastShotTime = Time.time;
                } else {
                    multishot = false;
                    Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    bullet.send_off(Vector2.up, bullet_speed, xpl);

                    lastShotTime = Time.time;
                }
            } else {
                Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                bullet.send_off(Vector2.up, bullet_speed, xpl);

                lastShotTime = Time.time;
            }
        }
	}

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
        if(ocOn == true){
            overchargeTime -= Time.deltaTime;
        }
        if(multishot == true){
            multishotTime -= Time.deltaTime;
        }
        if(xpl == true){
            xplTime -= Time.deltaTime;
            if(xplTime <= 0f){
                xpl = false;
            }
        }

        if (Input.GetKey("r") == true){
            ocOn = true;
            overchargeTime = 4;
        }
        if (Input.GetKey("e") == true){
            multishot = true;
            multishotTime = 8;
            //Debug.Log("e pressed");
            
        }
        if (Input.GetKey("f") == true){
            xpl = true;
            xplTime = 5;
            //Debug.Log("f pressed");
            
        }

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetKey("space") == true)
        {
            //spr.material.SetColor("_Color", newCol);
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
    }
}
