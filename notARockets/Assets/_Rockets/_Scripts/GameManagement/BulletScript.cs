using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float speed = 2f;
    public float lifeDuration = 2f; // Время жизни пули

    public PlayerRotationScript player;
    public EnemyRotationScript enemy;
    public GameController gameController;

    public int NumberOfLevel;

    public GameObject ShipPrefab;
    public GameObject EnemyPrefab;

    public float bulletDamage;
    public int shieldDamageBoost = 1; // Коефициент урона щитам

    Rigidbody rb;
    Transform tr;


    private float lifeTimer; // Вспомогательный таймер жизни

    public string BulletTag = "FirstBossBullet";
    public string BossTag = "FirstBoss";

    //private Vector3 moveDirection;

    Vector3 movement;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (GameObject.FindGameObjectWithTag("Player").activeSelf)
        {
            //Debug.Log("FindPlayer!");
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRotationScript>();
        }

        if (gameObject.CompareTag("playerBullet"))
        {
            /* Подвязываем босса к пулям игрока в зависимости от уровня */
            NumberOfLevel = gameController.NumberOfLevel;

            if (NumberOfLevel == 0)
            {
                //Debug.Log("Level 0!");
                enemy = GameObject.FindGameObjectWithTag("FirstBoss").GetComponent<EnemyRotationScript>();
            }
            else if (NumberOfLevel == 1)
            {
                enemy = GameObject.FindGameObjectWithTag("SecondBoss").GetComponent<EnemyRotationScript>();
            }
            else if (NumberOfLevel == 2)
            {
                enemy = GameObject.FindGameObjectWithTag("ThirdBoss").GetComponent<EnemyRotationScript>();
            }

            ShipPrefab = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void OnEnable()
    {

        lifeTimer = lifeDuration;

        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        //moveDirection = ShipPrefab.transform.forward;
        // Debug.Log("Our Vectors:"+" x "+moveDirection.x+" y "+moveDirection.y+" z "+moveDirection.z);
    }


    void Update()
    {

        movement = tr.forward * speed * Time.deltaTime;
        //Debug.Log("Movement:" + movement);
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
        if (gameObject.CompareTag(BulletTag) && (other.CompareTag("Player")))
        {
            //Debug.Log("Got it!");
            player.BulletDamage(bulletDamage, shieldDamageBoost);

            gameObject.SetActive(false);
        }
        if (gameObject.CompareTag("playerBullet") && (other.CompareTag(BossTag)))
        {
            //Debug.Log("GodLike!");
            enemy.BulletDamage(bulletDamage, shieldDamageBoost);

            gameObject.SetActive(false);
        }
    }

}
