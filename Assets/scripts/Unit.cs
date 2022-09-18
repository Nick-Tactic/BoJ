using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Unit : MonoBehaviour
{
    [SerializeField] public FixedJoystick _joystick;

    public int Current_Health_Points, Maximum_Health_Points, Current_Shield_Points, Maximum_Shield_Points, Current_Energy_Points, Maximum_Energy_Points;

    public float Ground_Radius;

    public LayerMask What_Is_Ground;

    public bool Health_Points_Regenerative, Shield_Points_Regenerative, Energy_Points_Regenerative;

    private float Health_Points_Regeneration_Timer, Shield_Points_Regeneration_Timer, Energy_Points_Regeneration_Timer;

    protected bool Combat_Status = false, Grounded = false;

    private int Combat_Status_Numer = 0;

    private float Combat_Status_Timer = 0;

    public float Maximum_Speed = 1000f;
    public float jumpForce = 1000f;

    new protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected SpriteRenderer sprite;

    protected void Awake_Procedure()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Combat_Status_Numer = Current_Health_Points + Current_Shield_Points;
    }

    protected UnitState State
    {
        get { return (UnitState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    public void Take_Damage(int Damage)
    {
        if(Damage > Current_Shield_Points)
        {
            Current_Health_Points -= Damage - Current_Shield_Points;
            Current_Shield_Points = 0;
        }
        else
        {
            Current_Shield_Points -= Damage;
        }
    }

    protected void CheckGround()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);
        Grounded = Physics2D.OverlapCircle(transform.position, Ground_Radius, What_Is_Ground);
        //Grounded = colliders.Length > Ground_Radius;

        if (!Grounded) State = UnitState.Flight;
    }

    protected void Combat_Status_Checker()
    {
        if (Combat_Status && Combat_Status_Numer <= Current_Health_Points + Current_Shield_Points)
        {
            Combat_Status_Numer = Current_Health_Points + Current_Shield_Points;
            if (Combat_Status_Timer >= 7)
            {
                Combat_Status = false;
                Combat_Status_Timer = 0;
            }
            else
            {
                Combat_Status_Timer += Time.deltaTime;
            }
        }
        else if(Combat_Status_Numer > Current_Shield_Points + Current_Health_Points)
        {
            Combat_Status_Numer = Current_Health_Points + Current_Shield_Points;
            Combat_Status = true;
            Combat_Status_Timer = 0;
        }
    }

    protected void Jump()
    {
        State = UnitState.Jump;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    protected void Run()
    {
        Vector3 direction = transform.right * _joystick.Horizontal;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Maximum_Speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (Grounded && State != UnitState.Jump) State = UnitState.Run;
    }

    private void Shield_Points_Regeneration()
    {
        if (!Combat_Status)
        {
            if (Shield_Points_Regeneration_Timer <= 0)
            {
                Current_Shield_Points++;
                Shield_Points_Regeneration_Timer = 3;
            }
            else
            {
                Shield_Points_Regeneration_Timer -= Time.deltaTime;
            }
        }
        else
        {
            Shield_Points_Regeneration_Timer = 0;
        }
    }

    private void Health_Points_Regeneration()
    {
        if (Health_Points_Regeneration_Timer <= 0)
        {
            Current_Health_Points++;
            Health_Points_Regeneration_Timer = 3;
        }
        else
        {
            Health_Points_Regeneration_Timer -= Time.deltaTime;
        }
    }

    private void Energy_Points_Regeneration()
    {
        if (Energy_Points_Regeneration_Timer <= 0)
        {
            Current_Energy_Points++;
            Energy_Points_Regeneration_Timer = 1;
        }
        else
        {
            Energy_Points_Regeneration_Timer -= Time.deltaTime;
        }
    }

    protected void Regeneration()
    {
        if (Current_Energy_Points < Maximum_Energy_Points && Energy_Points_Regenerative) Energy_Points_Regeneration();
        else Energy_Points_Regeneration_Timer = 0;
        if (Current_Health_Points < Maximum_Health_Points && Health_Points_Regenerative) Health_Points_Regeneration();
        else Health_Points_Regeneration_Timer = 0;
        if (Current_Shield_Points < Maximum_Shield_Points && Shield_Points_Regenerative) Shield_Points_Regeneration();
    }

}

public enum UnitState
{
    Idle,
    Run,
    Jump,
    Flight,
    Attack_In_Progress
}
