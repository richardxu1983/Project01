using UnityEngine;
using System.Collections;

public class Env : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    //FixedUpdate should be used instead of Update when dealing with Rigidbody.For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedUpdate instead of every frame inside Update.
    void FixedUpdate()
    {

    }
}
