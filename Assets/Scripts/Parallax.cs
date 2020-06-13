using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float startPointX, startPointY;
    public bool lockY;

    void Start()
    {
        startPointX = transform.position.x;
    }

    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY + cam.position.y * moveRate);
        }
        //这样游戏就会降速到0，不会动了
        // Time.timeScale = 0f;
        //组件Platform Effector 2D可是实现从下面跳跃到平台上面，但是要把Collider2D中的Used By Effector 勾选上
    }
}
