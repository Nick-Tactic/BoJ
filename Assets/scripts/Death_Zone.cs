using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Zone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponent<Unit>();
        if (unit)
        {
            unit.Current_Shield_Points = 0;
            unit.Current_Health_Points = 0;
        }
    }
}
