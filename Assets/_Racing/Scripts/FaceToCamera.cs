using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing
{
	public class FaceToCamera : MonoBehaviour
	{
		public Transform cam;

		Transform _transform;

		private void Start()
		{
			_transform = transform;
		}

		void Update()
		{
			Vector3 dir = _transform.position - cam.position;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = lookRotation.eulerAngles;
			_transform.rotation = Quaternion.Euler(0, rotation.y, 0);
		}
	}
}
