using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordController : MonoBehaviour
{
    Animator animator;
    public GameObject Player1;
    public GameObject Actualcamera1;
    public GameObject Virtualcamera1;

    public GameObject Player2;
    public GameObject Actualcamera2;
    public GameObject Virtualcamera2;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            animator.SetBool(AnimationStrings.SwordPickup, true);
            Player2.SetActive(false);
            Actualcamera2.SetActive(false);
            Virtualcamera2.SetActive(false);
            Player1.SetActive(true);
            Actualcamera1.SetActive(true);
            Virtualcamera1.SetActive(true);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    
    
}
