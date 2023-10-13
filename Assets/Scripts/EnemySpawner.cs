using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemy;

    public List<List<Enemy>> enemy_columns = new List<List<Enemy>>(); 


    // Start is called before the first frame update
    void Start()
    {
        place_swarm(4, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void place_swarm(int dim_x, int dim_y){
        int screen_width = 10; //Screen.width;
        float swarm_width = (float) ((screen_width / 3f) * 2);
        
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
                enemy_placement = new Vector3(
                   base_x_placement + (swarm_step *  col_idx),
                   base_y_placement + (swarm_step * (row_idx % swarm_row_num)),
                   0
                );

                Enemy enemy_i = Instantiate(enemy, enemy_placement, Quaternion.identity);
                enemy_i.transform.SetParent(this.transform);
                enemy_columns[col_idx].Insert(row_idx % swarm_row_num, enemy_i);
            }
        }

        check_enemies_can_shoot(); 

    }

    void check_enemies_can_shoot(){
        float lowest_y_val = 100f;

        // This might have an issue when Enemies die. Does the list actually get shortened?
        // Or does that index have a null value now?
        // If the list actually gets shortened, the best way to do this is to just grab the enemy
        // at the end of each column/list
        for (int i = 0; i < enemy_columns.Count; i++){
            int lowest_j = 0;
            for (int j = 0; j < enemy_columns[i].Count; j++){
                float current_enemy_y_pos = enemy_columns[i][j].transform.position.y;
                if ( current_enemy_y_pos < lowest_y_val){
                    lowest_y_val = current_enemy_y_pos;
                    lowest_j = j;
                }
            }
            enemy_columns[i][lowest_j].set_can_shoot(true);
        }
    }
}
