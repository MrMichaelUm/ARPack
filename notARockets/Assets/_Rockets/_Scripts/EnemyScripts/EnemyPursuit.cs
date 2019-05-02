using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyPursuit : MonoBehaviour
{
    public Rigidbody player;
    Rigidbody rb;
    Transform tr;
    //GameObject gm;
    public float speed;
    public Slider healthBar;
    public float stoppingDistance;
    public float retreatingDistance;

    private Vector3 moveDirection;
    private float health;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
       // gm = GetComponent<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(player.position.z, 0, player.position.x);
        health = healthBar.value;
        if (health <= 50)
        {
            speed = 16;
            stoppingDistance = 7;
            retreatingDistance = 5;
        }
        
    }
    void FixedUpdate()
    {
        
        if (Vector3.Distance(tr.position,player.position) >= stoppingDistance)
        {

            tr.position = Vector3.MoveTowards(tr.position, player.position, speed * Time.deltaTime);
            //rb.MovePosition(rb.position + tr.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }
        else if ((Vector3.Distance(tr.position, player.position) < stoppingDistance)&&(Vector3.Distance(tr.position, player.position) >= retreatingDistance))
        {
            tr.position = this.transform.position;
        }
        else if (Vector3.Distance(tr.position, player.position) < retreatingDistance)
        {
            tr.position = Vector3.MoveTowards(tr.position, player.position, -speed * Time.deltaTime);
            //rb.MovePosition(rb.position - tr.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }


        
    }
}
