using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing
{

	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		public CarBlueprint[] cars;

		public static bool gameIsGoing = false;
		public static bool countdownGameStarted = false;                //начался ли обратный отсчет

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

		public void StartCountdown()
		{
			//if (MultiplayerFoundTracker.countOfTrackingFound == 2)
			{
				//if (track)
				//if (PhotonNetwork.countOfPlayers == 2)
				StartCoroutine(StartGame());
			}
		}

		public IEnumerator StartGame()
		{
			countdownGameStarted = true;

			yield return new WaitForSeconds(3);

			gameIsGoing = true;


			//!!!! Сейчас стоит в CarSystem в FixedUpdate проверка на gameStarted из  GameManager
			for (int i = 0; i < cars.Length; i++)
			{
				cars[i].GetComponent<CarSystem>().enabled = true;
			}
		}

		//use if game is paused or stopped
		public void StopGame()
		{
			//turn off the game
			gameIsGoing = false;
			//turn off the countdownTimer
			countdownGameStarted = false;

			for (int i = 0; i < cars.Length; i++)
			{
				cars[i].GetComponent<CarSystem>().enabled = false;
			}
		}

		//check for game winner
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

}