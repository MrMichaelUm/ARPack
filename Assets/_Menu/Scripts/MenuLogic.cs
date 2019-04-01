using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuLogic : MonoBehaviour
{
	public PhotonButton photonButton;

	public GameObject[] environment;						//массив targetImages, который будет заменяться на массив окружения выбранной игры 
	public EnvironmentStats selectedEnvironment;            //выбранное окружение, которое будет загружаться

	//!!!Создать в магазине массив объектов с PlayerPrefs, который будет запоминать выбранные в магазине машины/самолеты/ракеты и в зависимости от выбранной игры 
	//присваивать в playerObject машину/самолет/ракеты
	public GameObject playerObject;                 //сюда присваиваем префаб машины/самолета/ракеты из магазина, в префабе настраиваем все CarStats

	private void Awake()
	{
		DontDestroyOnLoad(this.transform);

		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 20;

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	public void CreateNewRoom()
	{
		PhotonNetwork.CreateRoom(photonButton.createRoomInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
	}

	public void JoinOrCreateRoom()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		//PhotonNetwork.JoinRoom(photonButton.joinRoomInput.text);
		PhotonNetwork.JoinOrCreateRoom(photonButton.joinRoomInput.text, roomOptions, TypedLobby.Default);
	}

	public void LoadScene()
	{
		photonButton = null;

		PhotonNetwork.LoadLevel("Racing");
	}

	void OnJoinedRoom()
	{
		LoadScene();
		Debug.Log("We are connected to the room");
	}

	void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Racing")
			Spawn();
	}

	void Spawn()
	{
		//создаем только 1 экземпляр окружения
		//if (PhotonNetwork.countOfPlayers < 2)
		//{
			selectedEnvironment = Instantiate(environment[ChooseGameController.environmentNum], Vector3.zero, Quaternion.identity).GetComponent<EnvironmentStats>();
		//}

		if (PhotonNetwork.countOfPlayers == 1)
		{
			GameObject car = PhotonNetwork.Instantiate(playerObject.name, selectedEnvironment.spawnPosition1.position, playerObject.transform.rotation, 0) as GameObject;
			car.transform.SetParent(selectedEnvironment.transform.Find("Add Content As Children Here"));
		}

		else if (PhotonNetwork.countOfPlayers == 2)
		{
			Debug.Log("2 PLAYERS!!!");
			GameObject car = PhotonNetwork.Instantiate(playerObject.name, selectedEnvironment.spawnPosition2.position, playerObject.transform.rotation, 0) as GameObject;
			car.transform.SetParent(selectedEnvironment.transform.Find("Add Content As Children Here"));
		}

		//EventTrigger.Entry entry = new EventTrigger.Entry();
		//entry.eventID = EventTriggerType.PointerDown;
	}
}
