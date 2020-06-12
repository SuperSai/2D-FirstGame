using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D disColl;
    public Transform cellingCheck;
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
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(dir));
        }

        if (dir != 0)
        {
            transform.localScale = new Vector3(dir, 1, 1);
        }
        //跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            audioSource.Play();
            anim.SetBool("jumping", true);
            anim.SetBool("crouch", false);
        }
        Crouch();
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

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Collection")
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNum.text = cherry + "";
        }
        //游戏重新开始
        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            //延迟2秒重新刷新游戏
            Invoke("Restart", 1f);
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

    //蹲下
    void Crouch()
    {
        //Physics2D.OverlapCircl：意思是说检查某个点的圆形上是否碰到某个图层
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                disColl.enabled = false;
                anim.SetBool("crouch", true);
            }
            else
            {
                disColl.enabled = true;
                anim.SetBool("crouch", false);
            }
        }
    }

    void Restart()
    {
        //GetActiveScene().name:获取当前使用的场景的名字
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
