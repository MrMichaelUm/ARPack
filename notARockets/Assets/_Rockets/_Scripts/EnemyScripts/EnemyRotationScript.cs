using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyRotationScript : MonoBehaviour
{

    Transform tr;

    public Transform player;
    public Transform ShootEmitter;
    public Transform enemyparent;   //Пустой объект который выполняет движение по планете и хранит в себе объект врага

    public int probabilityInPercent;  // Вероятность кастования умения в процентном соотношении
    public float freezeDuration;      //Длительность умения "Заморозка"
    public float freezeDamage;        //Урон заморозки за каждый фрэйм(примерно 2 милисекунды)

    public float enemyHealth;
    public float enemyShield;

    public GameObject BulletPrefab;  
    private Slider enemyHealthBar;   //Полоса здоровья
    private Slider enemyShieldBar;   //Полоса щитов
    public GameObject ShieldPrefab;  //Щит врага

    

    private float timeBetweenShots;    //Вспомогательный таймер для скорости стрельбы
    public float startBetweenShots;    //Скорость(период) стрельбы 

    public bool castFreeze;            //Сигнал кастования умения "Заморозка"

    private PlayerRotationScript playerStats;  //Связь со скриптом игрока

    public int RandomDigit;                    //Случайное число для иллюзии вероятности

    void Start()
    {

        StartCoroutine(RandomPerSec());  //Задаёт сигнал для кастования умения один раз в заданный период времени(5 сек по стандарту)

        tr = GetComponent<Transform>();

        playerStats = player.GetComponent<PlayerRotationScript>();
        
        enemyHealthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<Slider>();
        enemyShieldBar = GameObject.FindGameObjectWithTag("EnemyShieldBar").GetComponent<Slider>();

        ShieldPrefab.SetActive(true); //Активируем щиты в начале

        timeBetweenShots = startBetweenShots;

        tr.rotation = enemyparent.rotation; //На всякий случай переприсваиваем напраление вращения родителя

        castFreeze = false;                 //В начале не кастуем умений

        RandomDigit = 102;                  //В начале задаём число не входящее в наш промежуток
    }

    void Update()
    {
        /* Здесь можно подключить тот или иной общий стиль поведения :*/

        Enemy2Behaviour();

        /* Здесь можно было подключить тот или иной общий стиль поведения.*/

        /* Стрельба раз в определённый период времени*/
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


    public void Enemy1Behaviour()
    {
        if (enemyHealth <= 50f)
        {

            startBetweenShots = 0.5f;
        }
    }

    public void Enemy2Behaviour()
    {
        if (enemyHealth <= 50f)
        {
            startBetweenShots = 0.5f;
        }

        
        if (castFreeze)
        {
            playerStats.FreezeCast(freezeDuration, freezeDamage);  //Кастуем заморозку
            castFreeze = false;
        }

    }
    public void Shoot()
    {
        GameObject bulletObject = ObjectPoolingManager.Instance.GetEnemyBullet(BulletPrefab);  //Вызываем пулю из пула:)
        bulletObject.transform.position = ShootEmitter.position;                               //Ставим её на позицию выстрелов
        bulletObject.transform.forward = transform.forward;                                    //Направляем по направлению движения врага

    }

    /* задаём каст умений с определённой вероятностью в каждый заданный период */
    IEnumerator RandomPerSec()
    {

        yield return new WaitForSeconds(5f);

        RandomDigit = Random.Range(0, 101);
        if (RandomDigit <= probabilityInPercent)
        {
            castFreeze = true;
        }
        else
        {
            castFreeze = false;
        }

        StartCoroutine(RandomPerSec());
    }

    /* Функция получения урона от пули игрока */
    public void BulletDamage(float playerBulletDamage, int shieldDamageBoost)
    {
        
        if (ShieldPrefab.activeSelf)
        {
            //Debug.Log("EnemyShieldDamaged!");
            enemyShield -= (playerBulletDamage * shieldDamageBoost);  //Если щиты активны то перераспределяем урон на них
            enemyShieldBar.value = enemyShield;
            if (enemyShield <= 0f)
            {
                enemyShieldBar.value = 0f;
                ShieldPrefab.SetActive(false);                        //Если щит исчерпался - выключаем его
            }
        }
        else if (gameObject.activeSelf)
        {
            enemyHealth -= playerBulletDamage;                        //Если нет щитов и объект ещё жив - отнимаем здоровье
           enemyHealthBar.value = enemyHealth;

            if (enemyHealth <= 0f)
            {
                enemyHealthBar.value = 0f;
                gameObject.SetActive(false);                          //Уничтожение объекта
            }

        }

        

    }


    /* Функция получения урона от метеорита */
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

            if (enemyHealth <= 0f)
            {
                enemyHealthBar.value = 0f;
                gameObject.SetActive(false);                          //Уничтожение объекта
            }
        }
    }


    /* Функция получения урона от ракеты игрока */
    public void MissileDamage(float playerMissileDamage, int shieldDamageBoost)
    {

        if (ShieldPrefab.activeSelf)
        {
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

            if (enemyHealth <= 0f)
            {
                enemyHealthBar.value = 0f;
                gameObject.SetActive(false);                          //Уничтожение объекта
            }
        }
    }

}
