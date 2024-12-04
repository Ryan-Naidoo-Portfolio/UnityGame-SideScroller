using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDestroy : MonoBehaviour
{   
    public GameObject Object;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            Object.SetActive(false);
        }
    }
}
