using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planes
{
    public class SandStorm : MonoBehaviour
    {
        bool isStorming = false;
        float timer = 0;
        public float sandStormDurability = 10f;
        GameObject _player;
        Transform _playerTransform;
        PlayerBluePrint _playerScript;
        PlayerMovement _playerMovement;
        FixedJoystick _joystick;

        float standartSpeed;
        float standartRotationSpeed;
        public float sandSpeedIncrease = 2;
        public float sandRotationIncrease = 4;
        double timeForRandomRotate = 1.5;
        Vector3 randomRotation;
        System.Random random;
        public float damage;
        public float deltaTimeDamage;
        float timeSinceLastDamage;

        void Start()
        {
            _joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
            _player = GameObject.FindWithTag("Player");
            _playerTransform = _player.GetComponent<Transform>();
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _playerScript = _player.GetComponent<PlayerBluePrint>();
            standartSpeed = _playerMovement.speed;
            standartRotationSpeed = _playerMovement.rotationSpeed;
            random = new System.Random();
        }
        
        void Update()
        {
            if (!isStorming)
            {
                return;
            }
            if (isStorming && timer < sandStormDurability)
            {
                //Debug.Log("IT'S SANDSTORM!!!");
                timer += Time.deltaTime;
                _joystick.SnapX = true;
                _joystick.SnapY = true;
                _playerMovement.speed = standartSpeed * sandSpeedIncrease;
                _playerMovement.rotationSpeed = standartRotationSpeed * sandRotationIncrease;
                if (_joystick.Horizontal >= -0.2 && _joystick.Horizontal <= 0.2 && _joystick.Vertical >= -0.2 && _joystick.Vertical <= 0.2)
                {
                    if (timeForRandomRotate > 0)
                    {
                        timeForRandomRotate -= Time.deltaTime;
                        //Debug.Log(timeForRandomRotate);
                        _playerTransform.Rotate(randomRotation * standartRotationSpeed * sandRotationIncrease);
                    }
                    else
                    {
                        timeForRandomRotate = random.NextDouble() * (2 - 0.5) + 0.5;
                        randomRotation.x = (float) random.NextDouble();
                        randomRotation.y = (float)random.NextDouble();
                    }
                }
                else
                {
                    timeForRandomRotate = 0;
                }
                
                if (timer - timeSinceLastDamage >= deltaTimeDamage)
                {
                    _playerScript.TakeDamage(damage);
                    timeSinceLastDamage = timer;
                }
            }
            if (isStorming && timer > sandStormDurability)
            {
                _joystick.SnapX = false;
                _joystick.SnapY = false;
                _playerMovement.speed = standartSpeed;
                _playerMovement.rotationSpeed = standartRotationSpeed;
                //медленно заканчиваем шторм
                /*if (шторм полностью закончен)
                {
                    timer = 0;
                    isStorming = false;
                }*/

                timer = 0;
                isStorming = false;
                timeSinceLastDamage = 0;
                Debug.Log("Sandstorm ends");
            }
        }

        public void StartSandStorm()
        {
            isStorming = true;
            timer = 0;
        }
    }
}