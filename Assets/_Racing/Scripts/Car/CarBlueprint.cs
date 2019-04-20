﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Racing
{
	public class CarBlueprint : MonoBehaviour
	{
		[Header("Distances")]
		[Tooltip("Расстояние до земли если машина перевернулась")]
		public float turnTurtleDistance;
		[Tooltip("Расстояние до земли если машина стоит на колесах")]
		public float toGroundDistance;
		[Tooltip("Расстояния до ограничивающего бортика, если машина уперлась")]
		public float toBorderDistance;
		[Tooltip("Расстояние, при котором мы будем менять следующий чекпоинт для врага")]
		public float switchCheckpointDistance;

		[Space]

		[Header("Speed")]
		public float movingSpeed;
		public float turnSpeed = 180f;
		[Tooltip("Скорость, с которой машина останавливается перед бортом")]
		public float stopSpeed;
		[Tooltip("Скорость, с которой машина набирает разгон")]
		public float accelerationSpeed;


		[HideInInspector]
		public int score = 0;						//count of laps completed
		public TextMeshProUGUI scoreText;

		private void Start()
		{
			//speed = transform.localScale.x * 300;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, -transform.up * toGroundDistance);

			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(transform.position, transform.right * turnTurtleDistance);
			Gizmos.DrawRay(transform.position, -transform.right * turnTurtleDistance);
			Gizmos.DrawRay(transform.position, transform.up * turnTurtleDistance);
			Gizmos.DrawWireSphere(transform.position, switchCheckpointDistance);

			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position, transform.forward * toBorderDistance);
		}
	}
}