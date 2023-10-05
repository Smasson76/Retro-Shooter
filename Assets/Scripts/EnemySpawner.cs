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
        place_swarm(16);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void place_swarm(int enemy_number){
        // The enemy_number must have a whole number square-root
        // At some point, we should do some error handling here to enforce this.


        int screen_width = 10; //Screen.width;
        float swarm_width = (float) ((screen_width / 3f) * 2);
        
        float swarm_half_width = (float) swarm_width / 2;
        int swarm_row_num = (int) Mathf.Sqrt(enemy_number);
        float swarm_step = (float) (swarm_width / swarm_row_num);

        int row_idx = 0;
        float base_x_placement = (float) this.transform.position.x - swarm_half_width;
        float base_y_placement = (float) this.transform.position.y;

        Vector3 enemy_placement = new Vector3(base_x_placement, base_y_placement, 0);
        enemy_columns.Insert(0, new List<Enemy>());

        for (int i = 0; i < enemy_number; i++){
             if (((i % swarm_row_num) == 0) && (i != 0)){
                 row_idx++; 
                 enemy_columns.Insert(row_idx, new List<Enemy>());
             }    
             Debug.Log(String.Format("\nCOL: {0}, ROW: {1}", row_idx, i % swarm_row_num));
             enemy_placement = new Vector3(
                base_x_placement + (swarm_step *  row_idx),
                base_y_placement + (swarm_step * (i % swarm_row_num)),
                0
             );

             Debug.Log(enemy_placement);

             Enemy enemy_i = Instantiate(enemy, enemy_placement, Quaternion.identity);

             enemy_columns[row_idx].Insert(i % swarm_row_num, enemy_i);
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
