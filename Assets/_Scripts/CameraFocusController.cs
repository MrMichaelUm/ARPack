﻿using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraFocusController : MonoBehaviour
{
	public Transform focusCursor;

	void Start()
	{
		var vuforia = VuforiaARController.Instance;
		vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		vuforia.RegisterOnPauseCallback(OnPaused);
	}

	private void OnVuforiaStarted()
	{
		CameraDevice.Instance.SetFocusMode(
			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	private void OnPaused(bool paused)
	{
		if (!paused) // resumed
		{
			// Set again autofocus mode when app is resumed
			CameraDevice.Instance.SetFocusMode(
			   CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

			focusCursor.position = Input.GetTouch(0).position;
		}
	}
}