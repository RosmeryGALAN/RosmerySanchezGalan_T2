using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNinjaMan : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Transform _transform;

    private int vidas = 3;

    public GameObject ShurikenRigth;
    public GameObject ShurikenLeft;

    public VidaText Vida;
    public ControlPuntaje Puntaje;


    private bool trepar = false;
    private bool muerte = false;
    private bool planear = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetInteger("Estado", 0);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(10f, rb.velocity.y);
            animator.SetInteger("Estado", 1);
            sr.flipX = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-10f, rb.velocity.y);
            animator.SetInteger("Estado", 1);
            sr.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(rb.velocity.x, 30f), ForceMode2D.Impulse);
            animator.SetInteger("Estado", 2);
        }
        if (Input.GetKey(KeyCode.DownArrow))
            animator.SetInteger("Estado", 4);
        if (trepar)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            animator.SetInteger("Estado", 5);
            if (Input.GetKey(KeyCode.UpArrow))
                rb.velocity = new Vector2(rb.velocity.x, 10f);
            if (Input.GetKey(KeyCode.DownArrow))
                rb.velocity = new Vector2(rb.velocity.x, -10f);
        }
        if (!trepar)
            rb.gravityScale = 10;

        if (Input.GetKeyUp("x"))
        {
            animator.SetInteger("Estado", 3);
            if (!sr.flipX)
            {
                var KunaiPosition = new Vector3(_transform.position.x + 3f, _transform.position.y, _transform.position.z);
                Instantiate(ShurikenRigth, KunaiPosition, Quaternion.identity);
            }
            if (sr.flipX)
            {
                var KunaiPosition = new Vector3(_transform.position.x - 3f, _transform.position.y, _transform.position.z);
                Instantiate(ShurikenLeft, KunaiPosition, Quaternion.identity);
            }
        }

        if (muerte)
            animator.SetInteger("Estado", 7);

        if (Input.GetKey("c") && planear)
        {
            rb.gravityScale = 1;
            animator.SetInteger("Estado", 6);
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(10f, -10f);
                sr.flipX = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-10f, -10f);
                sr.flipX = true;
            }
        }

        Caida();
    }

    private void Caida()
    {
        if (rb.velocity.y < -60)
            muerte = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            vidas--;
            if (vidas == 0) muerte = true;
            if (vidas >= 0)
            {
                Vida.QuitarVida(1);
                Debug.Log(Vida.GetVida());
            }

        }
        if (collision.gameObject.layer == 8)
            planear = false;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
            planear = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Escalera")
            trepar = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Escalera")
            trepar = false;
    }
}
