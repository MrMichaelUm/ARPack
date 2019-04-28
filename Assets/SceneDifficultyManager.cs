using UnityEngine;

//добавляет в сцену игрока, врага и остров соответсвующего уровня
namespace Planes
{
    public class SceneDifficultyManager : MonoBehaviour
    {
        private void Start()
        {
            switch (GameManager.instance.currentDifficulty)
            {
                case GameManager.Difficulties.EASY:
                    {
                        Debug.Log("Easy level");
                        Instantiate(GameManager.instance.Island1);
                        Instantiate(GameManager.instance.GreenEnemy);
                        break;
                    }
                case GameManager.Difficulties.MEDIUM:
                    {
                        Debug.Log("Medium level");
                        Instantiate(GameManager.instance.YellowEnemy);
                        break;
                    }
                case GameManager.Difficulties.HARD:
                    {
                        Debug.Log("Hard level");
                        break;
                    }
            }
            Instantiate(GameManager.instance.Player);
        }
    }
}