using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyRotationScript : MonoBehaviour
{

    Transform tr;
    Rigidbody rb;
    GameObject gm;
    public Transform player;
    public Transform ShootEmitter;
    public Transform enemyparent;
    public Transform playerparent;
    public float eulerSpeed;
    public float enemyHealth;
    public float enemyShield;

    public GameObject BulletPrefab;
    public Slider enemyHealthBar;
    public Slider enemyShieldBar;
    public GameObject ShieldPrefab;

    private float enemyXRotation;
    private Vector3 enemyLookPosition;
    private Vector3 playerLookPosition;
    private Vector3 targetVector;
    private Vector3 newDir;
    private Vector3 enemyUp;

    private float timeBetweenShots;
    public float startBetweenShots;

    private Vector3 currentRotateDirection;
    private Vector3 previousRotateDirection;
    private Quaternion slerpRotation;
    private Quaternion targetRotation;
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        gm = GetComponent<GameObject>();
        ShieldPrefab.SetActive(true);
        timeBetweenShots = startBetweenShots;
        tr.rotation = enemyparent.rotation;

    }

    void Update()
    {



        //enemyXRotation = transform.rotation.x;
        //enemyLookPosition = new Vector3(transform.position.x, 0, transform.position.z);
        //playerLookPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);       
        //slerpRotation.x = enemyparent.transform.rotation.x;

        if (player.gameObject.activeSelf)
        {
            targetRotation = Quaternion.LookRotation((player.position - transform.position));
            slerpRotation = Quaternion.Euler(enemyparent.localRotation.x, targetRotation.y, enemyparent.localRotation.z);
            tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, eulerSpeed * Time.deltaTime);
        }



        //Twist();

        //targetVector = player.position - transform.position;
        //slerpRotation = Quaternion.AngleAxis(Mathf.Atan2(targetVector.y, targetVector.x)*Mathf.Rad2Deg , transform.up);
        //transform.rotation =new Quaternion(enemyparent.rotation.normalized.x, slerpRotation.normalized.y, enemyparent.rotation.normalized.z, 0.5f);


        if (enemyHealth <= 50f)
        {
            eulerSpeed = 12;
            
            startBetweenShots = 0.5f;
        }

        if (timeBetweenShots <= 0)
        {
            Shoot();
            timeBetweenShots = startBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

    }
    void FixedUpdate()
    {
        
        
    }

    public void Shoot()
    {
        GameObject bulletObject = ObjectPoolingManager.Instance.GetEnemyBullet(BulletPrefab);
        bulletObject.transform.position = ShootEmitter.position;
        bulletObject.transform.forward = transform.forward;

    }

    public void BulletDamage(float playerBulletDamage, int shieldDamageBoost)
    {

        if (ShieldPrefab.activeSelf)
        {
            Debug.Log("EnemyShieldDamaged!");
            enemyShield -= (playerBulletDamage * shieldDamageBoost);
            enemyShieldBar.value = enemyShield;
            if (enemyShield <= 0f)
            {
                enemyShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);
            }
        }
        else if (gameObject.activeSelf)
        {
            enemyHealth -= playerBulletDamage;
           enemyHealthBar.value = enemyHealth;

        }

        if (enemyHealth == 0f)
        {
            gameObject.SetActive(false);
        }

    }

    public void MeteorDamage(float meteorDamage, int shieldDamageBoost)
    {
        if (ShieldPrefab.activeSelf)
        {
            enemyShield -= (meteorDamage * shieldDamageBoost);
            enemyShieldBar.value = enemyShield;
            if (enemyShield <= 0f)
            {
                enemyShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);
            }
        }
        else if (gameObject.activeSelf)
        {
            enemyHealth -= meteorDamage;
            enemyHealthBar.value = enemyHealth;

        }

        if (enemyHealth == 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void MissileDamage(float playerMissileDamage, int shieldDamageBoost)
    {

        if (ShieldPrefab.activeSelf)
        {
            Debug.Log("EnemyShieldDamagedRock!");
            enemyShield -= (playerMissileDamage * shieldDamageBoost);
            enemyShieldBar.value = enemyShield;
            if (enemyShield <= 0f)
            {
                enemyShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);
            }
        }
        else if (gameObject.activeSelf)
        {
            enemyHealth -= playerMissileDamage;
            enemyHealthBar.value = enemyHealth;

        }

        if (enemyHealth == 0f)
        {
            gameObject.SetActive(false);
        }

    }

    void Twist()
    {
        newDir = playerparent.position - enemyparent.position;
        float h1 = newDir.normalized.x; // set as your inputs 
        float v1 = newDir.normalized.z;

        float h2 = newDir.normalized.x; // set as your inputs 
        float v2 = -newDir.normalized.z;
        Debug.Log("Angles: "+tr.localEulerAngles.y);

        if (tr.localEulerAngles.y == 76f || tr.localEulerAngles.y == 274f)
        { // this statement allows it to recenter once the inputs are at zero 
            Debug.Log("WTF!!!");
            Vector3 curRot = tr.localEulerAngles; // the object you are rotating
            Vector3 homeRot;
            if (curRot.y > 180f)
            { // this section determines the direction it returns home 

                homeRot = new Vector3(0f, 359.999f, 0f); //it doesnt return to perfect zero 
                
            }
            else
            {                                                                      // otherwise it rotates wrong direction 
                homeRot = Vector3.zero;
            }
            v1 = v1*(-1);
       }
        else
        {

            currentRotateDirection = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f);

            tr.localEulerAngles = currentRotateDirection;


        }
        
        
    }
}
