using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    bool paused = true; //указывает, нажата ли пауза. (В начале игры нажата)
    Image _pauseScreen; //полупрозрачный экран, который появляется, когда нажата пауза
    private void Awake()
    {
        //для Singleton - инициализируем единственный экземпляр GameManager и удаляем любые вторые
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); //не позволяем удалять GameManager при перезагрузке сцені

        _pauseScreen = GameObject.FindWithTag("PauseScreen").GetComponent<Image>();
        Time.timeScale = 0; //игра начинается с паузы
    }

    public void PauseButtonPressed()
    {
        if (paused) //если пауза нажата, а мы нажимаем кнопку, то отжимаем паузу
        {
            Debug.Log("Pause unpressed");
            paused = false; //установка нового состояния паузы
            Time.timeScale = 1; //запускаем ход времени
            _pauseScreen.enabled = false; //убрать экран
            return;
        }
        if (!paused)
        {
            Debug.Log("Pause pressed");
            paused = true; //установка нового состояния паузы
            Time.timeScale = 0; //останавливаем ход времени
            _pauseScreen.enabled = true; //включить экран
            return;
        }
    }
}
