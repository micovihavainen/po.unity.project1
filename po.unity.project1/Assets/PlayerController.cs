using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D PlayerRb;
    private Animator PlayerAnim;

    private float speed = 10;
    private float jumpforce = 12;
    private bool isGrounded = true;

    private int health = 3;
    public TextMeshProUGUI healthtext;
    private int cherries = 0;
    public TextMeshProUGUI cherrytext;
    private int gems = 0;
    public TextMeshProUGUI gemtext;
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
        // Collectibles
        // Destroy collectible when collided with and update text
        if (collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherrytext.text = cherries.ToString();
        }
        else if (collision.tag == "Gem")
        {
            Destroy(collision.gameObject);
            gems += 1;
            gemtext.text = gems.ToString();
        }

        // Deathzones
        // Disable movement and slow falling, play hurt animation
        if (collision.gameObject.CompareTag("Deathzone"))
        {
            PlayerAnim.SetBool("Hurting", true);
            PlayerRb.drag = 25;
            speed = 0;
            jumpforce = 0;
            health--;
            healthtext.text = health.ToString();
        }

        //DoorFinish
        if (collision.gameObject.CompareTag("Finish") && cherries == 5 && gems == 5)
        {
            SceneManager.LoadScene("Finish");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Deathzone"))
        {
            PlayerAnim.SetBool("Hurting", true);
        }

        if (collision.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        // Teleport player back to spawn, enable movement, disable hurting anim
        if (collision.CompareTag("Deathzone") && health > 0)
        {
            transform.position = new Vector2(-3, -2);
            jumpforce = 12;
            PlayerRb.drag = 0;
            PlayerAnim.SetBool("Hurting", false);
        }

        // When exiting deathzone with 0 health, destroy player and load main menu
        if (collision.CompareTag("Deathzone") && health == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
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
            if (health == 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("MainMenu");
            }
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
