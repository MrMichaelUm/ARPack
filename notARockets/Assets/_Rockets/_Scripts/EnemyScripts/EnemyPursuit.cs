using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyPursuit : MonoBehaviour
{
    public Rigidbody player;
    Rigidbody rb;
    Transform tr;

    public float speed;
    public float rotationSpeed;
    public float stoppingDistance;
    public float retreatingDistance;

    private Vector3 movement;

    private Quaternion moveRotation;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<Rigidbody>();
    }
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

    }
    void Update()
    {
        if (player.gameObject.activeSelf)
        {
            moveRotation = Quaternion.LookRotation((player.transform.position - tr.position)/2, tr.up);  //Нахоим нужное направление поворота на цель
            tr.rotation = Quaternion.Slerp(tr.rotation, moveRotation, rotationSpeed * Time.deltaTime);  //Плавно нводимся на цель с заданной угловой скоростью

            movement = tr.forward * speed * Time.deltaTime;  //Движение происходит вперёд
        }
    }
    void FixedUpdate()
    {

        /* Двигаемся через физику, т.к. нас притягивает планета. Задаём характер движения в зависимости от расстояния до игрока */
        if (player.gameObject.activeSelf)
        {
            if (Vector3.Distance(tr.position, player.position) >= stoppingDistance)
            {

                rb.MovePosition(tr.position + movement);

            }
            else if ((Vector3.Distance(tr.position, player.position) < stoppingDistance) && (Vector3.Distance(tr.position, player.position) >= retreatingDistance))
            {
                tr.position = this.transform.position;
            }
            else if (Vector3.Distance(tr.position, player.position) < retreatingDistance)
            {
                rb.MovePosition(tr.position - movement);
            }


        }
    }
}
