using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Planes
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        bool paused = false; //указывает, нажата ли пауза
        Image _pauseScreen; //полупрозрачный экран, который появляется, когда нажата пауза
        //GameObject _chooseLevelScreen;
        public bool buttonOrientation = false;
        public enum Difficulties { EASY, MEDIUM, HARD };
        public Difficulties currentDifficulty;

        Transform _joystick;
        Transform _fireSystem;

        //префабы врагов и островов
        public GameObject Island1;//, Island2, Island3;
        public GameObject GreenEnemy, YellowEnemy;//, RedEnemy;
        public GameObject Player;

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
            //_chooseLevelScreen = GameObject.FindWithTag("ChooseLevelScreen");
            _joystick = GameObject.FindWithTag("Joystick").GetComponent<RectTransform>();
            _fireSystem = GameObject.FindWithTag("FireSystem").GetComponent<RectTransform>();
            Time.timeScale = 0;
            paused = true;
        }

        //ставит/убирает паузу при нажатии на кнопку
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

        public void ChangeDifficulty (Difficulties newDifficulty) //регулировка сложности уровня
        {
            currentDifficulty = newDifficulty;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneDifficultyManager.instance.
            /*switch (newDifficulty)
            {
                case Difficulties.EASY:
                {
                    Debug.Log("Easy level");
                    //_chooseLevelScreen.SetActive(false);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    //Instantiate(Island1);
                    //Instantiate(GreenEnemy);
                    break;
                }
                case Difficulties.MEDIUM:
                {
                    Debug.Log("Medium level");
                    //_chooseLevelScreen.SetActive(false);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    Instantiate(YellowEnemy);
                    break;
                }
                case Difficulties.HARD:
                {
                    Debug.Log("Hard level");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                }
            }
            //Instantiate(Player);
            inst();*/
        }

        void inst ()
        {
            Instantiate(Island1);
            Instantiate(GreenEnemy);
            Instantiate(Player);
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