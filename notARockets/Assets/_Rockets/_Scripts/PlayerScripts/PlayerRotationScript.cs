using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class PlayerRotationScript : MonoBehaviour {

    //public float moveSpeed;
    Rigidbody rb;
    Transform tr;
    GameObject gm;

    public Joystick joystick;
    public Button shoot;
    public Transform ShootEmitter;
    public Transform RightMissileEmitter;
    public Transform LeftMissileEmitter;
    public GameObject BulletPrefab;
    public GameObject ShieldPrefab;
    public GameObject MissilePrefab;
    public GameObject Enemy;
    public Slider playerHealthBar;
    public Slider playerShieldBar;

    private Vector3 currentRotateDirection;
    private Vector3 previousRotateDirection;
    private Quaternion slerpRotation;
    private bool DamageInputFlag;
    private bool MissileEmitterChange;
    private float ShieldRecoveryTime;
    public int Win;

    private float playerHealth;
    private float playerShield;
    public float playerStartHealth;
    public float playerStartShield;
    public float ShieldRecoveryDelay;
    public float ShieldRecoveryValue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        gm = GetComponent<GameObject>();
        playerHealthBar.value = playerStartHealth;
        playerShieldBar.value = playerStartShield;
        playerHealth = playerStartHealth;
        playerShield = playerStartShield;


        ShieldPrefab.SetActive(true);             //Включаем щиты

        DamageInputFlag = false;                  //Индикатор входящего урона
        MissileEmitterChange = false;

        ShieldRecoveryTime = ShieldRecoveryDelay; //Устанавливаем задержку перед началом восстановления щитов

        previousRotateDirection = tr.localEulerAngles; //Вектор для слерпа

        //Устанавливаем параметры для передачи вне сцены
        Win = 0;
        PlayerPrefs.SetInt("Win", Win);
    }
    void Update()
    {
        ShieldRecovery();   

        if ((gameObject.activeSelf)&&(!Enemy.activeSelf)) {
            Win = 1;
            PlayerPrefs.SetInt("Win", Win);
            PlayerPrefs.Save();
        }
    }

    void FixedUpdate()
    {
        Twist();
        
        //Quaternion rot = Quaternion.LookRotation(new Vector3(joystick.Direction.x, 0, joystick.Direction.y), transform.up);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);

    }

    void ShieldRecovery()
    {
        if ((DamageInputFlag) && (playerShieldBar.value < 100))
        {
            
            if (ShieldRecoveryTime <= 0)
            {
               
                if (ShieldPrefab.activeSelf)
                {
                    playerShield += ShieldRecoveryValue;
                    playerShieldBar.value = playerShield;
                }
                else
                {
                    
                    ShieldPrefab.SetActive(true);
                    playerShield += ShieldRecoveryValue;
                    playerShieldBar.value = playerShield;
                }
            }
            else
            {
                ShieldRecoveryTime -= Time.deltaTime;
            }
        }
    }
    public void BulletDamage(float enemyBulletDamage, int shieldDamageBoost)
    {
        
        if (ShieldPrefab.activeSelf)
        {
            Debug.Log("PlayerShieldDamaged!");
            playerShield -= (enemyBulletDamage*shieldDamageBoost);
            playerShieldBar.value = playerShield;
            if (playerShield <= 0f)
            {
                playerShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);
            }
        }
        else if (gameObject.activeSelf)
        {
            playerHealth -= enemyBulletDamage;
            playerHealthBar.value = playerHealth;
            
        }

        if (playerHealth == 0f)
        {
            gameObject.SetActive(false);
            if (!Enemy.activeSelf)
            {
                Win = 3;
                PlayerPrefs.SetInt("Win", Win);
                PlayerPrefs.Save();
            }
            else
            {
                Win = 2;
                PlayerPrefs.SetInt("Win", Win);
                PlayerPrefs.Save();
            }
            
        }
        else
        {
            DamageInputFlag = true;
            ShieldRecoveryTime = ShieldRecoveryDelay;
        }

    }

    public void MeteorDamage(float meteorDamage, int shieldDamageBoost)
    {
        if (ShieldPrefab.activeSelf)
        {
            
            playerShield -= (meteorDamage*shieldDamageBoost);
            
            playerShieldBar.value = playerShield;
            
            if (playerShield <= 0f)
            {
                playerShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);
            }
        }
        else if (gameObject.activeSelf)
        {
            
            playerHealth -= meteorDamage;
            playerHealthBar.value = playerHealth;

        }

        if (playerHealth == 0f)
        {
            
            if (!Enemy.activeSelf)
            {
                Win = 3;
                PlayerPrefs.SetInt("Win", Win);
            }
            else
            {
                Win = 2;
                PlayerPrefs.SetInt("Win", Win);
            }
            gameObject.SetActive(false);
        }
        else
        {
            DamageInputFlag = true;
        }
    }
    void Twist()
    {
        float h1 = joystick.Horizontal; // set as your inputs 
        float v1 = joystick.Vertical;
        
        if (h1 == 0f && v1 == 0f)
        { // this statement allows it to recenter once the inputs are at zero 
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
            //tr.localEulerAngles = Vector3.Slerp(curRot, curRot, Time.deltaTime * 2);

        }
        else
        {

            currentRotateDirection = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f);

            slerpRotation = Quaternion.LookRotation(currentRotateDirection, tr.up);
            //Vector3 lerpQuaternion = Quaternion.Lerp(Quaternion.LookRotation(previousRotateDirection), slerpRotation, Time.deltaTime * 2).eulerAngles;

            tr.localEulerAngles = currentRotateDirection;
            //tr.rotation = Quaternion.Euler(0, lerpQuaternion.y, 0);


        }
    }

    public void Shoot()
    {
        if (gameObject.activeSelf) { 
            GameObject bulletObject = ObjectPoolingManager.Instance.GetPlayerBullet(BulletPrefab);
            bulletObject.transform.position = ShootEmitter.position;
            bulletObject.transform.forward = transform.forward;
        }
    }

    public void LaunchMissile()
    {
        if (gameObject.activeSelf)
        {
            GameObject missileObject = ObjectPoolingManager.Instance.GetMissile(MissilePrefab);
            if (MissileEmitterChange)
            {
                missileObject.transform.position = RightMissileEmitter.position;
                MissileEmitterChange = false;
            }
            else
            {
                missileObject.transform.position = LeftMissileEmitter.position;
                MissileEmitterChange = true;
            }
            missileObject.transform.forward = transform.forward;
        }
    }
}
