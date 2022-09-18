using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using BoJNameSpace;

public class RotationScript : MonoBehaviour {
	public float xspeed = 0.04f;
	public float yspeed = 0.04f;
	public float zspeed = 0.04f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (xspeed * Time.deltaTime,  yspeed * Time.deltaTime, zspeed * Time.deltaTime));
	}
}
