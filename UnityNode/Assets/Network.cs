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
            var movePosition = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

            var navigatePos = player.GetComponent<Navigator>();

            navigatePos.NavigateTo(movePosition);
        }
    }

     void OnMove(SocketIOEvent e)
    {
        Debug.Log("Player is moving" + e.data);

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));
        
        var player = spawner.FindPlayer(e.data["id"].str);

        var navigatePos = player.GetComponent<Navigator>();

        navigatePos.NavigateTo(position);

    }

    void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow  request"+ e.data);

        var player = spawner.FindPlayer(e.data["id"].ToString());

        var target = spawner.FindPlayer(e.data["targetId"].str);

        var follower = player.GetComponent<Follower>();

        follower.target = target.transform;


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

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

        var player = spawner.FindPlayer(e.data["id"].str);

        player.transform.position = position;


    }


    void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("server is requestig position ");
        socket.Emit("updatePosition", new JSONObject(VectorToJson(myPlayer.transform.position)));
    }


    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].str);
    }


    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.y);
    }

    public static string PLayerIdToJson(string id)
    {
        return string.Format(@"{{""targetId"":""{0}""}}", id);
    }
}
