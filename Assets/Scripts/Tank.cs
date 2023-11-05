using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tank : Enemy 
{
    private float movement_radius = 0.5f;
    private float scale_scalar;
    public Tank(float x, float y, float radius){
        this.gameObject.name = "Tank";
        this.gameObject.tag = "Tank";
        scale_scalar = radius;
        transform.position = new Vector2(x,y);
        transform.localScale = new Vector3(scale_scalar,scale_scalar,scale_scalar);
    }

   void Start(){
        float vel_mag = 0.0025f;
        Vector2 velocity = new Vector2(0,-0.25f);
        transform.position =  new Vector2(transform.position.x+vel_mag * velocity.x,transform.position.y+vel_mag * velocity.y);
    }
    void Update(){
        moveCircles(transform.position.x,transform.position.y,movement_radius);
    }
}