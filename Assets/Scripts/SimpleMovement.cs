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

    public float xplTime;
    public float overchargeTime;
    public float multishotTime;
    public float ocStart;

    public Bullet bulletPrefab;

    void Fire()
	{
        if((Time.time - lastShotTime) > cooldownTime){
            if(GameManager.instance.multishot == true){
                if(multishotTime > 0f){
                    Bullet bullet1 = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    Bullet bullet2 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(-1f, 0.5f, 0), Quaternion.identity);
                    Bullet bullet3 = Instantiate(this.bulletPrefab, this.transform.position + new Vector3(1f, 0.5f, 0), Quaternion.identity);
                    bullet1.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);
                    bullet2.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);
                    bullet3.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);

                    lastShotTime = Time.time;
                } else {
                    GameManager.instance.multishot = false;
                    multishotTime = 0;
                    Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                    bullet.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);

                    lastShotTime = Time.time;
                }
            } else {
                Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position + firing_point_offset, Quaternion.identity);
                bullet.send_off(Vector2.up, bullet_speed, GameManager.instance.xpl);

                lastShotTime = Time.time;
            }
        }
	}

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        if(GameManager.instance.multishot == true){
            multishotTime -= Time.deltaTime;
            if (multishotTime <= 0f)
            {
                GameManager.instance.multishot = false;
                GameManager.instance.MultiShotPowerUpImage.SetActive(false);
                multishotTime = 0;
            }
        }
        if(GameManager.instance.ocOn == true) {
            overchargeTime -= Time.deltaTime;
            cooldownTime = .25f;
            if (overchargeTime <= 0f)
            {
                GameManager.instance.ocOn = false;
                GameManager.instance.OverchargePowerUpImage.SetActive(false);
                cooldownTime = .5f;
                overchargeTime = 0;
            }
        }
        if(GameManager.instance.xpl == true){
            xplTime -= Time.deltaTime;
            if(xplTime <= 0f){
                GameManager.instance.xpl = false;
                GameManager.instance.ExplosivePowerUpImage.SetActive(false);
                xplTime = 0;
            }
        }

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetKey("space") == true)
        {
            //spr.material.SetColor("_Color", newCol);
            //if(GameManager.instance.ocOn == true){
                //overchargeTime -= Time.deltaTime;
                //if(overchargeTime > 0.0f){
                    //cooldownTime = .1f;
                //} else {
                    //cooldownTime = .5f;
                    //GameManager.instance.ocOn = false;
                //}
            //}
            Fire();
        }
    }
}
