﻿using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour {

    NavMeshAgent agent;
    Targeter targeter;
    
    // Use this for initialization
	void Awake () {
        agent = GetComponent<NavMeshAgent> ();
        targeter = GetComponent<Targeter>();
	}
	
	// Update is called once per frame
	public void NavigateTo (Vector3 position) {

        agent.SetDestination(position);
        targeter.target = null;
        animator.SetBool("Attack", false);
    }

    void Update()
    {
        animator.SetFloat("Distance", agent.remainingDistance);
    }
}
