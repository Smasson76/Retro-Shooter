using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed;
    private float Move;
    private Color newCol;
    private Color oldCol;

    public Bullet bulletPrefab;

    void Fire()
	{
		Instantiate(this.bulletPrefab, this.transform.position, Quaternion.identity);
	}

    //public String powerType;

    private Rigidbody2D rb;
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        
        oldCol = spr.material.GetColor("_Color");
        newCol = new Color(0.4f, 0.9f, 0.7f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetKey("space") == true)
        {
            spr.material.SetColor("_Color", newCol);
            Fire();
            Debug.Log("PEW");
        }
        if (Input.GetKey("space") == false)
        {
            spr.material.SetColor("_Color", oldCol);
            Debug.Log("Silence");
        }
    }
}
