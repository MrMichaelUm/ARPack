using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Racing
{
	public class CarSystem : MonoBehaviour
	{
		public enum Character { Player, Enemy }
		[Tooltip("Кто наша машина: игрок или враг")]
		public Character character;                            //who is our car: player or enemy

		[Header("CheckPoints")]
		public Transform checkpointsHolder;
		float switchCheckpointDistance;              //distance when we change enemy waypoint
		Vector3[] checkpointsPosition;
		Vector3 enemyCheckpointTarget;                  //next waypoint of enemy
		Vector3 lastCheckpointPos;                  //точка последнего чекпоинта машины
		int enemyCheckpointTargetIndex = 0;                 //index of waypoint

		float normalMovingSpeed;
		float movingSpeed;
		float stopSpeed = 30;                       //Скорость, с которой машина останавливается перед бортом
		float accelerationSpeed;
		float turnSpeed;

		float turnTurtleDistance;                   // Расстояние до земли если машина перевернулась
		float toGroundDistance;                     // Расстояние до земли если машина стоит на колесах
		float toBorderDistance;                     // Расстояния до ограничивающего бортика, если машина уперлась

		private Rigidbody m_Rigidbody;              // Reference used to move the tank.
		private float movementInputValue;         // The current value of the movement input.
		private float turnInputValue;             // The current value of the turn input.
		private int roadLayerMask;                  // LayerMask для дороги
		private int borderLayerMask;                // LayerMask для ограничивающего бортика


		CarBlueprint carStats;

		//Кеш
		Transform _transform;


		private void Awake()
		{
			m_Rigidbody = GetComponent<Rigidbody>();
			carStats = GetComponent<CarBlueprint>();


			//Получение данных из CarBlueprint
			normalMovingSpeed = carStats.movingSpeed;
			stopSpeed = carStats.stopSpeed;
			accelerationSpeed = carStats.accelerationSpeed;
			turnSpeed = carStats.turnSpeed;
			turnTurtleDistance = carStats.turnTurtleDistance;
			toGroundDistance = carStats.toGroundDistance;
			toBorderDistance = carStats.toBorderDistance;
			switchCheckpointDistance = carStats.switchCheckpointDistance;


			movingSpeed = normalMovingSpeed;


			roadLayerMask = LayerMask.GetMask("Road");
			borderLayerMask = LayerMask.GetMask("Border");


			checkpointsPosition = new Vector3[checkpointsHolder.childCount];

			for (int i = 0; i < checkpointsHolder.childCount; i++)
			{
				checkpointsPosition[i] = checkpointsHolder.GetChild(i).position;
			}
			enemyCheckpointTarget = checkpointsPosition[0];
			lastCheckpointPos = checkpointsPosition[0];

			//cash
			_transform = GetComponent<Transform>();
		}

		private void OnEnable()
		{
			// When the car is turned on, make sure it's not kinematic.
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
			Debug.Log(enemyCheckpointTargetIndex);
			movementInputValue = Mathf.Clamp(movementInputValue, 0, 1);
			LeanOnBorder();

			if (character == Character.Enemy)
				return;

			if (Input.GetKey(KeyCode.LeftArrow))
				TurnCar(-1);
			if (Input.GetKey(KeyCode.RightArrow))
				TurnCar(1);


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
			if (character == Character.Enemy)
			{
				if (!OnTurnTurtle())
					EnemyMovement();
				return;
			}

			PlayerController();
		}

		void PlayerController()
		{
			if (!OnTurnTurtle())
				Move();
			if (OnGround())
				Turn();


			movementInputValue = Mathf.Clamp(movementInputValue, 0, 1);

		}

		void EnemyMovement()
		{
			//create a vector, that move us forward
			Vector3 movement = _transform.forward * movementInputValue * movingSpeed * Time.deltaTime * 0.2f;
			//move our player by this vector
			m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
			//check for the next checkpoint
			if (OnGround())
				NextWaypoint();
			//rotatate our enemy
			EnemyRotation();
		}

		void EnemyRotation()
		{
			//find our rotation vector
			Vector3 direction = enemyCheckpointTarget - _transform.position;
			//look for this vector
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			//rotate our player smoothly
			Vector3 rotation = Quaternion.Lerp(_transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
			//apply our rotations by y coordinate
			_transform.rotation = Quaternion.Euler(0, rotation.y, 0);
		}

		void NextWaypoint()
		{
			//if our distance is less than switchCheckpointDistance
			if ((_transform.position - enemyCheckpointTarget).sqrMagnitude <= switchCheckpointDistance * switchCheckpointDistance)
			{
				Debug.Log(enemyCheckpointTargetIndex);
				//infinity cicle, in the end we will back to first element of array
				enemyCheckpointTargetIndex = (enemyCheckpointTargetIndex + 1) % checkpointsPosition.Length;
				enemyCheckpointTarget = checkpointsPosition[enemyCheckpointTargetIndex];
			}


		}

		bool OnGround()
		{
			return Physics.Raycast(_transform.position, -_transform.up, toGroundDistance, roadLayerMask);
		}

		//если машина перевернулась (лежит на боку либо на крыше)
		bool OnTurnTurtle()
		{
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

				//до определенного момента медленно умеьшаем скорость
				if (movementInputValue > 0.4f)
					movementInputValue -= Time.deltaTime * stopSpeed;
				//после этой грани ставим скорость почти в 0
				else
					movementInputValue = 0.1f;

				return true;

			}
			//после начала движения постепенно увеличиваем скорость
			movementInputValue += Time.deltaTime * accelerationSpeed;
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
			Vector3 movement = _transform.forward * movementInputValue * movingSpeed * Time.deltaTime * 0.2f;

			// Apply this movement to the rigidbody's position.
			m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
		}

		private void Turn()
		{
			// Determine the number of degrees to be turned based on the input, speed and time between frames.
			float turn = turnInputValue * turnSpeed * Time.deltaTime;

			// Make this into a rotation in the y axis.
			Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

			// Apply this rotation to the rigidbody's rotation.
			m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
		}

		//возврат на трек после вылета
		public void BackToTrack()
		{
			_transform.position = lastCheckpointPos;
			_transform.rotation = Quaternion.identity;
		}

		//increase player speed
		public void SpeedUp(float speedUp, float effectTime, float backToNormalTime)
		{
			StartCoroutine(SpeedUpCoroutine(speedUp, effectTime, backToNormalTime));
		}

		//increase player speed coroutine
		public IEnumerator SpeedUpCoroutine(float speedUp, float effectTime, float backToNormalTime)
		{
			movingSpeed += speedUp;
			yield return new WaitForSeconds(effectTime);

			while (movingSpeed >= normalMovingSpeed)
			{
				movingSpeed -= Time.deltaTime * speedUp / backToNormalTime;
				yield return null;
			}

			movingSpeed = normalMovingSpeed;
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
				//carStats.scoreText.text = carStats.score.ToString();

				//проверка на то, победил ли игрок
				Racing.GameManager.Instance.GetGameWinner();
			}

			else if (other.CompareTag("GravityOff"))
			{
				m_Rigidbody.useGravity = false;
			}

			else if (other.CompareTag("GravityOn"))
			{
				m_Rigidbody.useGravity = true;
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
}