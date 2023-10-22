using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner: MonoBehaviour{
    EnemySpawner spawner;
    private List<Enemy> enemyList;
    private Enemy enemy;
    private Rigidbody rb;
    public const int qty = 16;
    int s_width = Screen.width/((int)Mathf.Sqrt(qty));
    
    void start(){
        enemyList = new List<Enemy>();
        rb.velocity = Vector3.zero;
        //enemyList.Add(Spawn());
        Spawn();
    }

    public Enemy[] Spawn(){
        Enemy[] spawns=new Enemy[qty];
        int row_num = (int)Mathf.Floor(Mathf.Sqrt(qty));
        print(row_num);
        for(int i=0;i<row_num;i++)
        {
            for(int j = 0;j<row_num;j++){
                spawns[i+j]=Instantiate(enemy,new Vector3(rb.position.x + (i+j)*s_width,rb.position.y+(i*j)*s_width,0),Quaternion.identity);
                enemyList.Add(spawns[i+j]);
            }
        }
        return spawns;
    }
}