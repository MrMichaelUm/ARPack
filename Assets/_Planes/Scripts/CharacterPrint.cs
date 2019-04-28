using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public abstract class CharacterPrint : MonoBehaviour
    {
        public bool isPlayer; //является ли наследник класса игроком (true) или врагом (false)
        protected float timer; //таймер для подсчета времени между выстрелами/отображением эффектов
        public float timeBetweenShoots = 0.3f; //время между выстрелами
        protected float effectsDisplayTime = 0.2f; //время отображения эффектов
        public float health; //здоровье
        public float aimRange = 10;
        protected Slider _sliderHealth; //слайдер, показывающий здоровье
        protected SingleShoot[] shootingPoints; //точки, откуда делаются выстрелы

        public static float CRUSH = -1; //крушение: мгновенно убивает персонажа
        
        public abstract void Shooting(); //стрельба персонажа

        virtual public void TakeDamage(float damage) //отнять у персонажа здоровье
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
                //if (!isPlayer)
                    //GameManager.instance.ChangeDifficulty(GameManager.NEXT_DIFFICULTY); //если умирает враг, то игра переходит на следующую сложность
                Destroy(gameObject);
            }
            _sliderHealth.value = health;
        }
    }
}