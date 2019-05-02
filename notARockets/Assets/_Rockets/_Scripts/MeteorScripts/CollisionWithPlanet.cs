using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlanet : MonoBehaviour
{
    public GameObject Impact;
    public GameObject Explosion;
    public GameObject Shadow;
    public PlayerRotationScript player;
    public EnemyRotationScript enemy;
    public float MeteorDamage;
    public int ShieldDamageBoost;
    public List<ParticleSystem> trails;

    public bool MeteorDestroyed;

    void OnEnable()
    {
        MeteorDestroyed = false;
    }
    void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        

        if (other.collider.CompareTag("PlanetSurface")) {

            
            if (Impact != null)
            {
                var ImpactVFX = Instantiate(Impact, pos, rot) as GameObject;
                Destroy(ImpactVFX, 10);
            }

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
            if (other.collider.CompareTag("Player")|| (other.collider.CompareTag("PlayerShield")))
            {
                //Debug.Log("Meteor Hit the Player!");
                player.MeteorDamage(MeteorDamage, ShieldDamageBoost);
            }
            if (other.collider.CompareTag("Enemy")||(other.collider.CompareTag("EnemyShield")))
            {
                Debug.Log("Meteor Hit the Enemy!");
                enemy.MeteorDamage(MeteorDamage, ShieldDamageBoost);
            }
                if (Explosion != null)
                {
                    var ExplosionVFX = Instantiate(Explosion, pos, rot) as GameObject;
                    Destroy(ExplosionVFX, 10);
                }

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
