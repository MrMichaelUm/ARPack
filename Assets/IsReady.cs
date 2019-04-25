using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsReady : MonoBehaviour
{
	public bool isSecondPlayerReady = false;
	public bool isMasterClientReady = false;

	public static IsReady Instance;

	#region Singleton
	private void Awake()
	{
		if (Instance != null)
			return;
		Instance = this;
	}
	#endregion

	public void CheckPlayers()
	{
		if (PhotonNetwork.isMasterClient)
		{
			isMasterClientReady = true;
			ReadyCheck();
		}
		else
		{
			GetComponent<PhotonView>().RPC("MeReady", PhotonTargets.AllBuffered, null);
		}
	}

	public void ReadyCheck()
	{
		if (isSecondPlayerReady && isMasterClientReady)
		{
			GetComponent<PhotonView>().RPC("GameStarter", PhotonTargets.All, null);
		}
	}



	[PunRPC]
	public void MeReady()
	{
		if (!PhotonNetwork.isMasterClient)
			return;
		isSecondPlayerReady = true;
		ReadyCheck();
	}


	[PunRPC]
	public void GameStarter()
	{
		GameManager.Instance.StartCountdown();
		Debug.Log("All players ready");
	}

}