using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "MaskGame";
	private string serverName = "Server Name";

	private HostData[] hostList;

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			serverName = GUI.TextField(new Rect(100, 50, 250, 50), serverName, 25);

			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer(serverName);
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnServerInitialized()
	{
		Debug.Log ("Server initialized.");
		Application.LoadLevel (1);
	}

	void OnConnectedToServer()
	{
		Debug.Log ("Connected to server.");
		Application.LoadLevel (1);
	}

	void StartServer(string gameName)
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
}
