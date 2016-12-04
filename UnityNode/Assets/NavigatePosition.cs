using UnityEngine;
using System.Collections;

public class NavigatePosition : MonoBehaviour {

    NavMeshAgent agent;
    
    // Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	public void NavigateTo (Vector3 position) {
        agent.SetDestination(position);
	}

    void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", agent.remainingDistance);
    }
}
