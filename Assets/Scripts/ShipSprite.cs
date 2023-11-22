using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSprite : MonoBehaviour
{
	private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
		parent = transform.parent.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void on_player_entered(){
		parent.GetComponent<SimpleMovement>().on_player_entered();
	}
}
