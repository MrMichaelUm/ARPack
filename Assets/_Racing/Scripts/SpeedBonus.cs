using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing {

	public class SpeedBonus : MonoBehaviour
	{
		public float speedUp = 3;               //value of bouns speed
		public float effectTime = 2;            //time of effect
		public float backToNormalTime = 1;      //after how much time player will be back to normal speed

		CarSystem player;

		public void Awake()
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<CarSystem>();
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				StartCoroutine(player.SpeedUp(speedUp, effectTime, backToNormalTime));
				//gameObject.SetActive(false);
			}
		}
	}
}
