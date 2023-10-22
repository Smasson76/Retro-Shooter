using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Bullet: MonoBehaviour{
    private Rigidbody bb;
    Vector3 pos;
    Vector3 vel;
    void start(){
        Bullet bullet = new Bullet();
        bb = bullet.AddComponent<Rigidbody>();
        bb.velocity=Vector3.down;
    }
}