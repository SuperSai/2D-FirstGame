using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Death()
    {
        //查找对应的代码
        FindObjectOfType<PlayerController>().CherryCount();
        Destroy(gameObject);
    }
}
