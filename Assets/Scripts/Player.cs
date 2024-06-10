using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canMove = true;
    Rigidbody2D rb;
    public float speed;

    Vector2 moveDir = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        if (canMove)
        {
            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            rb.velocity = moveDir * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
