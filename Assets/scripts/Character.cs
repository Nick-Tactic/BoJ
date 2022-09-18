using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Character : Unit
{

    private Basic_Hero_Attack Protoss_Beam;
    private float Attack_Charging_Timer = 0;

    public bool Charge_Locker, Overshield_Locker;

    private bool Charge_Active, Overshield_Active;

    private float Overshield_Timer, Charge_Timer;

    private void Awake()
    {
        Awake_Procedure();
        Protoss_Beam = Resources.Load<Basic_Hero_Attack>("Protoss_Beam");
        Overshield_Active = true;
        Charge_Active = true;
        Overshield_Timer = 0;
        Charge_Timer = 0;
    }

    private void Attack_Melee()
    {
        State = UnitState.Idle;
        Vector3 position = transform.position; position.y += 0f;
        Basic_Hero_Attack New_Protoss_Beam = Instantiate(Protoss_Beam, position, Protoss_Beam.transform.rotation) as Basic_Hero_Attack;
        New_Protoss_Beam.Parent = gameObject;
        New_Protoss_Beam.Direction = New_Protoss_Beam.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        New_Protoss_Beam.Termination_Time = 0.1F;
        New_Protoss_Beam.Damage = 8;
    }

    private void Attack_Range()
    {
        State = UnitState.Idle;
        Vector3 position = transform.position; position.y += 0f;
        Basic_Hero_Attack New_Protoss_Beam = Instantiate(Protoss_Beam, position, Protoss_Beam.transform.rotation) as Basic_Hero_Attack;
        New_Protoss_Beam.Parent = gameObject;
        New_Protoss_Beam.Direction = New_Protoss_Beam.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        New_Protoss_Beam.Termination_Time = 1.4F;
        New_Protoss_Beam.Damage = 20;
        Current_Energy_Points -= 10;
    }

    private void Overshield()
    {
        Current_Shield_Points += 100;
        Current_Energy_Points -= 50;
        Overshield_Active = false;
        Overshield_Timer = 0;
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    private void Update()
    {

        if (Grounded && State != UnitState.Jump && State != UnitState.Attack_In_Progress) State = UnitState.Idle;
        if (_joystick.Horizontal != 0 && State != UnitState.Attack_In_Progress) Run();
        if (Grounded && _joystick.Vertical > 0 && State != UnitState.Attack_In_Progress) Jump();
        if (Overshield_Locker && Input.GetKeyDown(KeyCode.W) && Overshield_Active && Current_Energy_Points >= 50)
        {
            Overshield();
        }
        
        //Добавь Charge!!!!
               
    }

    public void Prepare_Attack()
    {
        if (Grounded) State = UnitState.Attack_In_Progress;
    }

    public void Unleash_Attack()
    {
        if (Grounded)
        {
            if (Attack_Charging_Timer >= 1 && Current_Energy_Points >= 10)
            {
                Attack_Range();
                Attack_Charging_Timer = 0;
            }
            else
            {
                Attack_Melee();
                Attack_Charging_Timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!Overshield_Active)
        {
            Overshield_Timer += Time.deltaTime;
            if (Overshield_Timer >= 5 && Current_Shield_Points > Maximum_Shield_Points)
            {
                Current_Shield_Points = Maximum_Shield_Points;
            }
            if (Overshield_Timer >= 10)
            {
                Overshield_Active = true;
            }
        }
        if (State == UnitState.Attack_In_Progress) Attack_Charging_Timer += Time.deltaTime;
        CheckGround();
        Combat_Status_Checker();
        Regeneration();
        if (Current_Health_Points <= 0) Application.LoadLevel(Application.loadedLevel);
    }

    void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 25), "Stars:" + score + "/" + total);
        GUI.Box(new Rect(0, 25, 100, 25), "Shield:" + Current_Shield_Points + "/" + Maximum_Shield_Points);
        GUI.Box(new Rect(0, 50, 100, 25), "Health:" + Current_Health_Points + "/" + Maximum_Health_Points);
        GUI.Box(new Rect(0, 75, 100, 25), "Energy:" + Current_Energy_Points + "/" + Maximum_Energy_Points);
        if (Charge_Locker)
        {
            if (Overshield_Active) GUI.Box(new Rect(0, 100, 100, 25), "Charge is redy!");
            else GUI.Box(new Rect(0, 100, 100, 25), "Charge on Cooldown...");
        }
        if (Overshield_Locker)
        {
            if (Overshield_Active && Current_Energy_Points >= 50) GUI.Box(new Rect(0, 125, 150, 25), "Overshild is redy!");
            else GUI.Box(new Rect(0, 125, 150, 25), "Overshild on Cooldown...");
        }
            
    }

}


