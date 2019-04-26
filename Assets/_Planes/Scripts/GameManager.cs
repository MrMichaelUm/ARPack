using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        bool paused = false; //указывает, нажата ли пауза
        Image _pauseScreen; //полупрозрачный экран, который появляется, когда нажата пауза
        public bool buttonOrientation = false;

        static int currentDifficlty; 
        //варианты сложности врага
        public static int EASY = 1;
        public static int MEDIUM = 2;
        public static int HARD = 3;
        public static int NEXT_DIFFICULTY = -1; //следующая сложность

        //GameObject _enemy;
        Transform _joystick;
        Transform _fireSystem;

        private void Awake()
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
            DontDestroyOnLoad(gameObject); //не позволяем удалять GameManager при перезагрузке сцены

            _pauseScreen = GameObject.FindWithTag("PauseScreen").GetComponent<Image>();
            _pauseScreen.enabled = false;
            //_enemy = GameObject.FindWithTag("Enemy");
            _joystick = GameObject.FindWithTag("Joystick").GetComponent<RectTransform>();
            _fireSystem = GameObject.FindWithTag("FireSystem").GetComponent<RectTransform>();
            //ChangeDifficulty(EASY); //начинаем с первой сложности
        }

        public void PauseButtonPressed()
        {
            if (paused) //если пауза нажата, а мы нажимаем кнопку, то отжимаем паузу
            {
                Debug.Log("Pause unpressed");
                paused = false; //установка нового состояния паузы
                PauseMenu.Instance.PauseOff(); //запускаем ход времени
                _pauseScreen.enabled = false; //убрать экран
                return;
            }
            if (!paused)
            {
                Debug.Log("Pause pressed");
                paused = true; //установка нового состояния паузы
                PauseMenu.Instance.PauseOn(); //останавливаем ход времени
                _pauseScreen.enabled = true; //включить экран
                return;
            }
        }

        public void ChangeDifficulty (int newDifficulty) //регулировка сложности уровня
        {
            /*if (newDifficulty == NEXT_DIFFICULTY)
            {
                newDifficulty = currentDifficlty + 1;
                if (newDifficulty > 3)
                {
                    newDifficulty = 1;
                }
            }
            switch (newDifficulty)
            {
                case 1:
                    {
                        _enemy.AddComponent<EnemyGreenPrint>();
                        break;
                    }
                case 2:
                    {
                        _enemy.AddComponent<EnemyYellowPrint>();
                        break;
                    }
                case 3:
                    {
                        _enemy.AddComponent<EnemyRedPrint>();
                        break;
                    }
            }
            currentDifficlty = newDifficulty;*/
        }

        //нужно вызвать при переключении положения кнопок
        public void SwapButtons()
        {
            float xTemp = _joystick.position.x;
            _joystick.position = new Vector3(_fireSystem.position.x, _joystick.position.y, 0);
            _fireSystem.position = new Vector3(xTemp, _fireSystem.position.y, 0);
        }
    }
}