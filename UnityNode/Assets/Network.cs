using UnityEngine;
using System.Collections.Generic;
using SocketIO;
using System;

public class Network : MonoBehaviour {

    static SocketIOComponent socket;

    public GameObject myPlayer;

    public Spawner spawner;

	// Use this for initialization
	void Start ()
    {
	    socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("follow", OnFollow);
        socket.On("attack", OnAttack);
        socket.On("registered", OnRegistered);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);

	}


    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected from unity side");
        //socket.Emit("move");
    }

    void OnRegister(SocketIOEvent e)
    {
        Debug.Log("Succesfully registerd with id" + e.data);
        spawner.AddPlayer(e.data["id"].str, myPlayer);
    }

    void OnSpawned (SocketIOEvent e)
    {
       Debug.Log("spawned"+ e.data);
       var player = spawner.SpawnPlayer(e.data["id"].str);

        if (e.data["x"])
        {
            Vector3 movePosition = GetVectorFromJson(e);

            var navigatePos = player.GetComponent<Navigator>();

            navigatePos.NavigateTo(movePosition);
        }
    }


    void OnMove(SocketIOEvent e)
    {
        Debug.Log("Player is moving" + e.data);

        var position = GetVectorFromJson(e);

        var player = spawner.FindPlayer(e.data["id"].str);

        var navigatePos = player.GetComponent<Navigator>();

        navigatePos.NavigateTo(position);

    }

    void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow  request"+ e.data);

        var player = spawner.FindPlayer(e.data["id"].str);

        var targetTransform = spawner.FindPlayer(e.data["targetId"].str).transform;

        var target = player.GetComponent<Targeter>();

        target.target = targetTransform;

    }

    private void OnAttack(SocketIOEvent e)
    {
        Debug.Log("received attack" + e.data);
    }

    void OnRegistered(SocketIOEvent e)
    {
        Debug.Log("registered id " + e.data);
        
    }


    void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected" + e.data);

        var id = e.data["id"].str;

        spawner.Remove(id);
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("updating position"+ e.data);

        var position = GetVectorFromJson(e);

        var player = spawner.FindPlayer(e.data["id"].str);

        player.transform.position = position;

    }


    static public void Move(Vector3 position)
    {
        //send position to node
        Debug.Log("sending position to node" + Network.VectorToJson(position));
        socket.Emit("move", Network.VectorToJson(position));
    }


    static public void Follow(string id)
    {
        //send position to node
        Debug.Log("sending follow player id " + Network.PLayerIdToJson(id));
        socket.Emit("follow", Network.PLayerIdToJson(id));
    }

    static public void Attack(string targetId)
    {
        Debug.Log("attacking player" + Network.PLayerIdToJson(targetId));
        socket.Emit("attack", Network.PLayerIdToJson(targetId));
    }


    void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("server is requestig position ");
        socket.Emit("updatePosition", VectorToJson(myPlayer.transform.position));
    }


    private static Vector3 GetVectorFromJson(SocketIOEvent e)
    {
        return new Vector3(e.data["x"].n, 0, e.data["y"].n);
    }


    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("x", vector.x);
        j.AddField("y", vector.y);
        return j;
    }

    public static JSONObject PLayerIdToJson(string id)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("targetId", id);
        return j;
    }
}
