using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baneling : Unit {

    public LayerMask Who_Is_Enemy;

    public float Explosion_Radius;

    private Acid_Splash_Attack Acid_Splash;

    private float Boom_Timer = 0.1F;

    public int Damage;

    public float speed = 7f;
    float direction = -1f;
    public string Turner_0;
    public string Turner_1;

    private GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == Turner_1)
            direction *= -1f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == Turner_0)
            direction *= -1f;
    }

    private void Awake()
    {
        Awake_Procedure();
        Acid_Splash = Resources.Load<Acid_Splash_Attack>("Acid Splash");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
    private void Attack_Suicide ()
    {
        Vector3 position = transform.position; position.y += 0f;
        Acid_Splash_Attack New_Acid_Splash = Instantiate(Acid_Splash, position, Acid_Splash.transform.rotation) as Acid_Splash_Attack;
        New_Acid_Splash.Parent = gameObject;
        New_Acid_Splash.Damage = Damage;
        New_Acid_Splash.Direction = New_Acid_Splash.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        if (Grounded)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, GetComponent<Rigidbody2D>().velocity.y);
            transform.localScale = new Vector3(direction, 1, -1);
        }
    }

    private void FixedUpdate()
    {
        if ((Physics2D.OverlapCircle(transform.position, Explosion_Radius, Who_Is_Enemy) || Current_Health_Points <= 0) && Boom_Timer <= 0) Attack_Suicide();
        else if (Physics2D.OverlapCircle(transform.position, Explosion_Radius, Who_Is_Enemy) || Current_Health_Points <= 0) Boom_Timer -= Time.deltaTime;
    }

}
