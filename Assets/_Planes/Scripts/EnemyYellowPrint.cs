using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class EnemyYellowPrint : CharacterPrint
    {
        public float standartSpeed = 10;
        public float lowSpeed = 5;
        public float speed;
        public float lookDelay = 0.3f;
        Transform _enemy;
        Rigidbody _enemyRigidbody;
        Transform _player;
        Transform _nose;

        float shootDelayTimer;
        public float shootDelay = 1f;
        bool playerInRange = false;
        public float prevShoot = 0;
        public float offset = 5;

        float y = 0f;

        void Awake()
        {
            isPlayer = false; //объект не является игроком
            timeBetweenShoots = 0.6f;
            shootingPoints = GetComponentsInChildren<SingleShoot>();
            _enemy = GetComponent<Transform>();
            _enemyRigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            _nose = GameObject.FindWithTag("EnemyNose").GetComponent<Transform>();
            _sliderHealth = GameObject.FindWithTag("EnemyHealth").GetComponent<Slider>();
            health = _sliderHealth.value;
            speed = standartSpeed;
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
            else if (shootDelayTimer <= 0 && playerInRange)
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