using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rb;
    Transform tr;
    public Joystick joystick;
    //public float rotationSpeed;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + tr.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }



    
}
