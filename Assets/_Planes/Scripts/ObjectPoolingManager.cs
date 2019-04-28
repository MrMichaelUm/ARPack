using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planes
{
    public class ObjectPoolingManager : MonoBehaviour
    {
        public static ObjectPoolingManager instance;

        public int healthBonusAmount = 3;
        private Queue<GameObject> healthBonuses;
        private Transform healthBonusesParent;

        void Awake()
        {
            #region Singleton
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion

            healthBonuses = new Queue<GameObject>(healthBonusAmount);
            healthBonusesParent = GameObject.FindWithTag("Bonuses").GetComponent<Transform>();
        }

        public GameObject GetHealthBonus(GameObject HealthBonus)
        {
            foreach (GameObject healthBonus in healthBonuses)
            {
                if (!healthBonus.activeInHierarchy)
                {
                    healthBonus.SetActive(true);
                    //Debug.Log("Found unused object");
                    return healthBonus;
                }
            }
            GameObject prefabInstance = Instantiate(HealthBonus);
            prefabInstance.transform.SetParent(healthBonusesParent);
            healthBonuses.Enqueue(prefabInstance);

            return prefabInstance;
        }
        
    }
}