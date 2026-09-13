using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;


    Animator ani;

    bool cuan = false;


    public GameObject player2;
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        if (move >= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (move == 0)
        {
            ani.SetBool("run", false);
        }
        else
        {
            ani.SetBool("run",true);
        }
        if (cuan == false) {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            cuan = true;

            if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
            {
                rb.AddForce(new Vector2(25f, 0), ForceMode2D.Impulse);
            }
            else {

                rb.AddForce(new Vector2(-25f, 0), ForceMode2D.Impulse);
            }

            
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            cuan = false;
            
        }


        if (isGrounded == false)
        {
            //ani.SetBool("jump", true);
        }
        else
        {
            //ani.SetBool("jump", false);
        }

    }
    IEnumerator Afun()
    {

        yield return new WaitForSeconds(3f);
        player2.SetActive(true);
        player2.transform.position = new Vector3(gameObject.transform.position.x,-2.69f,0);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "door") {
            collision.GetComponent<Animator>().enabled = true;

            StartCoroutine(Afun());

            

        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("item"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("item"))
        {
            isGrounded = false;
        }
    }
}