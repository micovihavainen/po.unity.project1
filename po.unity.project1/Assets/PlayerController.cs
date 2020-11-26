using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRb;
    private Animator PlayerAnim;
    public float speed = 10;
    public float jumpforce = 12;
    private bool isGrounded = true;

    public Transform explosion;

    public int health = 5;
    public TextMeshProUGUI healthtext;

    public TextMeshProUGUI cherrytext;
    public int cherries = 0;
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
            explosion.GetComponent<ParticleSystem>().Play(explosion);
        }

        // Ground check fix
        if (Input.GetKeyDown(KeyCode.S) && isGrounded == false)
        {
            isGrounded = true;
            PlayerAnim.SetBool("Jumping", false);
        }
    }

    // Collision for collectibles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy collectible when collided with and update text
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherrytext.text = cherries.ToString();
        }
    }
    // Collision mechanics
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground check / disabling hurt and enabling movement when landing
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            PlayerAnim.SetBool("Jumping", false);
            speed = 10;
            PlayerAnim.SetBool("Hurting", false);
        }

        // Enemy behaviour

        // Enemy dies when player is jumping and colliding
        if (collision.gameObject.CompareTag("Enemy") && isGrounded == false)
        {
            Destroy(collision.gameObject);
            PlayerRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }

        // When player is moving from the right, player gets thrown left and up, while playing "hurt" animation
        // Player speed is disabled and isGrounded = false is preventing the player from jumping
        else if (collision.gameObject.CompareTag("Enemy") && Input.GetAxis("Horizontal") > 0)
        {
            PlayerRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            PlayerRb.AddForce(Vector2.left * jumpforce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Hurting", true);
            speed = 0;
            isGrounded = false;
            health--;
            healthtext.text = health.ToString();
        }

        // When player is moving from the left, player gets thrown right and up, while playing "hurt" animation
        // Player speed is disabled and isGrounded = false is preventing the player from jumping
        else if (collision.gameObject.CompareTag("Enemy") && Input.GetAxis("Horizontal") < 0)
        {
            PlayerRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            PlayerRb.AddForce(Vector2.right * jumpforce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Hurting", true);
            speed = 0;
            isGrounded = false;
            health--;
            healthtext.text = health.ToString();
        }
    }
}
