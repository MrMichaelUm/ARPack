using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class EnemyYellowPrint : CharacterPrint
    {
        public float standartSpeed = 10;
        public float maxSpeed = 60;
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
        public float offsetClose = 5;
        public float offsetFarBegin = 150f;
        public float offsetFarEnd = 70f;
        bool turboMode = false;
        SandStorm sandStorm;
        float timeBeforeNextSandStorm;
        float timeSinceLastSandStorm = 0;
        public int minTimeBeforeNextSandStorm = 30;
        public int maxTimeBeforeNextSandStorm = 160;
        System.Random random;

        float y = 0f;

        void Awake()
        {
            isPlayer = false; //объект не является игроком
            timeBetweenShoots = 0.6f;
            shootingPoints = GetComponentsInChildren<SingleShoot>();
            _enemy = GetComponent<Transform>();
            _enemyRigidbody = GetComponent<Rigidbody>();
            _nose = GameObject.FindWithTag("EnemyNose").GetComponent<Transform>();
            _sliderHealth = GameObject.FindWithTag("EnemyHealth").GetComponent<Slider>();
            sandStorm = GetComponent<SandStorm>();
            random = new System.Random();
            timeBeforeNextSandStorm = random.Next(minTimeBeforeNextSandStorm, maxTimeBeforeNextSandStorm);
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
            if (Vector3.Distance(_player.position, _enemy.position) < offsetClose) //если подлетел слишком быстро: сбавляет скорость и рандомно меняет направление
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
                if (Vector3.Distance(_player.position, _enemy.position) > offsetFarBegin || turboMode) //если мы улетели слишком далеко от врага - начинаем ускорение
                {
                    turboMode = true;
                    speed = maxSpeed;
                    if (Vector3.Distance(_player.position, _enemy.position) < offsetFarEnd)
                    {
                        turboMode = false;
                    }
                }
                else
                {
                    speed = standartSpeed;
                }
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

            if (timer - timeSinceLastSandStorm >= timeBeforeNextSandStorm)
            {
                Debug.Log("Starting SandStorm");
                timeSinceLastSandStorm = timer;
                sandStorm.StartSandStorm();
                timeBeforeNextSandStorm = random.Next(minTimeBeforeNextSandStorm, maxTimeBeforeNextSandStorm);
            }
        }

        override public void Shooting() //стрельба
        {
            foreach (SingleShoot point in shootingPoints)
            {
                point.Shoot();
            }
        }
    }
}