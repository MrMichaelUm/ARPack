using UnityEngine;

//Показывает вокруг игрока красный шар, предупреждающий, что он улетел слишком далеко
namespace Planes
{
    public class RedSphere : MonoBehaviour
    {
        Transform _player;
        Material _sphere;
        public float maxAlphaOfSphere = 80f; //максимальная непрозрачность шара
        Color currentSphere; //цвет шара в данный момент
        public float enablingTime = 0.3f; //скорость создания шара
        public bool disabling = false; //показывает, убирается ли шар в данный момент
        public bool enabling = false; //показывает, создается ли шар в данный момент
        public bool inSphere; //показывает, находится игрок внутри допустимой зоны-сферы

        void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            _sphere = GetComponent<MeshRenderer>().material;
            currentSphere = _sphere.color;
        }

        private void FixedUpdate()
        {
            if (inSphere && _player.position.y <= 0) //если игрок в допустимой зоне-сфере, но ниже купюры
            {
                disabling = false; //прекратить убирать красный шар
                EnableSphere(); //создание красного шара
                return;
            }
            if (inSphere && _player.position.y > 0) //если игрок в зоне-сфере и выше купюры
            {
                DisableSphere(); //убирать красный шар
                return;
            }

            if (enabling) //обеспечивает прорисовку красного шара, когда игрок не в сфере-зоне
            {
                EnableSphere();
            }
        }

        public void DisableSphere() //убирает красный шар
        {
            if (_player.position.y < 0) //если игрок находится ниже купюры, то убирать шар никак нельзя
            {
                return;
            }
            else //если шар убирать можно, то нужно "выключить" его создание
            {
                enabling = false;
            }
            if (currentSphere.a == 0) //если прозрачность шара достигла нуля, то есть он не виден, прекратить выполнять функцию
            {
                disabling = false;
                return;
            }
            disabling = true; //показать, что в данный момент шар убирается

            currentSphere.a = Mathf.Lerp(currentSphere.a, 0f, enablingTime * Time.deltaTime); //постепенно убираем непрозрачность - от текущей до нуля
            _sphere.color = currentSphere;
        }

        public void EnableSphere()
        {
            if (currentSphere.a == maxAlphaOfSphere) //если красный шар достиг своей максимальной непрозрачности, прекращаем выполнение функции
            {
                enabling = false;
                return;
            }
            enabling = true; //показать, что в данный момент шар прорисовывается

            currentSphere.a = Mathf.Lerp(currentSphere.a, maxAlphaOfSphere, enablingTime * Time.deltaTime); //постепенно добавляем непрозраность - от текущей до максимальной
            _sphere.color = currentSphere;
        }
    }
}