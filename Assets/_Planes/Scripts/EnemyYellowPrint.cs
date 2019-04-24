using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class EnemyYellowPrint : CharacterPrint
    {
        void Awake()
        {
            isPlayer = false; //объект не является игроком

            timeBetweenShoots = 0.3f;
            shootingPoints = GetComponentsInChildren<SingleShoot>();

            _sliderHealth = GameObject.FindWithTag("EnemyHealth").GetComponent<Slider>();
            health = _sliderHealth.value;
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= timeBetweenShoots * effectsDisplayTime)
            {
                foreach (SingleShoot point in shootingPoints)
                {
                    point.DisableEffects();
                }
            }
            if (timer >= timeBetweenShoots)
            {
                Shooting();
            }
        }

        override public void Shooting()
        {
            foreach (SingleShoot point in shootingPoints)
            {
                point.Shoot();
            }
            timer = 0f;
        }
    }
}