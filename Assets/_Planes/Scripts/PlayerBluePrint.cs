using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class PlayerBluePrint : CharacterPrint
    {
        Slider _gunCharge;

        void Awake()
        {
            isPlayer = true; //объект является игроком
            _gunCharge = GameObject.FindWithTag("PlayerCharge").GetComponent<Slider>();
            shootingPoints = GetComponentsInChildren<SingleShoot>();
            _sliderHealth = GameObject.FindWithTag("PlayerHealth").GetComponent<Slider>();
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
        }

        override public void Shooting()
        {
            if (timer < timeBetweenShoots)
            {
                Debug.Log("Cannot shoot now");
                return;
            }
            if (_gunCharge == null)
            {
                Debug.Log("No slider");
                return;
            }

            if (_gunCharge.value <= 0)
            {
                Debug.Log("No charge!");
                return;
            }

            foreach (SingleShoot point in shootingPoints)
            {
                point.Shoot();
            }
            _gunCharge.value -= 1;
            timer = 0f;
        }
    }
}

/*public class PlayerBluePrint : MonoBehaviour
{
    float timer;
    public float timeBetweenShoots = 0.3f;
    float effectsDisplayTime = 0.2f;
    public float health;
    Slider _gunCharge;
    Slider _sliderHealth;
    SingleShoot shootingR;
    SingleShoot shootingL;

    void Awake()
    {
        _gunCharge = GameObject.FindWithTag("PlayerCharge").GetComponent<Slider>();
        shootingR = GameObject.FindWithTag("ShootPointR").GetComponentInChildren<SingleShoot>();
        shootingL = GameObject.FindWithTag("ShootPointL").GetComponentInChildren<SingleShoot>();
        _sliderHealth = GameObject.FindWithTag("PlayerHealth").GetComponent<Slider>();
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
    }

    public void PlayerShooting()
    {
        if (timer < timeBetweenShoots)
        {
            Debug.Log("Cannot shoot now");
            return;
        }
        if (_gunCharge == null)
        {
            Debug.Log("No slider");
            return;
        }

        if (_gunCharge.value <= 0)
        {
            Debug.Log("No charge!");
            return;
        }

        shootingR.Shoot();
        shootingL.Shoot();
        _gunCharge.value -= 1;
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
