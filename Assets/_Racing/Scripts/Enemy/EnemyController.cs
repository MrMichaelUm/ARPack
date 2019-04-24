using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing
{
	public class EnemyController : MonoBehaviour
	{
		public Transform pathHolder;

		public enum EnemySkill { None, ShockWave, SpawnEggs };
		public EnemySkill enemySkill;

		[Tooltip("Time after which enemy uses the skill")]
		public float skillReloadTime;
		float skillCountdown = 0;                                   //timer that we decrease to ckeck a reload time

		[Header("Shock Wave")]
		[Tooltip("Radius of circle of action")]
		public float shockWaveRadius;
		[Tooltip("Coef by which we multiply a shock force")]
		public float forceCoef;
		[Tooltip("Coef by which we multiply a shock torque")]
		public float torqueCoef;

		[Space]
		[Header("Spawns Eggs")]
		[SerializeField]
		GameObject eggPrefab;
		GameObject egg;

		//cash
		Transform player;
		Transform _transform;

		private void Start()
		{
			//cash 
			player = GameObject.FindGameObjectWithTag("Player").transform;
			_transform = transform;

			//set countdowntimer to start value
			skillCountdown = skillReloadTime;

			//spawn the egg at the (0, 0, 0) and disable it
			if (enemySkill == EnemySkill.SpawnEggs)
			{
				egg = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);
				egg.SetActive(false);
			}
		}

		private void Update()
		{
			if (GameManager.gameIsGoing)
			{
				if (skillCountdown <= 0)
					UseSkill();

				else
					skillCountdown -= Time.deltaTime;
			}
		}

		public void UseSkill()
		{
			if (enemySkill == EnemySkill.ShockWave)
			{
				if (Vector3.Distance(player.position, _transform.position) < shockWaveRadius)
				{
					float shockForce = 1 - (Vector3.Distance(player.position, transform.position) / shockWaveRadius);
					//player.GetComponent<Rigidbody>().AddExplosionForce(forceCoef, new Vector3((player.position.x - _transform.position.x), 0, (player.position.z - _transform.position.z)).normalized, shockWaveRadius,0, ForceMode.Impulse);
					//player.GetComponent<Rigidbody>().AddExplosionForce(forceCoef, player.position - _transform.position, shockWaveRadius, 0, ForceMode.Impulse);

					//player.GetComponent<Rigidbody>().AddTorque(new Vector3(0, (player.position.y - _transform.position.y), 0).normalized * shockForce * torqueCoef, ForceMode.Impulse);


					float y = Mathf.Lerp(player.rotation.y, 360 * shockForce, Time.deltaTime);
					player.rotation = Quaternion.Euler(player.rotation.x, y, player.rotation.z);
					//player.Rotate(0, 360 * shockForce * Time.deltaTime * 2, 0);

					skillCountdown = skillReloadTime;

					Debug.Log("SHOCK");
				}
			}

			//move the egg to the new position, enable it and reset countdown timer
			else if (enemySkill == EnemySkill.SpawnEggs)
			{
				egg.SetActive(true);

				egg.transform.position = transform.position;

				skillCountdown = skillReloadTime;
			}
		}

		//Draw waypoints positions
		private void OnDrawGizmosSelected()
		{
			Vector3 startPos = pathHolder.GetChild(0).position;
			Vector3 previousPos = startPos;

			foreach (Transform waypoint in pathHolder)
			{
				Gizmos.DrawSphere(waypoint.position, 0.1f);
				Gizmos.DrawLine(previousPos, waypoint.position);

				previousPos = waypoint.position;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, shockWaveRadius);
		}
	}
}
