using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	public Transform pathHolder;

	//Draw waypoints positions
	private void OnDrawGizmos()
	{
		Vector3 startPos = pathHolder.GetChild(0).position;
		Vector3 previousPos = startPos;

		foreach(Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, 0.1f);
			Gizmos.DrawLine(previousPos, waypoint.position);

			previousPos = waypoint.position;
		}
	}
}
