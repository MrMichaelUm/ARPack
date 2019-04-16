using UnityEngine;
using UnityEngine.UI;

namespace Planes
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        bool paused = false; //указывает, нажата ли пауза
        Image _pauseScreen; //полупрозрачный экран, который появляется, когда нажата пауза

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
    }
}