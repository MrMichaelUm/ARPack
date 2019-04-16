using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class EnemyRedPrint : CharacterPrint
    {
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
                Shooting();
            }
        }

        override public void Shooting()
        {
            //Debug.Log("RRRRShoot!");
            shootingL.Shoot();
            shootingR.Shoot();
            timer = 0f;
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
