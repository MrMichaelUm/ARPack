using UnityEngine;

//единичный выстрел из точки ShootPoint
//не контролирует частоту выстрелов и заряд!
namespace Planes
{
    public class SingleShoot : MonoBehaviour
    {
        public int damagePerShot = 5; //урон
        public float range = 50f; //радиус

        Ray _shootRay; //луч-направление выстрела
        RaycastHit shootHit; //для сбора информации при попадании
        int shootableMask; //слой, чтобы определить объекты, в которые можно стрелять
        Transform _shootPoint; //точка, из которой стреляем
        ParticleSystem _shootParticles; //анимация выстрела
        LineRenderer _shootLine; //видимый луч выстрела
        Light _shootLight; //свет при выстреле


        void Awake()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            _shootPoint = GetComponent<Transform>();
            _shootRay = new Ray();
            _shootParticles = GetComponent<ParticleSystem>();
            _shootLine = GetComponent<LineRenderer>();
            _shootLight = GetComponent<Light>();
        }

        public void DisableEffects() //отключаем эффекты
        {
            _shootLine.enabled = false;
            _shootLight.enabled = false;
        }


        public void Shoot()
        {
            //включание эффектов
            _shootLight.enabled = true;
            _shootParticles.Stop();
            _shootParticles.Play();
            _shootLine.enabled = true;
            _shootLine.SetPosition(0, _shootPoint.position); //ставим направление выстрела
                                                             //направление луча выстрела
            _shootRay.origin = _shootPoint.position;
            _shootRay.direction = _shootPoint.forward;

            if (Physics.Raycast(_shootRay, out shootHit, range, shootableMask)) //если попали по объекту на слое Shootable
            {
                EnemyRedPrint obj = shootHit.collider.GetComponentInParent<EnemyRedPrint>(); //если попали по врагу, находим его основной скрипт
                if (obj != null)
                {
                    Debug.Log("Ouch");
                    obj.TakeDamage(damagePerShot); //отнимаем здоровье
                }
                else
                {
                    PlayerBluePrint objPlayer = shootHit.collider.GetComponentInParent<PlayerBluePrint>(); //если попали по игроку, находим его основной скрипт
                    if (objPlayer != null)
                    {
                        Debug.Log("Yauzah");
                        objPlayer.TakeDamage(damagePerShot); //отнимаем здоровье
                    }
                }
                _shootLine.SetPosition(1, shootHit.point); //прорисовываем луч выстрела до той точки, куда попали

            }
            else
            {
                _shootLine.SetPosition(1, _shootRay.origin + _shootRay.direction * range); //если ни в кого не попали, прорисовываем выстрел на максимальный радиус
            }
        }
    }
}