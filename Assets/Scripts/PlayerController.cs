﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public LayerMask ground;
    public float speed;
    public float jumpForce;
    public int cherry;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
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

        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
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
            Destroy(collision.gameObject);
            cherry += 1;
        }
    }
}
