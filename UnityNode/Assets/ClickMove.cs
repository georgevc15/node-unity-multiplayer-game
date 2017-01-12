using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour, IClickable {

    public GameObject player;

	public void OnClick (RaycastHit hit) {
        var navigator = player.GetComponent<Navigator>();

        navigator.NavigateTo(hit.point);

        Network.Move(hit.point);
	}
}
