using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {

    public float attackDistance;
    public float attackRate;

    float lastAttackTime = 0;

    Targeter targeter;

	// Use this for initialization
	void Start () {
        targeter = GetComponent<Targeter>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isReadyToAttack() && targeter.IsInRange(attackDistance))
        {
            Debug.Log("attacking");
            lastAttackTime = Time.time;
        }
	}

    bool isReadyToAttack()
    {
        return Time.time - lastAttackTime > attackRate && targeter.target;
    }
}
