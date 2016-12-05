using UnityEngine;
using System.Collections;
using SocketIO;

public class newtwork : MonoBehaviour {

    static SocketIOComponent socket;

    public GameObject playerPrefab;


	// Use this for initialization
	void Start ()
    {
	    socket = GetComponent<SocketIOComponent>();
        socket.On("open", onConnected);
        socket.On("spawn", OnSpawned);
	}


    void onConnected(SocketIOEvent e)
    {
        Debug.Log("connected from unity side");
        //socket.Emit("move");
    }

    void OnSpawned (SocketIOEvent e)
    {
        Debug.Log("spawned");
        Instantiate(playerPrefab);
    }

}
