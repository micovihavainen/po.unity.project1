using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRb;
    private Animator PlayerAnim;
    public float speed = 20;
    public float jumpforce = 5;
    private bool isGrounded = true;
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
    }
    void Update()
    {
        // Horizontal movement based on A,D
        float HorizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * HorizontalInput * speed * Time.deltaTime);

        // Running and idle animation depending on horizontal movement
        if (HorizontalInput != 0)
        {
            PlayerAnim.SetBool("Running", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", false);
        }
        
        // Turning when going to the opposite direction
        if (HorizontalInput > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (HorizontalInput < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // Jumping mechanics
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            PlayerRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Jumping", true);
            isGrounded = false;
        }
    }

    // Ground check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            PlayerAnim.SetBool("Jumping", false);
        }
    }
}
