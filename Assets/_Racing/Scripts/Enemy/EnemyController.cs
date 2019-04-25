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
		public float shockTime = 1;
		float shockTImeCountdown;
		bool shocked = false;

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

			shockTImeCountdown = shockTime;

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
			if (enemySkill == EnemySkill.ShockWave && !shocked)
			{
				if (Vector3.Distance(player.position, _transform.position) < shockWaveRadius)
				{
					Debug.Log("SHOCK");

					StartCoroutine(Shock());

					skillCountdown = skillReloadTime;
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

		IEnumerator Shock()
		{
			shocked = true;

			while (shockTImeCountdown > 0)
			{
				player.GetComponent<Rigidbody>().AddExplosionForce(forceCoef, -player.position + _transform.position, shockWaveRadius, 0, ForceMode.Impulse);

				shockTImeCountdown -= Time.deltaTime;
				yield return null;
			}
			shockTImeCountdown = shockTime;
			shocked = false;
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
