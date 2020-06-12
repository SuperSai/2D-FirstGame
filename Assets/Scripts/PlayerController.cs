using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public LayerMask ground;
    public Text cherryNum;
    public float speed;
    public float jumpForce;
    public int cherry;
    private bool isHurt;
    public AudioSource audioSource, hurtAudio, cherryAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
        }
        SwitchAnim();
    }

    private void Movement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var dir = Input.GetAxisRaw("Horizontal");
        //移动
        if (horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(dir));
        }

        if (dir != 0)
        {
            transform.localScale = new Vector3(dir, 1, 1);
        }
        //跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            audioSource.Play();
            anim.SetBool("jumping", true);
            anim.SetBool("crouch", false);
        }
        var vrtical = Input.GetAxisRaw("Vertical");
        if (vrtical == -1 && !anim.GetBool("jumping"))
        {
            anim.SetBool("crouch", true);
        }
        else if (vrtical == 0)
        {
            anim.SetBool("crouch", false);
        }
    }

    private void SwitchAnim()
    {
        anim.SetBool("idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            //因为在移动过程中碰到了怪物，这个值没变回0，所以就不会却换到站立状态，这里设置为0
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNum.text = cherry + "";
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //消滅敵人
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            //受傷
            else if (transform.position.x < other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > other.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
        }
    }
}
