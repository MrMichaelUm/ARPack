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
	public int score = 0;               //количество пройденных кругов машиной
	public TextMeshProUGUI scoreText;

	private void Start()
	{
		speed = transform.localScale.x * 300;
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
