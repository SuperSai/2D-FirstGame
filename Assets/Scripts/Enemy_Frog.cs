using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform leftPoint, rightPoint;
    public float speed = 200;
    private bool faceLeft = true;
    private float leftX, rightX;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //断绝子类的关系
        // transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
        }
    }
}
