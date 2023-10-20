using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    //no constructor -> instanced with instansiate
    Enemy enemy;
    Rigidbody rb;
    Bullet bullet;
    public string id;
    
    // Update is called once per frame
    void start(){
        enemy.AddComponent<Rigidbody>();
        rb.velocity=Vector3.zero;
        enemy.shoot();
    }
    void Update()
    {
        //enemy.shoot();
    }
    
    public Vector3 place(float xpos, float ypos, float zpos){
        return rb.position = new Vector3(rb.position.x+ xpos, rb.position.y + ypos , rb.position.z + zpos);
    }
    public Bullet shoot(){
        return Instantiate(bullet);
    }
    
    void setid(string input){
        this.id = input;
    }
    string getid(){
        return this.id;
    }
}
