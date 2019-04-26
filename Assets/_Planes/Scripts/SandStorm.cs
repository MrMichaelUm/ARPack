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
        Transform _player;
        PlayerMovement _playerMovement;
        FixedJoystick _joystick;

        float standartSpeed;
        float standartRotationSpeed;
        public float sandSpeedIncrease = 2;
        public float sandRotationIncrease = 4;
        double timeForRandomRotate = 1.5;
        Vector3 randomRotation;

        System.Random random;

        void Awake()
        {
            _joystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
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
                        _player.Rotate(randomRotation * standartRotationSpeed * sandRotationIncrease);
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
                Debug.Log("Sandstorm ends");
            }
        }

        public void StartSandStorm()
        {
            isStorming = true;
        }
    }
}