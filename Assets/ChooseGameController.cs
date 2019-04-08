using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseGameController : MonoBehaviour
{
	public GameObject[] environment;                     //Массив всех окружений (треков/островов/планет)

	public MenuLogic menuLogic;

	[HideInInspector]
	public static int environmentNum = 0;

	//Добавить этот скрипт на OnClick кнопки выбора игры
	public void SetGame()
	{
		for (int i = 0; i < environment.Length; i++)
		{
			menuLogic.environment[i] = environment[i];
		}
	}

	//Добавить этот скрипт на OnClick кнопки выбора номинала купюры (num для первого номинала 0, для кождого последующего +1)
	public void SetEnvironment(int num)
	{
		environmentNum = num;
	}
}
