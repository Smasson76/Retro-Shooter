using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public bool is_clicked;
    ShipSelector obj;
    void Start()
    {
        is_clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_clicked){
            obj.Play();
        }
        is_clicked = false;
    }
    public void has_been_clicked(){
        is_clicked = !is_clicked;
    }
}
