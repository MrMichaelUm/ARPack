using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuLogic : MonoBehaviour
{
	public GameObject[] environment;                        //массив targetImages, который будет заменяться на массив окружения выбранной игры 
	public EnvironmentStats selectedEnvironment;            //выбранное окружение, которое будет загружаться

	//!!!Создать в магазине массив объектов с PlayerPrefs, который будет запоминать выбранные в магазине машины/самолеты/ракеты и в зависимости от выбранной игры 
	//присваивать в playerObject машину/самолет/ракеты
	public GameObject playerObject;                 //сюда присваиваем префаб машины/самолета/ракеты из магазина, в префабе настраиваем все CarStats

	private void Awake()
	{
		DontDestroyOnLoad(this.transform);

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Racing")
			Spawn();
	}

	void Spawn()
	{
		selectedEnvironment = Instantiate(environment[ChooseGameController.environmentNum], Vector3.zero, Quaternion.identity).GetComponent<EnvironmentStats>();

			//создаем машину в выбранном окружении
			GameObject car = Instantiate(playerObject, selectedEnvironment.spawnPosition1.position, playerObject.transform.rotation) as GameObject;
			//удочеряем ее TargetImage
			car.transform.SetParent(selectedEnvironment.transform.Find("Add Content As Children Here"));
			//добавляем машину в массив всех машин GameManager
			GameManager.Instance.cars[0] = car.GetComponent<CarBlueprint>();
			//Устанавливаем для машины управление
			UIManager.Instance.SetControl(car.GetComponent<CarSystem>());

	}
}
