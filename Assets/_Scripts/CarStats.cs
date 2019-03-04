using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
	public float m_Speed;
	public float m_TurnSpeed = 180f;

	[Tooltip("Расстояние до земли если машина перевернулась")]
	public float turnTurtleDistance;
	[Tooltip("Расстояние до земли если машина стоит на колесах")]
	public float toGroundDistance;
	[Tooltip("Расстояния до ограничивающего бортика, если машина уперлась")]
	public float toBorderDistance;

	private void Start()
	{
		m_Speed = transform.localScale.x * 300;
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
