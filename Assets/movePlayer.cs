using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    public float v_mag;
    // Start is called before the first frame update
    void Start()
    {
        v_mag = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        transform.Translate(x*v_mag,y*v_mag,0f);
    }
}
