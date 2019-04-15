using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterPrint : MonoBehaviour
{
    protected float timer;
    public float timeBetweenShoots = 0.3f;
    protected float effectsDisplayTime = 0.2f;
    public float health;
    protected Slider _sliderHealth;
    protected SingleShoot shootingR;
    protected SingleShoot shootingL;

    public static float CRUSH = -1;

    public abstract void Shooting();

    virtual public void TakeDamage(float damage)
    {
        if (damage == CRUSH)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        _sliderHealth.value = health;
    }
}
