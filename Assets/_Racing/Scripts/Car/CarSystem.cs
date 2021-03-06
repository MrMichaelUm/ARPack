﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CarSystem : MonoBehaviour
{
	[Header("Photon")]
	public PhotonView photonView;
	Vector3 selfPos;                            //Вектор смягчения движения в сети
	public bool devTesting = false;             //Переменная тестирования (если true - отключаем мульиплеер)

	[Header("CheckPoints")]
	Vector3 lastCheckpointPos;                  //точка последнего чекпоинта машины

	float turnTurtleDistance;                   // Расстояние до земли если машина перевернулась
	float toGroundDistance;                     // Расстояние до земли если машина стоит на колесах
	float toBorderDistance;                     // Расстояния до ограничивающего бортика, если машина уперлась
	float normalDistanceTurnTheBorder = 0.3f;   // Нормальное расстояние, чтобы машина могла развернуться, если она уперлась в бортик
	Transform frontOfTransport;


	private Rigidbody m_Rigidbody;              // Reference used to move the tank.
	private float movementInputValue;         // The current value of the movement input.
	private float turnInputValue;             // The current value of the turn input.
	private int roadLayerMask;                  // LayerMask для дороги
	private int borderLayerMask;                // LayerMask для ограничивающего бортика


	CarBlueprint carStats;

	//Кеш
	Transform _transform;
	Vector3 _globalPosition;


	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		carStats = GetComponent<CarBlueprint>();
		frontOfTransport = transform.Find("_FRONT");

		roadLayerMask = LayerMask.GetMask("Road");
		borderLayerMask = LayerMask.GetMask("Border");

		turnTurtleDistance = carStats.turnTurtleDistance;
		toGroundDistance = carStats.toGroundDistance;
		toBorderDistance = carStats.toBorderDistance;

		lastCheckpointPos = GameObject.Find("Checkpoint 1").transform.position;

		photonView = GetComponent<PhotonView>();

		//кеширование
		_transform = GetComponent<Transform>();
		_globalPosition = _transform.TransformPoint(frontOfTransport.position);
		normalDistanceTurnTheBorder *= 2;
	}

	private void OnEnable()
	{
		// When the tank is turned on, make sure it's not kinematic.
		m_Rigidbody.isKinematic = false;

		// Also reset the input values.
		movementInputValue = 1f;
	}

	private void OnDisable()
	{
		// When the car is turned off, set it to kinematic so it stops moving.
		m_Rigidbody.isKinematic = true;
	}

	private void Update()
	{

		if (Input.GetKey(KeyCode.LeftArrow))
			TurnCar(-1);
		if (Input.GetKey(KeyCode.RightArrow))
			TurnCar(1);

		movementInputValue = 1;


		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			turnInputValue = 0;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			turnInputValue = 0;
		}
	}

	private void FixedUpdate()
	{
		//если игра не началась - выходим !!!!! проверить быстродействие, возможно, стоит при старте заезда включать скрипт CarSystem из скрипта GameManager 
		//if (!GameManager.gameStarted)
		//	return;

		if (!devTesting)
		{
			//проверка на то, управляем ли мы нашим игроком Photon
			if (photonView.isMine)
				ReadyController();
			else
				SmoothNetMovement();

			//else
			//	SmoothNetMovement();
		}

		else
			ReadyController();
	}

	void ReadyController()
	{
		if (LeanOnBorder())
		{
			Turn();
			return;
		}
		else
		{
			if (!OnTurnTurtle())
				Move();
			if (OnGround())
				Turn();
		}
	}

	bool OnGround()
	{
		return Physics.Raycast(_transform.position, -_transform.up, toGroundDistance, roadLayerMask);
	}

	//если машина перевернулась (лежит на боку либо на крыше)
	bool OnTurnTurtle()
	{
		//RaycastHit hit;

		if (Physics.Raycast(_transform.position, _transform.up, turnTurtleDistance, roadLayerMask) ||
			Physics.Raycast(_transform.position, _transform.right, turnTurtleDistance, roadLayerMask) ||
			Physics.Raycast(_transform.position, -_transform.right, turnTurtleDistance, roadLayerMask))
		{
			BackToTrack();
			return true;
		}
		return false;
	}

	//если машина уперлась в стену
	bool LeanOnBorder()
	{
		RaycastHit hit;

		if (Physics.Raycast(_transform.position, _transform.forward, out hit, toBorderDistance, borderLayerMask))
		{
			// если расстояние от бортика до капота машины < normalDistanceTurnTheBorder, то откидываем машину назад
			// как следствие - машина не проходит сквозь текстуры

			//аналог методу Distance - работает быстрее
			if ((transform.TransformPoint(hit.transform.position) - _globalPosition).sqrMagnitude < normalDistanceTurnTheBorder)
				m_Rigidbody.AddForce((_transform.position - hit.transform.position) * 2);

			//if (Vector3.Distance(transform.TransformPoint(hit.transform.position), transform.TransformPoint(frontOfTransport.position)) < normalDistanceTurnTheBorder)
			//	m_Rigidbody.AddForce((_transform.position - hit.transform.position) * 2);
			return true;
		}

		return false;
	}

	public void TurnCar(int side)
	{
		if (movementInputValue != 0)
		{
			turnInputValue = side;
		}
	}

	private void Move()
	{
		// Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
		Vector3 movement = _transform.forward * movementInputValue * carStats.speed * Time.deltaTime * 0.2f;

		// Apply this movement to the rigidbody's position.
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}

	private void Turn()
	{
		// Determine the number of degrees to be turned based on the input, speed and time between frames.
		float turn = turnInputValue * carStats.turnSpeed * Time.deltaTime;

		// Make this into a rotation in the y axis.
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

		// Apply this rotation to the rigidbody's rotation.
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
	}

	//возврат на трек после вылета
	public void BackToTrack()
	{
		transform.position = lastCheckpointPos;
		transform.rotation = Quaternion.identity;
	}

	void SmoothNetMovement()
	{
		transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
			stream.SendNext(transform.position);
		else
			selfPos = (Vector3)stream.ReceiveNext();
	}

	public void OnTriggerEnter(Collider other)
	{
		//проверка на вход в чекпоинт
		if (other.CompareTag("Checkpoint"))
		{
			lastCheckpointPos = other.transform.position;
			Debug.Log(other.name);
		}

		//проверка на вход в финиш
		else if (other.CompareTag("Finish"))
		{
			carStats.score++;
			carStats.scoreText.text = carStats.score.ToString();

			//проверка на то, победил ли игрок
			GameManager.Instance.GetGameWinner();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Border"))
		{
			BackToTrack();
		}
	}

}
