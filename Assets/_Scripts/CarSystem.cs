using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarSystem : EventTrigger
{
	public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
												//public float m_Speed = 12f;                 // How fast the tank moves forward and back.
												//public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
	public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
	public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
	public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
	public float m_PitchRange = 0.3f;           // The amount by which the pitch of the engine noises can vary.

	float turnTurtleDistance;                   // Расстояние до земли если машина перевернулась
	float toGroundDistance;                     // Расстояние до земли если машина стоит на колесах
	float toBorderDistance;                     // Расстояния до ограничивающего бортика, если машина уперлась
	float normalDistanceTurnTheBorder = 0.3f;   // Нормальное расстояние, чтобы машина могла развернуться, если она уперлась в бортик
	Transform frontOfTransport;

	private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
	private string m_TurnAxisName;              // The name of the input axis for turning.
	private Rigidbody m_Rigidbody;              // Reference used to move the tank.
	private float m_MovementInputValue;         // The current value of the movement input.
	private float m_TurnInputValue;             // The current value of the turn input.
	private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
	int roadLayerMask;                          // LayerMask для дороги
	int borderLayerMask;                        // LayerMask для ограничивающего бортика

	CarStats carStats;

	private int buttonsSideClickedCount = 0;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		carStats = GetComponent<CarStats>();
		frontOfTransport = transform.Find("_FRONT");

		roadLayerMask = LayerMask.GetMask("Road");
		borderLayerMask = LayerMask.GetMask("Border");

		turnTurtleDistance = carStats.turnTurtleDistance;
		toGroundDistance = carStats.toGroundDistance;
		toBorderDistance = carStats.toBorderDistance;
		print(normalDistanceTurnTheBorder);
	}

	private void OnEnable()
	{
		// When the tank is turned on, make sure it's not kinematic.
		m_Rigidbody.isKinematic = false;

		// Also reset the input values.
		m_MovementInputValue = 1f;
	}

	private void OnDisable()
	{
		// When the tank is turned off, set it to kinematic so it stops moving.
		m_Rigidbody.isKinematic = true;
	}


	private void Start()
	{
		// The axes names are based on player number.
		m_MovementAxisName = "Vertical";
		m_TurnAxisName = "Horizontal";

		// Store the original pitch of the audio source.
		//m_OriginalPitch = m_MovementAudio.pitch;
	}


	private void Update()
	{
		// Store the value of both input axes.
		//m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		//m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
		//m_TurnInputValue = 0;

		///тест заднего хода для компа
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			buttonsSideClickedCount++;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			buttonsSideClickedCount++;
		}



		if (buttonsSideClickedCount != 2)
		{
			if (Input.GetKey(KeyCode.LeftArrow))
				TurnCar(-1);
			if (Input.GetKey(KeyCode.RightArrow))
				TurnCar(1);

			m_MovementInputValue = 1;
		}

		else
		{
			m_MovementInputValue = -1;
			TurnCar(0);
		}


		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			buttonsSideClickedCount--;
			m_TurnInputValue = 0;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			buttonsSideClickedCount--;
			m_TurnInputValue = 0;

		}

		//if (buttonsSideClickedCount == 2)
		//	m_MovementInputValue = -1;
		//else
		//	m_MovementInputValue = 1;

	}


	private void FixedUpdate()
	{
		//	Adjust the rigidbodies position and orientation in FixedUpdate.

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
		return Physics.Raycast(transform.position, -transform.up, toGroundDistance, roadLayerMask);
	}

	//если машина перевернулась (лежит на боку либо на крыше)
	bool OnTurnTurtle()
	{
		//RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.up, turnTurtleDistance, roadLayerMask) ||
			Physics.Raycast(transform.position, transform.right, turnTurtleDistance, roadLayerMask) ||
			Physics.Raycast(transform.position, -transform.right, turnTurtleDistance, roadLayerMask))

			return true;

		return false;
	}

	//если машина уперлась в стену
	bool LeanOnBorder()
	{
		RaycastHit hit;

		if (buttonsSideClickedCount == 0)
		{
			if (Physics.Raycast(transform.position, transform.forward, out hit, toBorderDistance, borderLayerMask))
			{
				// если расстояние от бортика до капота машины < normalDistanceTurnTheBorder, то откидываем машину назад
				// как следствие - машина не проходит сквозь текстуры

				if (Vector3.Distance(transform.TransformPoint(hit.transform.position), transform.TransformPoint(frontOfTransport.position)) < normalDistanceTurnTheBorder)
					m_Rigidbody.AddForce((transform.position - hit.transform.position) * 2);
				return true;
			}
		}
		return false;
	}

	public void TurnCar(int side)
	{
		if (m_MovementInputValue != 0)
		{
			m_TurnInputValue = side;
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		buttonsSideClickedCount++;
		print(buttonsSideClickedCount);

		if (buttonsSideClickedCount == 2)
		{
			m_MovementInputValue = -1;
			return;
		}
		else
			m_MovementInputValue = 1;
		base.OnPointerDown(eventData);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		buttonsSideClickedCount--;
		base.OnPointerUp(eventData);
	}

	private void Move()
	{
		// Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
		Vector3 movement = transform.forward * m_MovementInputValue * carStats.m_Speed * Time.deltaTime * 0.2f;

		// Apply this movement to the rigidbody's position.
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}


	private void Turn()
	{
		// Determine the number of degrees to be turned based on the input, speed and time between frames.
		float turn = m_TurnInputValue * carStats.m_TurnSpeed * Time.deltaTime;

		// Make this into a rotation in the y axis.
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

		// Apply this rotation to the rigidbody's rotation.
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
	}


}
