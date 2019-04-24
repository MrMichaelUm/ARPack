using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Racing
{
	public class CarSystem : MonoBehaviour
	{
		public enum Character { Player, Enemy }
		[Tooltip("Кто наша машина: игрок или враг")]
		public Character character;                         //who is our car: player or enemy

		[Header("CheckPoints")]
		public Transform checkpointsHolder;
		float switchCheckpointDistance;                     //distance when we change enemy waypoint
		Vector3[] checkpointsPosition;
		Vector3 enemyCheckpointTarget;                      //next waypoint of enemy
		Vector3 lastCheckpointPos;                          //last checpoint position
		int enemyCheckpointTargetIndex = 0;                 //index of waypoint
		bool fullLoop = false;								//did the player complete a full loop?

		float normalMovingSpeed;
		float movingSpeed;
		float stopSpeed = 30;                               //Speed with which our car stop in front of the board
		float accelerationSpeed;
		float turnSpeed;

		//agent
		float standardAgentAcceleration;                            //acceleration variable in navmeshagent

		float turnTurtleDistance;                           // Distance to the ground if the car is turned turtle
		float toGroundDistance;                             // Distance to the ground if the car is normal
		float toBorderDistance;                             // Distance to bord if car is leaned on board

		private Rigidbody m_Rigidbody;                      // Reference used to move the tank.
		private float movementInputValue;                   // The current value of the movement input.
		private float turnInputValue;                       // The current value of the turn input.
		private int roadLayerMask;                          // LayerMask for road
		private int borderLayerMask;                        // LayerMask for bord


		//Cash
		Transform _transform;
		CarBlueprint carStats;
		NavMeshAgent agent;


		private void Awake()
		{
			m_Rigidbody = GetComponent<Rigidbody>();
			carStats = GetComponent<CarBlueprint>();

			if (character == Character.Enemy)
			{
				agent = GetComponent<NavMeshAgent>();
				standardAgentAcceleration = agent.acceleration;
			}

			//get data from the CarBlueprint
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
			movementInputValue = 0f;

			AccelerateCar();
		}

		private void OnDisable()
		{
			// When the car is turned off, set it to kinematic so it stops moving.
			m_Rigidbody.isKinematic = true;
		}

		private void Update()
		{
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
			if (character == Character.Player)
			{
				PlayerController();
				return;
			}


			if (!OnTurnTurtle())
				EnemyMovement();
		}

		void PlayerController()
		{
			if (!OnTurnTurtle())
				Move();
			if (OnGround())
				Turn();


			//movementInputValue = Mathf.Clamp(movementInputValue, 0, 1);

		}

		void EnemyMovement()
		{
			//check for the next checkpoint
			if (OnGround())
				NextWaypoint();
			//rotatate our enemy
			EnemyRotation();

			//if we use agent moving system
			if (agent.enabled)
				agent.SetDestination(enemyCheckpointTarget);

			else
			{
				//create a vector, that move us forward
				Vector3 movement = _transform.forward * movementInputValue * movingSpeed * Time.deltaTime * 0.2f;
				//move our player by this vector
				m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
			}

			////create a vector, that move us forward
			//Vector3 movement = _transform.forward * movementInputValue * movingSpeed * Time.deltaTime * 0.2f;
			////move our player by this vector
			//m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
			////check for the next checkpoint
			//if (OnGround())
			//	NextWaypoint();
			////rotatate our enemy
			//EnemyRotation();
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

		//accelerate our car after stop/slowdown
		void AccelerateCar()
		{
			//after start moving increase our speed with a time
			movementInputValue += Time.deltaTime * accelerationSpeed;
		}

		//slowdown car
		void SlowdownCar()
		{
			//until the some speed (0.4) we slowdown the car
			if (movementInputValue > 0.4f)
				movementInputValue -= Time.deltaTime * stopSpeed;
			//after that moment set the speed to 0.1
			else
				movementInputValue = 0.1f;

		}

		bool OnGround()
		{
			return Physics.Raycast(_transform.position, -_transform.up, toGroundDistance, roadLayerMask);
		}

		//if car is turned turtle (lie on the side or roof)
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

		//if car is lean on boarder
		//если машина уперлась в стену
		bool LeanOnBorder()
		{
			RaycastHit hit;

			if (Physics.Raycast(_transform.position, _transform.forward, out hit, toBorderDistance, borderLayerMask))
			{
				SlowdownCar();
				return true;
			}

			AccelerateCar();
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

		//back to track if our car is off track
		//возврат на трек после вылета
		public void BackToTrack()
		{
			_transform.position = lastCheckpointPos;
			_transform.rotation = Quaternion.identity;

			//return speed to startSpeed
			movingSpeed = normalMovingSpeed;
		}

		//increase player speed
		public void ChangeSpeedBonus(float speedUp, float effectTime, float backToNormalTime)
		{
			StartCoroutine(ChangeSpeedBonusCoroutine(speedUp, effectTime, backToNormalTime));
		}

		//increase player speed coroutine
		public IEnumerator ChangeSpeedBonusCoroutine(float speedUp, float effectTime, float backToNormalTime)
		{
			movingSpeed += speedUp;
			yield return new WaitForSeconds(effectTime);

			while (Mathf.Abs(movingSpeed-normalMovingSpeed) >= 0.1f)
			{
				movingSpeed -= Time.deltaTime * speedUp / backToNormalTime;
				yield return null;
			}

			movingSpeed = normalMovingSpeed;
		}

		public void OnTriggerEnter(Collider other)
		{
			//check for enter the checkpoint
			//проверка на вход в чекпоинт
			if (other.CompareTag("Checkpoint"))
			{
				lastCheckpointPos = other.transform.position;
				Debug.Log(other.name);
			}

			//check for enter the finish
			//проверка на вход в финиш
			else if (other.CompareTag("Finish") && fullLoop)
			{
				carStats.UpdateScore();

				//check for winner of the game
				//проверка на то, победил ли игрок
				Racing.GameManager.Instance.GetGameWinner();
				
				fullLoop = false;
			}

			//set point on the middle of the loop to check did the player complete a loop
			else if (other.CompareTag("CheckForFullLoop"))
			{
				fullLoop = true;
			}

			else if (other.CompareTag("GravityOff"))
			{
				m_Rigidbody.useGravity = false;
			}

			else if (other.CompareTag("GravityOn"))
			{
				m_Rigidbody.useGravity = true;
			}

			//switch controller between navmeshagent and linear moving
			else if (other.CompareTag("SwitchAgent"))
			{
				if (character == Character.Enemy)
				{
					//increase car acceleration to any big num to delete a speed gap when controllers are switching
					agent.acceleration = 500_000;
					//switch controllers
					agent.enabled = !agent.enabled;
				}
			}
		}

		public void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Border"))
			{
				BackToTrack();
			}

			//back car acceleration to standard acceleration
			else if (other.CompareTag("SwitchAgent"))
			{
				if (character == Character.Enemy)
					agent.acceleration = standardAgentAcceleration;
			}
		}

	}
}