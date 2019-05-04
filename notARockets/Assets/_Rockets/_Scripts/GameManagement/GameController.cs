using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;

    public Slider PlayerHealth;
    public Slider PlayerShield;
    public Slider EnemyHealth;
    public Slider EnemyShield;

    public int Win;  //Переменная определяет победителя

    private PlayerRotationScript PlayerScript;
    private EnemyRotationScript EnemyScript;

    void Start()
    {
        Win = 0;

        Player.SetActive(true);
        Enemy.SetActive(true);

        if (Player.gameObject.activeSelf)
        {
            PlayerScript = GameObject.FindWithTag("Player").GetComponent<PlayerRotationScript>();
        }
        if (Enemy.gameObject.activeSelf)
        {
            EnemyScript = GameObject.FindWithTag("Enemy").GetComponent<EnemyRotationScript>();
        }

        PlayerHealth.value = 100f;
        PlayerShield.value = 100f;
        EnemyHealth.value = 100f;
        EnemyShield.value = 100f;
    }

    private void Update()
    {

        if (PlayerHealth.value == 0f)
        {
            Player.SetActive(false);
            Win = 2;
        }

        if (EnemyHealth.value == 0f)
        {
            Enemy.SetActive(false);
            if (!Player.activeSelf)
            {
                Win = 3;
            }
            else
            {
                Win = 1;
            }
        }
    }

}