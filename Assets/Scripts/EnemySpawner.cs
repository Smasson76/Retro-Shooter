using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemy;
    public List<List<Enemy>> enemy_columns = new List<List<Enemy>>(); 
    public int swarm_dim_x = 4;
    public int swarm_dim_y = 5;
    public int swarm_width = Screen.width;
    public Foes[] foeList;
    private int myTanks=0;
    public float SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        place_swarm(swarm_dim_x, swarm_dim_y, swarm_width);
        setSpawnTStamp();
    }

    void Update()
    {
        if (GameManager.instance.enemyCount <= 0)
        {
            GameManager.instance.enemyCount = 0;
            GameManager.instance.SpawnEnemy();
            Destroy(this.gameObject, 2f);
        }
    }

    void place_swarm(int dim_x, int dim_y, int width){
        
        float swarm_width = width; 
        
        float swarm_half_width = (float) swarm_width / 2;
        int swarm_col_num =  dim_y;
        int swarm_row_num =  dim_x;
        float swarm_step = (float) (swarm_width / swarm_row_num);

        float base_x_placement = (float) this.transform.position.x - swarm_half_width;
        float base_y_placement = (float) this.transform.position.y;

        Vector3 enemy_placement = new Vector3(base_x_placement, base_y_placement, 0);

        for (int col_idx = 0; col_idx < swarm_col_num; col_idx++){
            enemy_columns.Insert(col_idx, new List<Enemy>());
            for (int row_idx = 0; row_idx < swarm_row_num; row_idx++){
                float key = (float) UnityEngine.Random.Range(1,10)/10;
                if(key < 0.2f){
                    enemy = selectType("Tank");
                    myTanks++;
                }
                else{
                    enemy = selectType("Monkey");
                }
                enemy_placement = new Vector3(
                   base_x_placement + (swarm_step *  col_idx + enemy.transform.localScale.x),
                   base_y_placement + (swarm_step * (row_idx % swarm_row_num)+enemy.transform.localScale.y),
                   0
                );

                Enemy enemy_i = Instantiate(enemy, enemy_placement, Quaternion.identity);
                enemy_i.transform.SetParent(this.transform);
                enemy_columns[col_idx].Insert(row_idx % swarm_row_num, enemy_i);
                GameManager.instance.enemyCount++;
            }
        }

    }
    public Enemy selectType(string name){
        Foes s = Array.Find(foeList, x => x.type == name);
        if(s == null){
            //Debug.Log("Type not found!");
        }
        else{
            enemy = s.Enemy_Variant;
        }
        return enemy;
    }
    public void setSpawnTStamp(){
        foreach(List<Enemy>sub in enemy_columns){
            foreach(Enemy enemy in sub){
                enemy.setSpawnTime(Time.time);
            }
        }
    }
}
