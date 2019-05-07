using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    public EnemyRotationScript BossTarget;
    public PlayerRotationScript PlayerTarget;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameController gameController;

    public float moveSpeed;
    public float rotationSpeed;
    public float damage;
    public int shieldDamageBoost;

    public float lifeTime;
    public float searchTimeDelay;
    public float searchingTime;

    //public float switchsearchTime;
    public int NumberOfLevel;
    public string BossTag;

    Vector3 startMoveDirection;
    Vector3 searchDirection;
    Vector3 movement;
    Quaternion searchRotation;

    private float lifeTimer;
    private float searchTimer;
    private float searchingTimer;
    //private float switchsearchTimer;

    public bool searchFlag;

    public bool BossMissile;
    Rigidbody rb;
    Transform tr;

    void Awake()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        NumberOfLevel = gameController.NumberOfLevel;

        if (gameObject.CompareTag("PlayerMissile"))
        {
            BossMissile = false;
            if (NumberOfLevel == 0)
            {
                EnemyPrefab = GameObject.FindWithTag("FirstBoss");
                if (EnemyPrefab.activeSelf)
                    BossTarget = EnemyPrefab.GetComponent<EnemyRotationScript>();
                //BulletTag = "FirstBossBullet";
                BossTag = "FirstBoss";
            }
            else if (NumberOfLevel == 1)
            {
                EnemyPrefab = GameObject.FindWithTag("SecondBoss");
                if (EnemyPrefab.activeSelf)
                    BossTarget = EnemyPrefab.GetComponent<EnemyRotationScript>();
                //BulletTag = "SecondBossBullet";
                BossTag = "SecondBoss";
            }
            else if (NumberOfLevel == 2)
            {
                EnemyPrefab = GameObject.FindWithTag("ThirdBoss");
                if (EnemyPrefab.activeSelf)
                    BossTarget = EnemyPrefab.GetComponent<EnemyRotationScript>();
                //BulletTag = "ThirdBossBullet";
                BossTag = "ThirdBoss";
            }
        }
        else
        {
            BossMissile = true;
            PlayerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (PlayerPrefab.activeSelf)
                PlayerTarget = PlayerPrefab.GetComponent<PlayerRotationScript>();
        }

    }

    void OnEnable()
    {

        lifeTimer = lifeTime;
        searchTimer = searchTimeDelay;
        searchingTimer = searchingTime;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        startMoveDirection = tr.forward;
        searchFlag = false;
    }


    void Update()
    {


        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }

        searchTimer -= Time.deltaTime;

        //movement = startMoveDirection * moveSpeed * Time.deltaTime;

        if (searchTimer <= 0)
        {
           
            //searchDirection = Vector3.MoveTowards(tr.position, tr.forward , moveSpeed * Time.deltaTime);
            if (BossMissile)
            {
                searchRotation = Quaternion.LookRotation(PlayerTarget.transform.position - tr.position, tr.up);
            }
            else
            {
                searchRotation = Quaternion.LookRotation(BossTarget.transform.position - tr.position, tr.up);
            }
            tr.rotation = Quaternion.Slerp(tr.rotation, searchRotation, rotationSpeed * Time.deltaTime);
            movement = tr.forward * moveSpeed * Time.deltaTime;
            searchFlag = true;
        }


    }

    void FixedUpdate()
    {
        if (searchFlag)
        {

            rb.MovePosition(tr.position + movement);

            searchingTimer -= Time.deltaTime;

            if (searchingTimer <= 0)
            {
                searchTimer = searchTimeDelay;
                movement = tr.forward * moveSpeed * Time.deltaTime;
                searchingTimer = searchingTime;
                searchFlag = false;

            }

            //rb.MovePosition(tr.position + tr.forward);
        }
        else
            rb.MovePosition(tr.position + movement);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!BossMissile && other.CompareTag(BossTag))
        {
            BossTarget.MissileDamage(damage, shieldDamageBoost);
            gameObject.SetActive(false);

        }

        if (BossMissile && other.CompareTag("Player"))
        {
            PlayerTarget.MissileDamage(damage, shieldDamageBoost);
            gameObject.SetActive(false);
        }

    }
}
