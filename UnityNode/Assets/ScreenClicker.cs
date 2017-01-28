using UnityEngine;
using System.Collections;

public class ScreenClicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
            Clicked();

        //simulate hacking
        if (Input.GetButtonDown("Fire1"))
        {
            Network.Move(new Vector3(500, 0, 500), new Vector3(200, 0, 200));
        }
    }


    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit = new RaycastHit();

         if (Physics.Raycast(ray, out hit)) {
            // Debug.Log(hit.collider.gameObject.name);
            var clickable = hit.collider.gameObject.GetComponent<IClickable>();
            clickable.OnClick(hit);
         }
        //Debug.Log("clicked");
    }
}
