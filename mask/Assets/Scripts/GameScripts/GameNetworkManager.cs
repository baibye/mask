using UnityEngine;
using System.Collections;

public class GameNetworkManager : MonoBehaviour {

	public Object playerPrefab;

	void Start()
	{
		SpawnPlayer ();
	}

	void SpawnPlayer()
	{
		Network.Instantiate (playerPrefab, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
	}
}
