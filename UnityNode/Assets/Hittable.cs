using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

    public float health = 100;
    public float respawnTime = 5;

    public bool isDead {
        get {
            return health <= 0;
        }
    }

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnHit()
    {
        health -= 10;
        if(isDead)
        {
            animator.SetTrigger("Dead");
            Invoke("Spawn", respawnTime);
        }
    }

    void Spawn()
    {
        Debug.Log("Spawning my player");
        transform.position = Vector3.zero;
        health = 100;
        animator.SetTrigger("Spawn");
    }

}
