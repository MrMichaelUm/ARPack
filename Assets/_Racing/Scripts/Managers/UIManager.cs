using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Racing
{

	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance;

		public GameObject turnLeftButton;
		public GameObject turnRightButton;

		public TextMeshProUGUI timerText;

		float startGameTime = 3;                //Время перед стартом игры
		float timer;                            //таймер, с которым производятся все операции

		#region Singleton
		private void Awake()
		{

			if (Instance != null)
				return;
			Instance = this;
		}
		#endregion

		private void Start()
		{
			timer = startGameTime;
		}

		public void SetControl(CarSystem car)
		{
			EventTrigger leftButton = turnLeftButton.AddComponent<EventTrigger>();
			EventTrigger rightButton = turnRightButton.AddComponent<EventTrigger>();

			//Устанавливаем управление для правой кнопки (нажатие)

			EventTrigger.Entry entryRightDown = new EventTrigger.Entry();
			entryRightDown.eventID = EventTriggerType.PointerDown;
			entryRightDown.callback.AddListener((data) => { OnPointerDownRightDelegate((PointerEventData)data, car); });
			rightButton.triggers.Add(entryRightDown);

			//Устанавливаем управление для левой кнопки (нажатие)

			EventTrigger.Entry entryLeftDown = new EventTrigger.Entry();
			entryLeftDown.eventID = EventTriggerType.PointerDown;
			entryLeftDown.callback.AddListener((data) => { OnPointerDownLeftDelegate((PointerEventData)data, car); });
			leftButton.triggers.Add(entryLeftDown);

			//Устанавливаем управление для кнопок (отпускание)

			EventTrigger.Entry entryUp = new EventTrigger.Entry();
			entryUp.eventID = EventTriggerType.PointerUp;
			entryUp.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data, car); });
			leftButton.triggers.Add(entryUp);
			rightButton.triggers.Add(entryUp);
		}

		void OnPointerDownRightDelegate(PointerEventData data, CarSystem car)
		{
			car.TurnCar(1);
		}

		void OnPointerDownLeftDelegate(PointerEventData data, CarSystem car)
		{
			car.TurnCar(-1);
		}

		void OnPointerUpDelegate(PointerEventData data, CarSystem car)
		{
			car.TurnCar(0);
		}


		private void Update()
		{
			//if game is not started but countdown stated
			if (!GameManager.gameIsGoing && GameManager.countdownGameStarted)
			{
				//back to standart size of timer
				timerText.GetComponent<Animator>().SetBool("GameStarted", false);
				timer -= Time.deltaTime;
				timerText.text = Mathf.Ceil(timer).ToString();
			}
			//if game is started
			else if (GameManager.gameIsGoing)
			{
				timerText.text = "GO!";
				//start anim
				timerText.GetComponent<Animator>().SetBool("GameStarted", true);
				//set standart value for timer
				timer = startGameTime;
			}
		}
	}
}
