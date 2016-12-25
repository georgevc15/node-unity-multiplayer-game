using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour, IClickable {

    public GameObject player;

	public void OnClick (RaycastHit hit) {
        var navPos = player.GetComponent<Navigator>();
        var netMove = player.GetComponent<NetworkMove>();
        navPos.NavigateTo(hit.point);

        netMove.OnMove(hit.point);
	}
}
