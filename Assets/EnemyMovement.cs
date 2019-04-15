using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	public Transform pathHolder;


	//Draw waypoints positions
	private void OnDrawGizmos()
	{
		Vector3 statPos = pathHolder.GetChild(0).position;
		Vector3 previousPos = statPos;

		foreach(Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .035f);
			Gizmos.DrawLine(previousPos, waypoint.position);

			previousPos = waypoint.position;
		}
	}
}
