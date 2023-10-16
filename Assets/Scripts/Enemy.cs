using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public bool can_shoot = false;
    public Bullet bullet;
    public float bullet_speed = 1f;
    float timeLeft = 2.0f;
    Vector3 firing_point_offset = new Vector3(0, -0.25f, 0);
    

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

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
        Bullet new_bullet = Instantiate(bullet, transform.position + firing_point_offset, Quaternion.identity);
        new_bullet.send_off(Vector2.down, bullet_speed);
        Debug.Log("CALLED FIRE");

    }

    public void set_can_shoot(bool val){
        this.can_shoot = val;
        Debug.Log("CALLED set_can_shoot");
        Debug.Log(this);
    }
    
}
