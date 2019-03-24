using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public CarBlueprint[] cars;

	[Tooltip("Необходимое количество кругов для победы")]
	public int winScore = 3;

	#region Singleton
	private void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
	}
	#endregion

	public CarBlueprint GetGameWinner()
	{
		for (int i = 0; i < cars.Length; i++)
		{
			if (cars[i].score == winScore)
			{
				Debug.Log(cars[i].name + " Wins");
				return cars[i];
			}
		}
		return null;
	}
}

