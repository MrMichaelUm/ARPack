using UnityEngine;
using UnityEngine.UI;

//скрипт сохраняет действие всех кнопок при перезагрузке сцены
namespace Planes {
    public class Buttons : MonoBehaviour
    {
        public enum ButtonType
        {
            FIRE,
            PAUSE,
            SWAP,
            EASY_DIFFICULTY,
            MEDIUM_DIFFICULTY,
            HARD_DIFFICULTY
        }
        public ButtonType thisButton;
        //public enum Difficulties { EASY, MEDIUM, HARD };
        //public Difficulties difficulty;

        PlayerBluePrint _playerShooting;

        void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(TaskOnClick);
        }
        
        void TaskOnClick()
        {
            switch(thisButton)
            {
                case ButtonType.FIRE:
                {
                    if (_playerShooting == null)
                    {
                        _playerShooting = GameObject.FindWithTag("Player").GetComponent<PlayerBluePrint>();
                    }
                    _playerShooting.Shooting();
                    break;
                }
                case ButtonType.PAUSE: GameManager.instance.PauseButtonPressed(); break;
                case ButtonType.SWAP: GameManager.instance.SwapButtons(); break;
                case ButtonType.EASY_DIFFICULTY: GameManager.instance.ChangeDifficulty(GameManager.Difficulties.EASY); break;
                case ButtonType.MEDIUM_DIFFICULTY: GameManager.instance.ChangeDifficulty(GameManager.Difficulties.MEDIUM); break;
                case ButtonType.HARD_DIFFICULTY: GameManager.instance.ChangeDifficulty(GameManager.Difficulties.HARD); break;
            }
        }
    }
}