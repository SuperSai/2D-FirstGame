using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource audioSource;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        audioSource.Play();
        anim.SetTrigger("death");
    }
}
