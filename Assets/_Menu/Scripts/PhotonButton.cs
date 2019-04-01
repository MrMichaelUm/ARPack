using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonButton : MonoBehaviour
{
	public MenuLogic menuLogic;

	public InputField createRoomInput, joinRoomInput;

	public void OnClickCreateRoom()
	{
		menuLogic.CreateNewRoom();
	}

	public void OnClickJoinRoom()
	{
		menuLogic.JoinOrCreateRoom();
	}

}
