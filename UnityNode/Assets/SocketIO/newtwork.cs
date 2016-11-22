using UnityEngine;
using System.Collections;
using SocketIO;

public class newtwork : MonoBehaviour {

    static SocketIOComponent socket;

	// Use this for initialization
	void Start ()
    {
	    socket = GetComponent<SocketIOComponent>();
        socket.On("open", onConnected);
	}


    void onConnected(SocketIOEvent e)
    {
        Debug.Log("connected from unity side");
        socket.Emit("move");
    }

}
