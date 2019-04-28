using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planes {
    public class BonusesSpawnManager : MonoBehaviour
    {
        BonusesSpawnManager instance;
        public GameObject healthBonusPrefab;
        public int minTime = 20;
        public int maxTime = 60;
        float timeToNextBonus = 10;
        float timer;
        System.Random random;
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

            random = new System.Random();
            timeToNextBonus = random.Next(minTime, maxTime);
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= timeToNextBonus)
            {
                SpawnHealthBonus();
                timer = 0;
            }
        }

        void SpawnHealthBonus()
        {
            //Debug.Log("Time to spawn a bonus");
            GameObject bonus = ObjectPoolingManager.instance.GetHealthBonus(healthBonusPrefab);
            Vector3 pos = new Vector3();
            do
            {
                pos.x = random.Next(0, 300);
                pos.y = random.Next(-300, 300);
                pos.z = random.Next(-300, 300);
            } while (Vector3.Distance(new Vector3(0, 0, 0), pos) < 300);
            bonus.GetComponent<Transform>().position = pos;
            timeToNextBonus = random.Next(minTime, maxTime);
        }
    }
}