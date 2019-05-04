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

    public Slider healthBar;

    public float stoppingDistance;
    public float retreatingDistance;

    private Vector3 movement;
    private float health;

    private Quaternion moveRotation;
    private float originStoppingDistance;
    private float originRetreatingDistance;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        originStoppingDistance = stoppingDistance;     //Сохраняем изначальную дистанцию остановки
        originRetreatingDistance = retreatingDistance; //И дистанцию отступления

        
    }
    void Update()
    {
        moveRotation = Quaternion.LookRotation(player.transform.position - tr.position, tr.up);  //Нахоим нужное направление поворота на цель
        tr.rotation = Quaternion.Slerp(tr.rotation, moveRotation, rotationSpeed * Time.deltaTime);  //Плавно нводимся на цель с заданной угловой скоростью

        health = healthBar.value;               //Следим за уровнем здоровья для изменеия поведения

        /* Добавляем агрессии в поведение противника */
        if (health <= 50)
        {
            speed = 16;
            rotationSpeed = 10;
            stoppingDistance = originStoppingDistance/2+2f;
            retreatingDistance = originRetreatingDistance/2+2f;
        }

        movement = tr.forward * speed * Time.deltaTime;  //Движение происходит вперёд
    }
    void FixedUpdate()
    {
       
        /* Двигаемся через физику, т.к. нас притягивает планета. Задаём характер движения в зависимости от расстояния до игрока */

        if (Vector3.Distance(tr.position,player.position) >= stoppingDistance)
        {

            rb.MovePosition(tr.position + movement);   

        }
        else if ((Vector3.Distance(tr.position, player.position) < stoppingDistance)&&(Vector3.Distance(tr.position, player.position) >= retreatingDistance))
        {
            tr.position = this.transform.position;
        }
        else if (Vector3.Distance(tr.position, player.position) < retreatingDistance)
        {
            rb.MovePosition(tr.position - movement);
        }


        
    }
}
