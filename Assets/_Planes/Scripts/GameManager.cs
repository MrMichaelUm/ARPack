using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        bool paused = false; //указывает, нажата ли пауза
        Image _pauseScreen; //полупрозрачный экран, который появляется, когда нажата пауза

        static int currentDifficlty; 
        //варианты сложности врага
        public static int EASY = 1;
        public static int MEDIUM = 2;
        public static int HARD = 3;
        public static int NEXT_DIFFICULTY = -1; //следующая сложность

        GameObject _enemy;

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
            _enemy = GameObject.FindWithTag("Enemy");
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
    }
}