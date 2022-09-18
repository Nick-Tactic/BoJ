using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlord : Unit {

    private float Spawn_Timer = 3;
    private Baneling Spawn_Baneling;

    private void Awake()
    {
        Awake_Procedure();
        Spawn_Baneling = Resources.Load<Baneling>("Baneling");
    }

    // Use this for initialization
    void Start () {
		
	}
	
    private void Spawn()
    {
        Spawn_Timer = 3;
        Vector3 position = transform.position; position.y -= 1f;
        Baneling New_Spawn_Baneling = Instantiate(Spawn_Baneling, position, Spawn_Baneling.transform.rotation) as Baneling;
        New_Spawn_Baneling.Parent = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Regeneration();
        if (Spawn_Timer <= 0) Spawn();
        else Spawn_Timer -= Time.deltaTime;
        if (Current_Health_Points <= 0) Destroy(gameObject);
    }
}
