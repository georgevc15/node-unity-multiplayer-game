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
        socket.On("registered", OnRegistered);
        socket.On("disconnected", OnDisconnected);

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

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));
        
        var player = players [e.data["id"].ToString()];

        var navigatePos = player.GetComponent<NavigatePosition>();

        navigatePos.NavigateTo(position);

    }

    void OnRegistered(SocketIOEvent e)
    {
        Debug.Log("registered id " + e.data);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data["x"].ToString().Replace("\"", ""));
    }

    void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected" + e.data);

        var id = e.data["id"].ToString();

        var player = players [id];
        Destroy(player);

        players.Remove(id);
    }
}
