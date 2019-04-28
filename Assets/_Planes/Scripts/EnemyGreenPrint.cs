﻿using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class EnemyGreenPrint : CharacterPrint
    {
        public float standartSpeed = 10;
        public float lowSpeed = 5;
        public  float speed;
        public float lookDelay = 0.3f;
        protected Transform _enemy;
        protected Rigidbody _enemyRigidbody;
        protected Transform _player;
        protected Transform _nose;


        protected float shootDelayTimer;
        public float shootDelay = 1f;
        protected bool playerInRange = false;
        public float prevShoot = 0;
        public float offset = 5;

        protected float y = 0f;

        void Awake()
        {
            isPlayer = false; //объект не является игроком
            timeBetweenShoots = 0.6f;
            shootingPoints = GetComponentsInChildren<SingleShoot>();
            _enemy = GetComponent<Transform>();
            _enemyRigidbody = GetComponent<Rigidbody>();
            _nose = GameObject.FindWithTag("EnemyNose").GetComponent<Transform>();
            _sliderHealth = GameObject.FindWithTag("EnemyHealth").GetComponent<Slider>();
            health = _sliderHealth.value;
            speed = standartSpeed;
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            _enemyRigidbody.angularVelocity = new Vector3(0, 0, 0);
            //постоянное движение вперед
            _enemy.position = Vector3.MoveTowards(_enemy.position, _nose.position, speed * Time.deltaTime);
            //с небольшой задержкой поворачиваемся в направлении игрока
            if (Vector3.Distance(_player.position, _enemy.position) < offset)
            {
                speed = lowSpeed;
                System.Random random = new System.Random();
                if (y == 0)
                {
                    int dir = random.Next(1, 5);
                    switch (dir)
                    {
                        case 1: y = 60f; break;
                        case 2: y = -60f; break;
                        case 3: y = 120f; break;
                        case 4: y = -120f; break;
                    }
                }
                
                _enemy.Rotate(new Vector3(), Space.Self);
                Quaternion neededRotation = Quaternion.Euler(0, y, 0);
                _enemy.rotation = Quaternion.Slerp(_enemy.rotation, neededRotation, Time.deltaTime * lookDelay);
            }
            else
            {
                y = 0f;
                speed = standartSpeed;
                Debug.Log("vsio ok");
                Quaternion neededRotation = Quaternion.LookRotation(_player.position - _enemy.position);
                _enemy.rotation = Quaternion.Slerp(_enemy.rotation, neededRotation, Time.deltaTime * lookDelay);
            }

            foreach (SingleShoot point in shootingPoints)
            {
                if (point.CanDamage() && !playerInRange)
                {
                    playerInRange = true;
                    shootDelayTimer = shootDelay;
                }
            }
            if (shootDelayTimer > 0)
            {
                shootDelayTimer -= Time.deltaTime;
            }
            else if(shootDelayTimer <= 0 && playerInRange)
            {
                if (timer - prevShoot >= timeBetweenShoots)
                {
                    Shooting();
                    prevShoot = timer;
                }
                playerInRange = false;
            }

            

            if (timer - prevShoot >= timeBetweenShoots * effectsDisplayTime)
            {
                foreach (SingleShoot point in shootingPoints)
                {
                    point.DisableEffects();
                }
            }
        }

        override public void Shooting()
        {
            foreach (SingleShoot point in shootingPoints)
            {
                point.Shoot();
            }
        }
    }
}
/*ВАРИАНТ ДО ИНТЕРФЕЙСА
 * 
 * 
 * public class EnemyRedPrint : MonoBehaviour
{
    float timer;
    public float timeBetweenShoots = 0.3f;
    float effectsDisplayTime = 0.2f;
    public float health;
    Slider _sliderHealth;
    SingleShoot shootingR;
    SingleShoot shootingL;

    void Awake()
    {
        shootingR = GameObject.FindWithTag("EnemyShootPointR").GetComponentInChildren<SingleShoot>();
        shootingL = GameObject.FindWithTag("EnemyShootPointL").GetComponentInChildren<SingleShoot>();
        _sliderHealth = GameObject.FindWithTag("EnemyHealth").GetComponent<Slider>();
        health = _sliderHealth.value;
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenShoots * effectsDisplayTime)
        {
            shootingR.DisableEffects();
            shootingL.DisableEffects();
        }
        if (timer >= timeBetweenShoots)
        {
            EnemyShooting();
        }
    }

    void EnemyShooting()
    {
        
        //Debug.Log("RRRRShoot!");
        shootingL.Shoot();
        shootingR.Shoot();
        timer = 0f;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        _sliderHealth.value = health;
    }
 }
 */