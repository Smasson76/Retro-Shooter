using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    
    private void Update(){
        this.transform.position += this.direction * this.speed * Time.deltaTime;
        
    }
}
