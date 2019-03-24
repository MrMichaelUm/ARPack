using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarBlueprint : MonoBehaviour
{
	public float speed;
	public float turnSpeed = 180f;

	[Tooltip("Расстояние до земли если машина перевернулась")]
	public float turnTurtleDistance;
	[Tooltip("Расстояние до земли если машина стоит на колесах")]
	public float toGroundDistance;
	[Tooltip("Расстояния до ограничивающего бортика, если машина уперлась")]
	public float toBorderDistance;

	[HideInInspector]
	public Vector3 lastCheckpointPos;     //точка последнего чекпоинта машины


	[HideInInspector]
	public int score = 0;               //количество пройденных кругов машиной
	public TextMeshProUGUI scoreText;

	private void Start()
	{
		speed = transform.localScale.x * 300;
	}

	//возврат на трек после вылета
	public void BackToTrack()
	{
		transform.position = lastCheckpointPos;
		transform.rotation = Quaternion.identity;
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
			Debug.Log(score);

			score++;
			scoreText.text = score.ToString();

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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -transform.up * toGroundDistance);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, transform.right * turnTurtleDistance);
		Gizmos.DrawRay(transform.position, -transform.right * turnTurtleDistance);
		Gizmos.DrawRay(transform.position, transform.up * turnTurtleDistance);

		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, transform.forward * toBorderDistance);
	}
}
