using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tank : Enemy 
{
    private float movement_radius = 0.5f;
    private float scale_scalar;
    public Tank(float x, float y, float radius){
        //GameObject.tag = "Enemy";
        scale_scalar = radius;
        transform.position = new Vector2(x,y);
        transform.localScale = new Vector3(scale_scalar,scale_scalar,scale_scalar);
    }

   void Start(){
        float vel_mag = 1f;
        Vector2 velocity = new Vector2(0,-1);
        transform.position = new Vector2(vel_mag * velocity.x*Time.deltaTime,vel_mag * velocity.y*Time.deltaTime);
    }
   /* void Update(){
        moveCircles(transform.position.x,transform.position.y,movement_radius);
    }*/
}