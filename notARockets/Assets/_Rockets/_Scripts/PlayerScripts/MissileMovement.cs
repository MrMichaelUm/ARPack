using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    public EnemyRotationScript enemyTarget;
    public GameObject PlayerPrefab;
    public float moveSpeed;
    public float rotationSpeed;
    public float damage;
    public int shieldDamageBoost;
    public float lifeTime;
    public float searchTimeDelay;
    public float searchingTime;
    public float switchsearchTime;

    Vector3 startMoveDirection;
    Vector3 searchDirection;
    Vector3 movement;
    Quaternion searchRotation;

    private float lifeTimer;
    private float searchTimer;
    private float searchingTimer;
    private float switchsearchTimer;
    public bool searchFlag;
    Rigidbody rb;
    Transform tr;

    void Start()
    {
        
        

    }

    void OnEnable()
    {
        lifeTimer = lifeTime;
        searchTimer = searchTimeDelay;
        searchingTimer = searchingTime;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        if (enemyTarget.gameObject.activeSelf)
        {
            enemyTarget = GameObject.FindWithTag("Enemy").GetComponent<EnemyRotationScript>();
        }
        startMoveDirection = PlayerPrefab.transform.forward;
        searchFlag = false;
    }

    
    void Update()
    {
        movement = transform.TransformDirection(startMoveDirection) * moveSpeed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
        
        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0)
        {

            //searchDirection = Vector3.MoveTowards(tr.position, tr.forward , moveSpeed * Time.deltaTime);
            searchRotation = Quaternion.LookRotation(enemyTarget.transform.position - tr.position, tr.up);
            tr.rotation = Quaternion.Slerp(tr.rotation, searchRotation, rotationSpeed * Time.deltaTime);
            movement = tr.forward*moveSpeed*Time.deltaTime;
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
        if (other.CompareTag("Enemy"))
        {
            enemyTarget.MissileDamage(damage, shieldDamageBoost);
            gameObject.SetActive(false);

        }

    }
}
