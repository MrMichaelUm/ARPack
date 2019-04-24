using UnityEngine;

namespace Planes
{
    public class HealthBonusScript : MonoBehaviour
    {
        public float healthBonus = -15f;
        float timer;
        public float timeBeforeDisappear = 15f;
        Transform _bonus;
        private void Awake()
        {
            _bonus = GetComponent<Transform>();
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                timer += Time.deltaTime;
            }
            else if (!gameObject.activeInHierarchy)
            {
                timer = 0;
            }
            if (timer >= timeBeforeDisappear)
            {
                gameObject.SetActive(false);
                timer = 0;
            }
        }

        private void FixedUpdate()
        {
            _bonus.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 100);
        }
        private void OnTriggerEnter(Collider other)
        {
            CharacterPrint enteredObj = other.GetComponentInParent<CharacterPrint>();
            if (enteredObj != null)
            {
                enteredObj.TakeDamage(healthBonus);
                gameObject.SetActive(false);
            }
        }
    }
}