using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlanet : MonoBehaviour
{
    public GameObject Impact;        //Еффект при столконовении с планетой
    public GameObject Explosion;     //Еффект при столкновении с игроком
    public GameObject Shadow;        //Тень

    public PlayerRotationScript player;
    public EnemyRotationScript enemy;

    public float MeteorDamage;
    public int ShieldDamageBoost;

    public List<ParticleSystem> trails;  //Дым, огонь и другие "цепные" эффекты

    public bool MeteorDestroyed;

    void OnEnable()
    {
        MeteorDestroyed = false;

        if (player.gameObject.activeSelf)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerRotationScript>();
        }
        if (enemy.gameObject.activeSelf)
        {
            enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyRotationScript>();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        /* Запоминаем точку столкноения и угол(вращение относительно точки) при столкновении */
        ContactPoint contact = other.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        

        if (other.collider.CompareTag("PlanetSurface")) {

            //Создаём кратер
            if (Impact != null)
            {
                var ImpactVFX = Instantiate(Impact, pos, rot) as GameObject;
                Destroy(ImpactVFX, 10);
            }

            //Плавно убираем дым с огнём
            if (trails.Count > 0 )
            {
                for (int i = 0; i<trails.Count; i++)
                {
                    if (trails[i] != null)
                    {
                        trails[i].transform.parent = null;
                        var ps = trails[i];
                        if (ps != null)
                        {
                            ps.Stop();
                            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                        }
                    }
                }
            }

            MeteorDestroyed = true;
            gameObject.SetActive(false);
            
        }
        else
        {
            if (other.collider.CompareTag("Player"))
            {
                //Debug.Log("Meteor Hit the Player!");
                player.MeteorDamage(MeteorDamage, ShieldDamageBoost);  //Вызываем функцию урона игроку
            }
            if (other.collider.CompareTag("Enemy"))
            {
                //Debug.Log("Meteor Hit the Enemy!");
                enemy.MeteorDamage(MeteorDamage, ShieldDamageBoost);  //Вызываем функцию урона противнику
            }

            //Создаём взрыв
            if (Explosion != null)
            {
                    var ExplosionVFX = Instantiate(Explosion, pos, rot) as GameObject;
                    Destroy(ExplosionVFX, 10);
            }

            //Плавно убираем дым с огнём
            if (trails.Count > 0)
             {
                    for (int i = 0; i < trails.Count; i++)
                    {
                        if (trails[i] != null)
                        {
                            trails[i].transform.parent = null;
                            var ps = trails[i];
                            if (ps != null)
                            {
                                ps.Stop();
                                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                            }
                        }
                    }
            }

            MeteorDestroyed = true;
            gameObject.SetActive(false);

        }
    }
}
