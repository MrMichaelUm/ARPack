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
        Transform _enemy; //для отслеживания положения врага
        float aimRange;
        Vector3 neededRotation;

        void Start()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            _shootPoint = GetComponent<Transform>();
            _shootRay = new Ray();
            _shootParticles = GetComponent<ParticleSystem>();
            _shootLine = GetComponent<LineRenderer>();
            _shootLight = GetComponent<Light>();
            CharacterPrint thisCharacterPrint = GetComponentInParent<CharacterPrint>();
            aimRange = thisCharacterPrint.aimRange;
            if (thisCharacterPrint.isPlayer)
            {
                _enemy = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
            }
            else if (!thisCharacterPrint.isPlayer)
            {
                _enemy = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }

        public void DisableEffects() //отключаем эффекты
        {
            _shootLine.enabled = false;
            _shootLight.enabled = false;
        }

        private void Update()
        {

            _shootPoint.LookAt(_enemy.position);
            Vector3 rotation = _shootPoint.localEulerAngles;
            //Debug.Log("Rotation after LookAt: " + rotation.x + ", " + rotation.y + ", " + rotation.z);
            while (!(rotation.x > -180 && rotation.x < 180))
            {
                if (rotation.x > 180)
                {
                    rotation.x -= 360;
                }
                if(rotation.x < -180)
                {
                    rotation.x += 360;
                }
            }
            while (!(rotation.y > -180 && rotation.y < 180))
            {
                if (rotation.y > 180)
                {
                    rotation.y -= 360;
                }
                if (rotation.y < -180)
                {
                    rotation.y += 360;
                }
            }
            while (!(rotation.z > -180 && rotation.z < 180))
            {
                if (rotation.z > 180)
                {
                    rotation.z -= 360;
                }
                if (rotation.z < -180)
                {
                    rotation.z += 360;
                }
            }
            //Debug.Log("Rotation after normalization: " + rotation.x + ", " + rotation.y + ", " + rotation.z);
            if (rotation.x < 0 - aimRange || rotation.x > 0 + aimRange)
            {
                rotation.x = 0;
            }
            if (rotation.y < 0 - aimRange || rotation.y > 0 + aimRange)
            {
                rotation.y = 0;
            }
            if (rotation.z < 0 - aimRange || rotation.z > 0 + aimRange)
            {
                rotation.z = 0;
            }
            //Debug.Log("Rotation after check: " + rotation.x + ", " + rotation.y + ", " + rotation.z);
            _shootPoint.localEulerAngles = rotation;
            /*neededRotation = Quaternion.ToEulerAngles(Quaternion.LookRotation(_enemy.position - _shootPoint.position));


             if ((neededRotation.x >= 0 - aimRange & neededRotation.x <= 0 + aimRange) & (neededRotation.y >= 90 - aimRange & neededRotation.y <= 90 + aimRange))
             {
                 _shootPoint.eulerAngles = new Vector3(
                   Mathf.Clamp(neededRotation.x, 0 - aimRange, 0 + aimRange),
                   Mathf.Clamp(neededRotation.y, 90 - aimRange, 90 + aimRange),
                   neededRotation.z
                   );
                 Debug.Log("See the enemy!");*/
            //_shootPoint.rotation = Quaternion.Slerp(_shootPoint.rotation, neededRotation, Time.deltaTime);
        
        }


        public void Shoot()
        {
            //включение эффектов
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
                CharacterPrint obj = shootHit.collider.GetComponentInParent<CharacterPrint>();
                if (obj != null)
                {
                    Debug.Log("Ouch");
                    obj.TakeDamage(damagePerShot); //отнимаем здоровье
                }
                _shootLine.SetPosition(1, shootHit.point); //прорисовываем луч выстрела до той точки, куда попали

            }
            else
            {
                _shootLine.SetPosition(1, _shootRay.origin + _shootRay.direction * range); //если ни в кого не попали, прорисовываем выстрел на максимальный радиус
            }
        }

        public bool CanDamage()
        {
            _shootRay.origin = _shootPoint.position;
            _shootRay.direction = _shootPoint.forward;
            if(Physics.Raycast(_shootRay, out shootHit, range, shootableMask))
            {
                CharacterPrint obj = shootHit.collider.GetComponentInParent<CharacterPrint>();
                if (obj != null)
                {
                    if (obj.isPlayer)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}