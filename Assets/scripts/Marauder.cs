using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marauder : Unit {

    public LayerMask Who_Is_Enemy;

    private Basic_Hero_Attack Grenade_Attack;

    public int Damage;

    private float Reload_Timer = 1;
    private float Sensor_Radius = 17;

    private void Awake()
    {
        Awake_Procedure();
        Grenade_Attack = Resources.Load<Basic_Hero_Attack>("Grenade_Attack");
    }

    private void Attack()
    {
        Vector3 position = transform.position; position.y += 0f;
        Basic_Hero_Attack New_Grenade_Attack = Instantiate(Grenade_Attack, position, Grenade_Attack.transform.rotation) as Basic_Hero_Attack;
        New_Grenade_Attack.Parent = gameObject;
        New_Grenade_Attack.Direction = New_Grenade_Attack.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        New_Grenade_Attack.Termination_Time = 1.4F;
        New_Grenade_Attack.Damage = Damage;
        Reload_Timer += 1;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, Sensor_Radius, Who_Is_Enemy) && Reload_Timer <= 0) Attack();
        else if (Reload_Timer > 0) Reload_Timer -= Time.deltaTime;
        if (Current_Health_Points <= 0) Destroy(gameObject);
    }
}
