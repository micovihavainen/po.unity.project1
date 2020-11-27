using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opossum : MonoBehaviour
{
    public float leftCap = 1;
    public float rightCap = 1;

    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    public float speed = 3f;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (facingLeft == true)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            //Make sure sprite is facing right location, and if it is not, face the right direction
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(1, 1);
            }
        }
        else
        {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                //Make sure sprite is facing right location, and if it is not, face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
            }
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBound"))
        {
            facingLeft = false;
        }
        if (collision.gameObject.CompareTag("RightBound"))
        {
            facingLeft = true;
        }
    }
}