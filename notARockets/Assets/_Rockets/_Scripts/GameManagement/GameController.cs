using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public int NumberOfLevel;
    public Slider PlayerHealth;
    public Slider PlayerShield;
    public Slider EnemyHealth;
    public Slider EnemyShield;

    public GameObject PlayerBullet;
    public GameObject PlayerMissile;

    public GameObject FirstBossBullet;
    public GameObject SecondBossBullet;
    public GameObject ThirdBossBullet;

    public GameObject SecondBossMissile;
    public GameObject ThirdBossMissile;


    public int Win;  //Переменная определяет победителя

    private PlayerRotationScript PlayerScript;
    private EnemyRotationScript EnemyScript;

    void Awake()
    {

        Player = GameObject.FindGameObjectWithTag("Player");
        Player.SetActive(true);
        PlayerScript = Player.GetComponent<PlayerRotationScript>();

    }
    void Start()
    {

        if (NumberOfLevel == 0)
        {
            Enemy = GameObject.FindWithTag("FirstBoss");
            Enemy.SetActive(true);
            EnemyScript = Enemy.GetComponent<EnemyRotationScript>();
        }
        else if (NumberOfLevel == 1)
        {
            Enemy = GameObject.FindWithTag("SecondBoss");
            Enemy.SetActive(true);
            EnemyScript = Enemy.GetComponent<EnemyRotationScript>();
        }
        else if (NumberOfLevel == 2)
        {
            Enemy = GameObject.FindWithTag("ThirdBoss");
            Enemy.SetActive(true);
            EnemyScript = Enemy.GetComponent<EnemyRotationScript>();
        }

        Win = 0;




        if (Player.gameObject.activeSelf)
        {
        }
        if (Enemy.gameObject.activeSelf)
        {
        }

        PlayerHealth.value = 100f;
        PlayerShield.value = 100f;
        EnemyHealth.value = 100f;
        EnemyShield.value = 100f;
    }

    private void Update()
    {

        if (PlayerHealth.value == 0f)
        {
            Player.SetActive(false);
            Win = 2;
        }
        else if (EnemyHealth.value == 0f)
        {
            Enemy.SetActive(false);

            if (!Player.activeSelf)
            {
                Win = 3;
            }
            else
            {
                Win = 1;
            }
        }
    }

}