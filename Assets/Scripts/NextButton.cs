using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
     ShipSelector obj;
    public bool is_clicked;
    void Start()
    {
        is_clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_clicked){
            is_clicked = !is_clicked;
        }
        /*if(is_clicked){
            obj.Next();
        }
        is_clicked = false;*/
    }
    public void has_been_clicked(){
        is_clicked = !is_clicked;
        obj.Next();
    }
}
