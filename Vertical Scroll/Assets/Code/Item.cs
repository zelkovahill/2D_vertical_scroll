using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type;
    private Rigidbody2D rb;

    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 1f;
    }

    
}
