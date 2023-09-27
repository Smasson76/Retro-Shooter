using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    private float nextShootTime = 0.0f;
    private float period = 0.1f;
    
    private void Update(){
        if(Time.time > nextShootTime){
            nextShootTime += period;
            this.transform.position += this.direction * this.speed * Time.deltaTime;
        }
    }
}
