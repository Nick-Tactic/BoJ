using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//namespace BoJNameSpace

public class CharacterControllerScript : MonoBehaviour
{
    public int Reg1 = 0;
    public int Reg2 = 0;
    public int HP;
    public int Rage;
    public int Shield;
    public int maxHP;
    public int maxRage;
    public int maxShield;
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    public float TDH_0;
    bool facingRight = true;
    bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public string levelload;
    public string bonuselevelload;
    public int score = 0;
    public int total = 0;
    public Transform starCheck;
    bool stared = false;
    public float starRadius = 0.2f;
    public LayerMask whatIsStar;

    bool CombatStat = true;

    bool ChargeActiviator = true;
    bool ChargeActive = false;
    public float ChargeLoсker = 0;
    public float Charge_TDH;
    public float move_x;

    private Animator HeroAnimation;
    private Rigidbody2D body;
    bool OnEarth;


    // Use this for initialization
    void Start()
    {
        HeroAnimation = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (ChargeActiviator == false)
        {
            Charge_TDH += Time.deltaTime;
            if (Charge_TDH >= 2)
            {
                Charge_TDH = 0;
                ChargeActiviator = true;
            }
        }
        if (ChargeActive)
        {
            Charge_TDH += Time.deltaTime;
            if (Charge_TDH >= 1)
            {
                ChargeActiviator = false;
                ChargeActive = false;
                maxSpeed = maxSpeed / 2;
            }
        }

        if (CombatStat)
        {
            if (Shield < maxShield)
            {
                TDH_0 += Time.deltaTime;
                if (TDH_0 >= 3)
                {
                    Reg1++;
                    Reg2 = Reg1;
                    TDH_0 = 0;
                    if (maxShield - Shield == 1)
                    {
                        Shield++;
                    }
                    else
                    {
                        Shield = Shield + 2;
                    }
                }
            }
        }
        else
        {
            if (Reg1 < Reg2)
            {
                TDH_0 = 0;
            }
            Reg2 = Reg1;
            TDH_0 += Time.deltaTime;
            if (TDH_0 >= 4)
            {
                CombatStat = true;
                TDH_0 = 0;
            }
        }

        if (body.velocity == Vector2.zero && grounded)
        {
            OnEarth = true;
        }

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        stared = Physics2D.OverlapCircle(starCheck.position, starRadius, whatIsStar);

        move_x = Input.GetAxis("Horizontal");

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (stared)
        {
            score++;
            if (col.gameObject.tag == "star")
            {
                Destroy(col.gameObject);
            }
        }
        if (col.gameObject.tag == "Finish")
        {
            if (score < total)
            {
                SceneManager.LoadScene(levelload);
            }
            else
            {
                SceneManager.LoadScene(bonuselevelload);
            }
        }
        if (col.gameObject.tag == "deathzone")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (col.gameObject.tag == "SuicideEnemy")
        {
            Destroy(col.gameObject);
            Reg1--;
            CombatStat = false;
            if (Shield == 0)
            {
                HP--;
            }
            if (Shield > 0)
            {
                Shield--;
            }
        }
        if (col.gameObject.tag == "ChargeUpgraid")
        {
            Destroy(col.gameObject);
            ChargeLoсker = 1;
        }


        if (HP == 0)
            Application.LoadLevel(Application.loadedLevel);

    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "Stars:" + score + "/" + total);
        GUI.Box(new Rect(0, 25, 100, 25), "Shield:" + Shield + "/" + maxShield);
        GUI.Box(new Rect(0, 50, 100, 25), "HP:" + HP + "/" + maxHP);
        GUI.Box(new Rect(0, 75, 100, 25), "Rage:" + Rage + "/" + maxRage);
        if (ChargeLoсker == 1)
        {
            if (ChargeActiviator)
            {
                GUI.Box(new Rect(0, 100, 150, 25), "Charge is ready!");
            }
            else
            {
                GUI.Box(new Rect(0, 100, 150, 25), "Charge on cooldown...");
            }
        }
    }

    // Update is called once per frame

    void Animation()
    {
        if (OnEarth)
        {
            if (move_x != 0 && grounded)
            {
                HeroAnimation.Play("MovementHeroAnimation");
            }
            else if (move_x == 0 && grounded)
            {
                HeroAnimation.Play("Idle");
            }
        }
        else if (grounded)
        {
            HeroAnimation.Play("Jump");
        }
        else
        {
            HeroAnimation.Play("Flight");
        }
    }

    void Update()
    {
        if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            OnEarth = false;
            Animation();
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(move_x * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        Animation();


        if (ChargeLoсker == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (ChargeActiviator)
                {
                    maxSpeed = maxSpeed * 2;
                    ChargeActive = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (ChargeActive)
                {
                    maxSpeed = maxSpeed / 2;
                }

                ChargeActive = false;
                ChargeActiviator = false;
                Charge_TDH = 0;
            }
        }


        if (move_x > 0 && !facingRight)
            Flip();
        else if (move_x < 0 && facingRight)
            Flip();


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
























