using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    public float size_scale = 1.25f;
    public float movement_radius = 0.25f;
    private int Count=0;
    //private Vector2 velocity = new Vector2(0,-1);
    private Vector2 original_position;

    void Start()
    {
        //maybe give a velocity to move toward player with
        this.gameObject.name = "Tank" + Count + 1;
        Count++;
        this.gameObject.tag = "Enemy";
        transform.localScale = new Vector3(size_scale,size_scale,size_scale);
        original_position = new Vector2(transform.position.x,transform.position.y);
        translational_velocity = new Vector2(0f,-2f);
        //theta = 2f;
    }
    void Update(){
        translational_velocity = new Vector2(translational_velocity.x * (0.1f*Time.deltaTime),translational_velocity.y *(0.1f*Time.deltaTime));
        //transform.position = new Vector2(transform.position.x + velocity.x,transform.position.y + velocity.y);
        moveCircles(original_position.x,original_position.y,movement_radius);
    }
}