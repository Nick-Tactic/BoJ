using UnityEngine;
using System.Collections;

//using BoJNameSpace;

public class Uprising : MonoBehaviour {
	public float speed = 7f;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, speed));
	}
		
}
