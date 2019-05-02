using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    
    public float speed = 2f;
    public float lifeDuration = 2f;
    public PlayerRotationScript player;
    public EnemyRotationScript enemy;
    public GameObject PlayerPrefab;
    public float bulletDamage;
    public int shieldDamageBoost = 1;
    Rigidbody rb;
    Transform tr;

    
    private float lifeTimer;
    
    private Vector3 moveDirection;
    Vector3 movement;

    void OnEnable ()
    {
        lifeTimer = lifeDuration;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        if (player.gameObject.activeSelf)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerRotationScript>();
        }
        if (enemy.gameObject.activeSelf)
        {
            enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyRotationScript>();
        }
        
        moveDirection = PlayerPrefab.transform.forward;
       // Debug.Log("Our Vectors:"+" x "+moveDirection.x+" y "+moveDirection.y+" z "+moveDirection.z);
    }
    void Update()
    {

        movement = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;
        
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(tr.position + movement);        
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("enemyBullet")&&(other.CompareTag("Player")))
        {
            //Debug.Log("Got it!");
            player.BulletDamage(bulletDamage, shieldDamageBoost);
            gameObject.SetActive(false);
        }
        if (gameObject.CompareTag("playerBullet") && (other.CompareTag("Enemy")))
        {
            Debug.Log("GodLike!");
            enemy.BulletDamage(bulletDamage, shieldDamageBoost);
            gameObject.SetActive(false);
        }
    }
    
}
