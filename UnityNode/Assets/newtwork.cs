using UnityEngine;
using System.Collections.Generic;
using SocketIO;
using System;

public class newtwork : MonoBehaviour {

    static SocketIOComponent socket;

    public GameObject playerPrefab;

    Dictionary<string, GameObject> players;

	// Use this for initialization
	void Start ()
    {
	    socket = GetComponent<SocketIOComponent>();
        socket.On("open", onConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);

        players = new Dictionary<string, GameObject> { };
	}


    void onConnected(SocketIOEvent e)
    {
        Debug.Log("connected from unity side");
        //socket.Emit("move");
    }

    void OnSpawned (SocketIOEvent e)
    {
       Debug.Log("spawned"+ e.data);
       var player = Instantiate(playerPrefab);

        players.Add(e.data["id"].ToString (), player);
        Debug.Log("count: " + players.Count);
    }

     void OnMove(SocketIOEvent e)
    {
        Debug.Log("Player is moving" + e.data);
    }

}
